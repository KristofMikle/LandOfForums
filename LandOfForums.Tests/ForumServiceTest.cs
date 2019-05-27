using LandOfForums.Data;
using LandOfForums.Data.Models;
using LandOfForums.Service;
using Microsoft.EntityFrameworkCore;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace LandOfForums.Tests
{
    public class ForumServiceTest
    {
        private IPost _postService;

        public ForumServiceTest()
        {
            _postService = Substitute.For<IPost>();
        }

        [Theory]
        [InlineData(1, "GetAll_first")]
        [InlineData(2, "GetAll_second")]
        [InlineData(3, "GetAll_third")]
        public void GetAll_ReturnsAllFoums(int expectedAmount, string databaseName)
        {
            //Arrange
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: databaseName)
                .Options;

            using (var context = new ApplicationDbContext(options))
            {
                for (int i = 0; i < expectedAmount; i++)
                {
                    context.Forums.Add(new Forum());
                }
                context.SaveChanges();
            }
            List<Forum> actual;

            //Act
            using (var context = new ApplicationDbContext(options))
            {
                var forumService = new ForumService(context, _postService);
                actual = forumService.GetAll().ToList();
            }

            //Assert
            Assert.Equal(expectedAmount, actual.Count());
        }

        [Fact]
        public async Task Delete_GivenCorrenctId_RemovesCorrespondingForum()
        {
            //Arrange
            int forumId = 1;
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "Delete_Database")
                .Options;

            using (var context = new ApplicationDbContext(options))
            {
                context.Forums.Add(new Forum()
                {
                    Id = forumId
                });
                context.SaveChanges();
            }

            //Act
            using (var context = new ApplicationDbContext(options))
            {
                var forumService = new ForumService(context, _postService);
                await forumService.Delete(forumId);
            }

            //Assert
            using (var context = new ApplicationDbContext(options))
            {
                Assert.Equal(expected: 0, actual: context.Forums.Count());
            }
        }

        [Fact]
        public void GetById_GivenCorrenctId_ReturnCorrespondingForum()
        {
            //Arrange
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "GetById_Database")
                .Options;

            using (var context = new ApplicationDbContext(options))
            {
                context.Forums.Add(new Forum()
                {
                    Id = 1
                });
                context.SaveChanges();
            }
            int expedtedId = 1;
            Forum actual;

            //Act
            using (var context = new ApplicationDbContext(options))
            {
                var forumService = new ForumService(context, _postService);
                actual = forumService.GetById(expedtedId);
            }

            //Assert
            Assert.Equal(expedtedId, actual.Id);
        }
        
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
                var forumService = new ForumService(context, _postService);
                await forumService.Add(new Forum());
            }

            // Assert
            // Use a separate instance of the context to verify correct data was saved to database
            using (var context = new ApplicationDbContext(options))
            {
                Assert.Equal(expected: 1, actual: context.Forums.Count());
            }
        }
    }
}
