using Square1Blogger.Models.Blogs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Square1Blogger.Services.ClientServices
{
    public interface IBlogHttpClientService
    {
        Task<List<StreamBlogItems>> GetPostsAsync();
    }
}
