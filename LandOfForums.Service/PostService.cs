using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LandOfForums.Data;
using LandOfForums.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace LandOfForums.Service
{
    public class PostService : IPost
    {
        private readonly ApplicationDbContext _context;
        public PostService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task Add(Post post)
        {
            _context.Add(post);
            await _context.SaveChangesAsync();
        }

        public Task AddRepy(PostReply reply)
        {
            throw new NotImplementedException();
        }

        public Task EditPost(int id, string newContent)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Post> GetAll()
        {
            return _context.Posts
                .Include(post => post.User)
                .Include(post => post.Replies)
                    .ThenInclude(reply => reply.User)
                .Include(post => post.Forum);
        }

        public Post GetById(int id)
        {
            return _context.Posts.Where(post => post.Id == id)
                .Include(post => post.User)
                .Include(post => post.Replies)
                    .ThenInclude(reply => reply.User)
                .Include(post => post.Forum)
                .First();
        }

        public IEnumerable<Post> GetFilteredPosts(int id, string searchQuerry)
        {
            var forum = _context.Forums.Find(id);
            return String.IsNullOrEmpty(searchQuerry) ? forum.Posts : forum.Posts
                .Where(post => post.Title.Contains(searchQuerry) || post.Content.Contains(searchQuerry));
        }

        public IEnumerable<Post> GetLatestsPosts(int v)
        {
            return GetAll().OrderByDescending(post => post.Created).Take(v);
        }

        public IEnumerable<Post> GetPostsByForum(int id)
        {
            return _context.Forums
                .Where(forum => forum.Id == id)
                .First()
                .Posts;
        }

        public Task Remove(int id)
        {
            throw new NotImplementedException();
        }
    }
}
