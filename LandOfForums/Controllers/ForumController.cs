using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using LandOfForums.Data;
using LandOfForums.Data.Models;
using LandOfForums.Models.Forum;
using LandOfForums.Models.Post;
using LandOfForums.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Net.Http.Headers;
using Microsoft.WindowsAzure.Storage.Blob;
using System.IO;

namespace LandOfForums.Controllers
{
    public class ForumController : Controller
    {
        private readonly IForum _forumService;
        private IPost _postService;
        private readonly IUpload _uploadService;
        private readonly IConfiguration _configuration;

        public ForumController(IForum forumService, IPost postService, IConfiguration configuration, IUpload uploadService)
        {
            _forumService = forumService;
            _postService = postService;
            _configuration = configuration;
            _uploadService = uploadService;
        }

        public IActionResult Index()
        {
            IEnumerable<ForumListingModel> forums = _forumService.GetAll()
                .Select(forum => new ForumListingModel {
                    Id = forum.Id,
                    Name = forum.Title,
                    Description = forum.Description,
                    NumberOfPosts = forum.Posts?.Count() ?? 0,
                    Latest = GetLatestPost(forum.Id) ?? new PostListingModel(),
                    NumberOfUsers = _forumService.GetActiveUsers(forum.Id).Count(),
                    ImageUrl = forum.ImageUrl,
                    HasRecentPost = _forumService.HasRecentPost(forum.Id)
                });
            var model = new ForumIndexModel
            {
                ForumList = forums
            };

            return View(model);
        }

        public IActionResult Topic(int id, string searchQuery)
        {
            var forum = _forumService.GetById(id);

            var posts = new List<Post>();
            if (String.IsNullOrEmpty(searchQuery))
            {
                posts = forum.Posts.ToList();
            }
            else
            {
                posts = _postService.GetFilteredPosts(id, searchQuery).ToList();
            }

            var postList = posts.Select(post => new PostListingModel
            {
                Id = post.Id,
                AuthorName = post.User.UserName,
                AuthorId = post.User.Id,
                AuthorRating = post.User.Rating,
                Title = post.Title,
                DatePosted = post.Created.ToString(),
                ReplyCount = post.Replies.Count(),
                Forum = BuildForumListing(post)
            }).ToList<PostListingModel>();

            var model = new ForumTopicModel
            {
                Posts = postList,
                Forum = BuildForumListing(forum),
                SearchQuery = searchQuery
            };
            return View(model);
        }

        [HttpPost]
        public IActionResult Search(int id, string searchquery)
        {
            return RedirectToAction("Topic", new { id, searchquery });
        }

        public static ForumListingModel BuildForumListing(Post post)
        {
            var forum = post.Forum;

            return BuildForumListing(forum);
        }

        private static ForumListingModel BuildForumListing(Forum forum)
        {
            return new ForumListingModel
            {
                Id = forum.Id,
                Name = forum.Title,
                Description = forum.Description,
                ImageUrl = forum.ImageUrl
            };
        }

        public IActionResult Delete(int id)
        {
            _forumService.Delete(id);
            return RedirectToAction("Index", "Forum");
        }

        public IActionResult Create()
        {
            var model = new AddForumModel();
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> AddForum(AddForumModel model)
        {
            var imageUri = "";

            if (model.ImageUpload != null)
            {
                var blockBlob = GetUploadedImage(model.ImageUpload);
                imageUri = blockBlob.Result.Uri.AbsoluteUri;
            }
            else
            {
                imageUri = "/images/forum/default.png";
            }

            var forum = new Forum()
            {
                Title = model.Title,
                Description = model.Description,
                Created = DateTime.Now,
                ImageUrl = imageUri
            };

            await _forumService.Add(forum);
            return RedirectToAction("Index", "Forum");
        }

        public async Task<CloudBlockBlob> GetUploadedImage(IFormFile file)
        {
            var connectionString = _configuration["AzureStorageAccountConnectionString"];
            var container = _uploadService.GetBlobContainer(connectionString);
            var parsedContentDisposition = ContentDispositionHeaderValue.Parse(file.ContentDisposition);
            var filename = Path.Combine(parsedContentDisposition.FileName.ToString().Trim('"'));
            var blockBlob = container.GetBlockBlobReference(filename);
            await blockBlob.UploadFromStreamAsync(file.OpenReadStream());

            return blockBlob;
        }

        public PostListingModel GetLatestPost(int forumId)
        {
            var post = _forumService.GetLatestPost(forumId);

            if (post != null)
            {
                return new PostListingModel
                {
                    AuthorName = post.User != null ? post.User.UserName : "",
                    DatePosted = post.Created.ToString(CultureInfo.InvariantCulture),
                    Title = post.Title ?? ""
                };
            }

            return null;
        }
    }
}