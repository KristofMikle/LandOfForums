using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LandOfForums.Data.Models;
using LandOfForums.Models.Post;
using LandOfForums.Models.Reply;
using LandOfForums.Service;
using Microsoft.AspNetCore.Mvc;

namespace LandOfForums.Controllers
{
    public class PostController : Controller
    {
        private readonly IPost _postService;
        public PostController(IPost postService)
        {
            _postService = postService;
        }

        public IActionResult Index(int id)
        {
            var post = _postService.GetById(id);

            var replies = BuildPostReplies(post.Replies);

            var model = new PostIndexModel
            {
                Id = post.Id,
                Title = post.Title,
                AuthorId = post.User.Id,
                AuthorName = post.User.UserName,
                AuthorImageURL = post.User.ProfileImageURL,
                AuthorRating = post.User.Rating,
                Created = post.Created,
                Content = post.Content,
                Replies = replies
            };
            return View(model);
        }

        private IEnumerable<PostReplyModel> BuildPostReplies(IEnumerable<PostReply> replies)
        {
            return replies.Select(reply => new PostReplyModel
            {
                Id = reply.Id,
                AuthorName = reply.User.UserName,
                AuthorId = reply.User.Id,
                AuthorImageURL = reply.User.ProfileImageURL,
                AuthorRating = reply.User.Rating,
                Content = reply.Content,
                Created = reply.Created,
                PostId = reply.Id
            });
        }
    }
}