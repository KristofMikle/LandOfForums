using System;
using System.Collections.Generic;

namespace LandOfForums.Data.Models
{
    public class Forum
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string ImageURL { get; set; }
        public DateTime Created { get; set; }

        public virtual IEnumerable<Post> Posts { get; set; }
    }
}
