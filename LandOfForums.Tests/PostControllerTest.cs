using Castle.Core.Configuration;
using LandOfForums.Controllers;
using LandOfForums.Data;
using LandOfForums.Data.Models;
using LandOfForums.Models.Post;
using LandOfForums.Service;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace LandOfForums.Tests
{
    public class PostControllerTest
    {
        private PostController _controller;
        private IForum _forumService;
        private IPost _postService;
        private UserManager<ApplicationUser> _userService;

        public PostControllerTest()
        {
            _forumService = Substitute.For<IForum>();
            _postService = Substitute.For<IPost>();
            var mockUserStore = new Mock<IUserStore<ApplicationUser>>();
            _userService = new UserManager<ApplicationUser>(mockUserStore.Object,null,null,null,null,null,null,null,null);
            _controller = new PostController( _postService, _forumService, _userService);
        }

        [Fact]
        public void Index_GivenOnePost_ReturnedModelHasItsPropertiesFilled()
        {
            //Arrange
            var expectedUserData = new ApplicationUser() { Created = DateTime.Now, Id = "1", ProfileImageUrl = "realUrl", UserName = "Pista", Rating = 1 };
            var expectedForumData = new Forum() { Id = 1, Title = "Forum Title" };
            var expectedPostData = new Post();      //needed to declare Post to be able to put a reference in reply
            var expectedReplyData = new List<PostReply>() { new PostReply() { User = expectedUserData, Content = "Reply Content", Created = DateTime.Now, Forum = expectedForumData, Id = 1, Post = expectedPostData } };
            expectedPostData = new Post() { Content = "Post Content", Created = DateTime.Now, Id = 1, Title = "Returned Post", Replies = expectedReplyData, User = expectedUserData, Forum = expectedForumData };
            _postService.GetById(1).Returns(expectedPostData);

            //Act
            ViewResult view = (ViewResult)_controller.Index(1);
            PostIndexModel actualPostData = (PostIndexModel)view.Model;

            //Assert
            Assert.Equal(expectedForumData.Id, actualPostData.ForumId);
            Assert.Equal(expectedForumData.Title, actualPostData.ForumName);
            Assert.Equal(expectedUserData.Id, actualPostData.AuthorId.ToString());
            Assert.Equal(expectedUserData.UserName, actualPostData.AuthorName);
            Assert.Equal(expectedUserData.ProfileImageUrl, actualPostData.AuthorImageUrl);
            Assert.Equal(expectedUserData.Rating, actualPostData.AuthorRating);
            Assert.Equal(expectedPostData.Id, actualPostData.Id);
            Assert.Equal(expectedPostData.Title, actualPostData.Title);
            Assert.Equal(expectedPostData.Created, actualPostData.Created);
            Assert.Equal(expectedPostData.Content, actualPostData.Content);
            Assert.NotEmpty(actualPostData.Replies);
        }
    }
}
