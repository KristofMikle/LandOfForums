using LandOfForums.Data.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LandOfForums.Service
{
    public interface IPost
    {
        Post GetById(int id);
        IEnumerable<Post> GetAll();
        IEnumerable<Post> GetFilteredPosts(int id, string searchQuery);
        IEnumerable<Post> GetFilteredPosts(string searchQuery);
        IEnumerable<Post> GetPostsByForum(int id);

        Task Add(Post post);
        Task Remove(int id);
        Task EditPost(int id, string newContent);
        Task AddReply(PostReply reply);
        IEnumerable<Post> GetLatestsPosts(int v);
        IEnumerable<ApplicationUser> GetAllUsers(IEnumerable<Post> posts);
    }
}
