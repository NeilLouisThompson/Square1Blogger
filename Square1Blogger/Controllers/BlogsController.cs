using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Square1Blogger.Models.Blogs;
using Square1Blogger.Services.BlogServices;
using System.Threading.Tasks;

namespace Square1Blogger.Controllers
{
    [Authorize]
    public class BlogsController : Controller
    {
        private readonly IBlogService _blogService;

        public BlogsController(IBlogService blogService)
        {

            _blogService = blogService;
        }

        public async Task<IActionResult> ManageBlogs()
        {
            var currentUserBlogs = await _blogService.GetAllBlogForCurrentUser(User);

            return View(currentUserBlogs);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Blog model)
        {
            _blogService.InsertBlog(model, User);

            return RedirectToAction("ManageBlogs");
        }
    }
}
