using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Square1Blogger.Models.Blogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace Square1Blogger.Services.ClientServices
{
    public class BlogHttpClientService : IBlogHttpClientService
    {
        private readonly ILogger<BlogHttpClientService> _logger;
        private readonly IHttpClientFactory _clientFactory;
        private IConfiguration _configutarations;
        private string postUrl;

        public BlogHttpClientService(IHttpClientFactory clientFactory, IConfiguration configutarations, ILogger<BlogHttpClientService> logger)
        {
            _clientFactory = clientFactory;
            _configutarations = configutarations;
            postUrl = _configutarations["PostUrl"];
            _logger = logger;
        }

        public async Task<List<StreamBlogItems>> GetPostsAsync()
        {
            var results = new List<StreamBlogItems>();
            try
            {
                var request = new HttpRequestMessage(HttpMethod.Get, postUrl);

                var client = _clientFactory.CreateClient();

                var response = await client.SendAsync(request);

                if (response.IsSuccessStatusCode)
                {
                    using var responseStream = await response.Content.ReadAsStreamAsync();
                    var streamBlog = await JsonSerializer.DeserializeAsync<StreamBlog>(responseStream);
                    results = streamBlog.data;
                }

                return results;
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return results;
            }
        }
    }
}
