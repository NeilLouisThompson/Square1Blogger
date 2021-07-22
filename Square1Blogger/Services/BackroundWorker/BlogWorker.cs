using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Square1Blogger.Hubs;
using Square1Blogger.Models.Blogs;
using Square1Blogger.Services.BlogServices;
using Square1Blogger.Services.ClientServices;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Square1Blogger.Services.BackroundWorker
{
    public class BlogWorker : IHostedService, IDisposable
    {

        //The time between making requests this could be saved to a DB for beter maintainability and configurations
        //Represents sec

        //Set to 1 min instead of 20min for a faster test time
        private int executionDelay = 60;

        private readonly ILogger<BlogWorker> _logger;
        private Timer _timer;

        private readonly IBlogHttpClientService _blogHttpClientService;
        private readonly IHubContext<BlogHub> _hubContext;

        //This IServiceProvider antipattern is need to accomidate the issues we will have to inject a scoped service into (IHostedService) singleton
        private readonly IServiceProvider _serviceProvider;

        public BlogWorker(ILogger<BlogWorker> logger, IBlogHttpClientService blogHttpClientService, IServiceProvider serviceProvider, IHubContext<BlogHub> hubContext)
        {
            _logger = logger;
            _blogHttpClientService = blogHttpClientService;
            _serviceProvider = serviceProvider;
            _hubContext = hubContext;
        }

        public Task StartAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Started up Blog Worker");

            _timer = new Timer(DoWork, null, TimeSpan.Zero, TimeSpan.FromSeconds(executionDelay));

            return Task.CompletedTask;
        }

        private async void DoWork(object state)
        {
            _logger.LogInformation("Requesting post from admin user");

            var requestedList = await _blogHttpClientService.GetPostsAsync();

            if (requestedList.Count > 0)
            {
                var convertedList = requestedList.Select(x => new Blog()
                {
                    Title = x.title,
                    Description = x.description,
                    PublicationDate = DateTime.Parse(x.publication_date),
                    UserId = "Admin",
                    UserName = "Admin"
                }).ToList();

                //This IServiceProvider antipattern is need to accomidate the issues we will have to inject a scoped service into (IHostedService) singleton
                using (var scope = _serviceProvider.CreateScope())
                {
                    var context = scope.ServiceProvider.GetRequiredService<IBlogService>();
                    context.BulkInsertBlog(convertedList);

                    //Additional way to call this, we can also just call inside the service via the private PublishSignalRRefresh methode
                    await _hubContext.Clients.All.SendAsync("RefreshBlogsPage");
                }

            }
            else
            {
                _logger.LogWarning("Request to get post returned 0 results. None inserted");
            }
        }

        public Task StopAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Blog Worker is stopping.");

            _timer?.Change(Timeout.Infinite, 0);

            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }
    }

}
