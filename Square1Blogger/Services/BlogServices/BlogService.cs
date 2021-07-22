using Square1Blogger.Models.Blogs;
using Square1Blogger.Repositories.BlogRepository;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Square1Blogger.Hubs;

namespace Square1Blogger.Services.BlogServices
{
    public class BlogService : IBlogService
    {
        private readonly IBlogRepository _blogRepository;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IHubContext<BlogHub> _hubContext;

        public BlogService(IBlogRepository blogRepository, UserManager<IdentityUser> userManager, IHubContext<BlogHub> hubContext)
        {
            _blogRepository = blogRepository;
            _userManager = userManager;
            _hubContext = hubContext;
        }

        public bool InsertBlog(Blog model, ClaimsPrincipal User)
        {
            //Apply Current user details 
            model.UserName = _userManager.GetUserName(User);
            model.UserId = _userManager.GetUserId(User);

            var insertResult = _blogRepository.InsertBlog(model);
            if (insertResult)
            {
                PublishSignalRRefresh();
            }
            return insertResult;
        }

        public void BulkInsertBlog(List<Blog> collection)
        {
            _blogRepository.BulkInsertBlogAsync(collection);
        }

        public async Task<List<Blog>> GetAllBlogsAsync()
        {
            return await _blogRepository.GetAllBlogsAsync();
        }

        public async Task<List<Blog>> GetAllBlogForCurrentUser(ClaimsPrincipal User)
        {
            return await _blogRepository.GetAllBlogForCurrentUser(_userManager.GetUserId(User));
        }

        private async void PublishSignalRRefresh()
        {
            await _hubContext.Clients.All.SendAsync("RefreshBlogsPage");
        }
    }
}
