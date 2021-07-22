using Square1Blogger.Models.Blogs;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Square1Blogger.Services.BlogServices
{
    public interface IBlogService
    {
        bool InsertBlog(Blog model, ClaimsPrincipal User);

        void BulkInsertBlog(List<Blog> collection);

        Task<List<Blog>> GetAllBlogsAsync();

        Task<List<Blog>> GetAllBlogForCurrentUser(ClaimsPrincipal User);
    }
}
