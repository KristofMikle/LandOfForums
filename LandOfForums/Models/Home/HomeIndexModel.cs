using LandOfForums.Models.Post;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LandOfForums.Models.Home
{
    public class HomeIndexModel
    {
        public string SearchQuerry { get; set; }
        public IEnumerable<PostListingModel> LatestPosts { get; set; }
    }
}
