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

namespace LandOfForums.Controllers
{
    public class ForumController : Controller
    {
        private readonly IForum _forumService;
        private IPost _postService;

        public ForumController(IForum forumService, IPost postService)
        {
            _forumService = forumService;
            _postService = postService;
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

        public IActionResult Topic(int id, string searchquery)
        {
            var forum = _forumService.GetById(id);

            var posts = new List<Post>();
            if (!String.IsNullOrEmpty(searchquery))
            {
                posts = _postService.GetFilteredPosts(id, searchquery).ToList();
            }
            else
            {
                posts = forum.Posts.ToList();
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
            });

            var model = new ForumTopicModel
            {
                Posts = postList,
                Forum = BuildForumListing(forum)
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
                //var blockBlob = PostForumImage(model.ImageUpload);
                //imageUri = blockBlob.Uri.AbsoluteUri;
                imageUri = "/images/users/default.png";
            }
            else
            {
                imageUri = "/images/users/default.png";
            }

            var forum = new Data.Models.Forum()
            {
                Title = model.Title,
                Description = model.Description,
                Created = DateTime.Now,
                ImageUrl = imageUri
            };

            await _forumService.Add(forum);
            return RedirectToAction("Index", "Forum");
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

            return new PostListingModel();
        }
    }
}