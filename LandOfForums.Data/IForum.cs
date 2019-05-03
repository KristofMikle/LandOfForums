using LandOfForums.Data.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LandOfForums.Data
{
    public interface IForum
    {
        Forum GetById(int id);
        IEnumerable<Forum> GetAll();
        
        Task Delete(int forumId);
        Task UpdateForumTitle(int forumId, string newTitle);
        Task UpdateForumDescription(int forumId, string newDescription);

        Task Add(Forum forum);
        Post GetLatestPost(int forumId);
        IEnumerable<ApplicationUser> GetActiveUsers(int forumId);
        bool HasRecentPost(int id);
    }
}
