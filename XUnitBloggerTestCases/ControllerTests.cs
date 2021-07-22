using System;
using System.Collections.Generic;
using Square1Blogger.Controllers;
using Microsoft.AspNetCore.Mvc;
using Square1Blogger.Services.BlogServices;
using Moq;
using System.Threading.Tasks;
using Square1Blogger.Models.Blogs;
using Xunit;
using System.Security.Claims;

namespace XUnitBloggerTestCases
{
    public class ControllerTests
    {
        [Fact]
        public async Task HomeController_Index_Test()
        {
            var mockBlogService = new Mock<IBlogService>();
            mockBlogService.Setup(repo => repo.GetAllBlogsAsync()).ReturnsAsync(GetTestBlogs());
            var controller = new HomeController(mockBlogService.Object);

            var result = await controller.Index();

            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<List<Blog>>(viewResult.ViewData.Model);
        }

        [Fact]
        public async Task BlogsController_ManageBlogs_Test()
        {
            var mockBlogService = new Mock<IBlogService>();
            mockBlogService.Setup(repo => repo.GetAllBlogForCurrentUser(MockClaimsPrincipal())).ReturnsAsync(GetTestBlogs());
            var controller = new HomeController(mockBlogService.Object);

            var result = await controller.Index();
            var viewResult = Assert.IsType<ViewResult>(result);
        }

        [Fact]
        public async Task BlogsController_Create_Test()
        {
            var mockBlogService = new Mock<IBlogService>();

            var blogModel = new Blog()
            {
                Description = "Description Here",
                Title = "Title here",
                PublicationDate = DateTime.Now,
                UserId = "TestUserID",
                UserName = "TestUserName"

            };

            mockBlogService.Setup(repo => repo.InsertBlog(blogModel, MockClaimsPrincipal()));
            var controller = new HomeController(mockBlogService.Object);

            var result = await controller.Index();
            var viewResult = Assert.IsType<ViewResult>(result);
        }

        private List<Blog> GetTestBlogs()
        {
            var timeNow = DateTime.Now;
            var blogsList = new List<Blog>
            {
                new Blog()
                {
                    Description = "Description Here",
                    Title = "Title here",
                    PublicationDate = timeNow,
                    UserId = "TestUserID",
                    UserName = "TestUserName"

                },
                new Blog()
                {
                    Description = "Description Here",
                    Title = "Title here",
                    PublicationDate = timeNow,
                    UserId = "TestUserID2",
                    UserName = "TestUserName2"

                }
            };

            return blogsList;
        }

        private ClaimsPrincipal MockClaimsPrincipal()
        {
            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Name, "username"),
                new Claim(ClaimTypes.NameIdentifier, "userId"),
                new Claim("name", "John Doe"),
            };
            var identity = new ClaimsIdentity(claims, "TestAuthType");
            var claimsPrincipal = new ClaimsPrincipal(identity);
            return claimsPrincipal;
        }

    }
}
