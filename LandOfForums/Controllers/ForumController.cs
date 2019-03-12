using System;
using System.Collections.Generic;
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

        public ForumController(IForum forumService)
        {
            _forumService = forumService;
        }

        public IActionResult Index()
        {

            IEnumerable<ForumListingModel> forums = _forumService.GetAll()
                .Select(forum => new ForumListingModel {
                    Id = forum.Id,
                    Name = forum.Title,
                    Description = forum.Description
                });
            var model = new ForumIndexModel
            {
                ForumList = forums
            };

            return View(model);
        }
        public IActionResult Topic(int id)
        {
            var forum = _forumService.GetById(id);
            var posts = forum.Posts;

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

        private ForumListingModel BuildForumListing(Post post)
        {
            var forum = post.Forum;

            return BuildForumListing(forum);
        }
        private ForumListingModel BuildForumListing(Forum forum)
        {
            return new ForumListingModel
            {
                Id = forum.Id,
                Name = forum.Title,
                Description = forum.Description,
                ImageURL = forum.ImageURL,
            };
        }
    }
}