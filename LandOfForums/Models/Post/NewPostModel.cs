using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LandOfForums.Models.Post
{
    public class NewPostModel
    {
        public string Title { get; set; }
        public string Content { get; set; }
        public int ForumId { get; set; }
        public string ForumName { get; set; }
        public string AuthorName { get; set; }
        public string ForumImageUrl { get; set; }
    }
}
