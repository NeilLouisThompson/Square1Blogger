using Square1Blogger.Models.Blogs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Square1Blogger.Repositories.BlogRepository
{
    public interface IBlogRepository
    {
        bool InsertBlog(Blog model);
        Task BulkInsertBlogAsync(List<Blog> collection);

         Task<List<Blog>> GetAllBlogsAsync();

         Task<List<Blog>> GetAllBlogForCurrentUser(string userId);
    }
}
