using System;
using Xunit;
using LandOfForums.Controllers;
using NSubstitute;
using LandOfForums.Data;
using Microsoft.Extensions.Configuration;
using LandOfForums.Service;
using Microsoft.AspNetCore.Mvc;
using LandOfForums.Models.Forum;
using System.Collections.Generic;
using LandOfForums.Data.Models;
using System.Linq;
using LandOfForums.Models.Post;

namespace LandOfForums.Tests
{
    public class ForumControllerTest
    {
        private ForumController _controller;
        private IForum _forumService;
        private IPost _postService;
        private IConfiguration _cofigService;
        private IUpload _uploadService;

        public ForumControllerTest()
        {
            _forumService = Substitute.For<IForum>();
            _postService = Substitute.For<IPost>();
            _cofigService = Substitute.For<IConfiguration>();
            _uploadService = Substitute.For<IUpload>();
            _controller = new ForumController(_forumService, _postService, _cofigService, _uploadService);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        public void Index_GivenForums_ReturnsTheForums(int expectedAmount)
        {
            //Arrange
            List<Forum> forumsToReturn = new List<Forum>();
            for (int i = 0; i < expectedAmount; i++)
            {
                forumsToReturn.Add(new Forum());
            }
            _forumService.GetAll().Returns(forumsToReturn);

            //Act
            ViewResult view = (ViewResult)_controller.Index();
            ForumIndexModel model = (ForumIndexModel) view.Model;

            //Assert
            Assert.Equal(expectedAmount, model.ForumList.Count());
        }

        [Fact]
        public void Topic_WithSearchQuery_ReturnsFilteredPosts()
        {
            //Arrange
            var forumToReturn = new Forum();
            var expectedPost = new Post() { Title = "Filtered Post", Forum= forumToReturn, User= new ApplicationUser(), Replies= new List<PostReply>() };
            var listToReturn = new Post[]{ expectedPost };
            _postService.GetFilteredPosts(1, "query").Returns(listToReturn);
            _forumService.GetById(1).Returns(forumToReturn);

            //Act
            ViewResult view = (ViewResult)_controller.Topic(1, "query");
            ForumTopicModel model = (ForumTopicModel)view.Model;
            var actualPost = model.Posts.First();

            //Assert
            Assert.Equal(expectedPost.Title, actualPost.Title);
        }

        [Fact]
        public void GetLatestPost_ForumHasNoPosts_ReturnsNull()
        {
            //Arrange
            _forumService.GetLatestPost(1).Returns(l => null);

            //Act
            PostListingModel actual =_controller.GetLatestPost(1);

            //Assert
            Assert.Null(actual);
        }

        [Fact]
        public void GetLatestPost_ForumHasPost_ReturnsAPost()
        {
            //Arrange
            _forumService.GetLatestPost(1).Returns(new Post());

            //Act
            PostListingModel actual = _controller.GetLatestPost(1);

            //Assert
            Assert.NotNull(actual);
        }
    }
}
