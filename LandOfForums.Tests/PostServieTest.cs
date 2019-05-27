using LandOfForums.Data;
using LandOfForums.Data.Models;
using LandOfForums.Service;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace LandOfForums.Tests
{
    public class PostServieTest
    {
        [Fact]
        public async Task Add_WritesToDatabase()
        {
            //Arrange
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "Add_Dtabase")
                .Options;

            //Act
            // Run the test against one instance of the context
            using (var context = new ApplicationDbContext(options))
            {
                var postService = new PostService(context);
                await postService.Add(new Post());
            }

            // Assert
            // Use a separate instance of the context to verify correct data was saved to database
            using (var context = new ApplicationDbContext(options))
            {
                Assert.Equal(expected: 1, actual: context.Posts.Count());
            }
        }

        [Fact]
        public async Task AddReply_WritesToDatabase()
        {
            //Arrange
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "AddReply_Dtabase")
                .Options;

            //Act
            // Run the test against one instance of the context
            using (var context = new ApplicationDbContext(options))
            {
                var postService = new PostService(context);
                await postService.AddReply(new PostReply());
            }

            // Assert
            // Use a separate instance of the context to verify correct data was saved to database
            using (var context = new ApplicationDbContext(options))
            {
                Assert.Equal(expected: 1, actual: context.PostReplies.Count());
            }
        }

        [Theory]
        [InlineData(1, "GetAll_first")]
        [InlineData(2, "GetAll_second")]
        [InlineData(3, "GetAll_third")]
        public void GetAll_ReturnsAllPosts(int expectedAmount, string databaseName)
        {
            //Arrange
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: databaseName)
                .Options;

            using (var context = new ApplicationDbContext(options))
            {
                for (int i = 0; i < expectedAmount; i++)
                {
                    context.Posts.Add(new Post());
                }
                context.SaveChanges();
            }
            List<Post> actual;

            //Act
            using (var context = new ApplicationDbContext(options))
            {
                var postService = new PostService(context);
                actual = postService.GetAll().ToList();
            }

            //Assert
            Assert.Equal(expectedAmount, actual.Count());
        }

        [Fact]
        public void GetById_GivenCorrenctId_ReturnCorrespondingPost()
        {
            //Arrange
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "GetById_Database")
                .Options;

            using (var context = new ApplicationDbContext(options))
            {
                context.Posts.Add(new Post()
                {
                    Id = 1
                });
                context.SaveChanges();
            }
            int expedtedId = 1;
            Post actual;

            //Act
            using (var context = new ApplicationDbContext(options))
            {
                var postService = new PostService(context);
                actual = postService.GetById(expedtedId);
            }

            //Assert
            Assert.Equal(expedtedId, actual.Id);
        }
    }
}
