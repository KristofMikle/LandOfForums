using System;
using Xunit;
using LandOfForums.Controllers;
using LandOfForums.Service;
using LandOfForums.Data;

namespace LandOfForums.Tests
{
    public class UnitTest1
    {
        [Fact]
        public void BuildHomeIndexModel_ReturnsNotNull()
        {
            var homeController = new HomeController(new PostService(new ApplicationDbContext()));
        }
    }
}
