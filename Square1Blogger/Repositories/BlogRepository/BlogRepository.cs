using Microsoft.EntityFrameworkCore;
using Square1Blogger.Models;
using Square1Blogger.Models.Blogs;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.Extensions.Logging;

namespace Square1Blogger.Repositories.BlogRepository
{
    public class BlogRepository : IBlogRepository
    {
        private readonly AppDBContext _bloggerDbContext;
        private readonly ILogger<BlogRepository> _logger;

        public BlogRepository(AppDBContext bloggerDbContext, ILogger<BlogRepository> logger)
        {
            _logger = logger;
            _bloggerDbContext = bloggerDbContext;
        }

        public bool InsertBlog(Blog model)
        {
            try
            {
                _bloggerDbContext.Blogs.Add(model);
                var result = _bloggerDbContext.SaveChanges();

                return result > 0;
            }
            catch (Exception e)
            {

                _logger.LogError($"An Error occured Inserting Blog :: {e.Message}");
                return false;
            }
        }

        public async Task BulkInsertBlogAsync(List<Blog> collection)
        {
            try
            {
                _bloggerDbContext.Blogs.AddRange(collection);
                var result = await _bloggerDbContext.SaveChangesAsync();
            }
            catch (Exception e)
            {
                _logger.LogError($"An Error occured Bull Inserting Blogs :: {e.Message}");
            }
        }

        public async Task<List<Blog>> GetAllBlogsAsync()
        {
            try
            {
                var result = await _bloggerDbContext.Blogs.ToListAsync();
                return result;
            }
            catch (Exception e)
            {
                _logger.LogError($"An Error occured retrieving All Blogs :: {e.Message}");
                return new List<Blog>();
            }
        }

        public async Task<List<Blog>> GetAllBlogForCurrentUser(string userId)
        {
            try
            {
                var result = await _bloggerDbContext.Blogs.Where(x => x.UserId == userId).ToListAsync();

                return result;
            }
            catch (Exception e)
            {
                _logger.LogError($"An Error occured retrieving All Blogs for userId {userId} :: {e.Message}");
                return new List<Blog>();
            }
        }
    }
}
