﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LandOfForums.Models.Forum;

namespace LandOfForums.Models.Post
{
    public class PostListingModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string DatePosted { get; set; }
        public string AuthorName { get; set; }
        public string AuthorRating { get; set; }
        public string AuthorId { get; set; }

        public int ForumId { get; set; }
        public string ForumImageURL { get; set; }
        public string ForumName { get; set; }

        public int ReplyCount { get; set; }

        public ForumListingModel Forum { get; set; }

    }
}