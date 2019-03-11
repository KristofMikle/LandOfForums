using Microsoft.AspNetCore.Identity;
using System;

namespace LandOfForums.Data.Models
{
    // Add profile data for application users by adding properties to the ApplicationUser class
    public class ApplicationUser : IdentityUser
    {
        public int Rating { get; set; }
        public string ProfileImageURL { get; set; }
        public DateTime Created { get; set; }
    }
}
