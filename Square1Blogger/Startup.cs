using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Square1Blogger.Models;
using Square1Blogger.Repositories.BlogRepository;
using Square1Blogger.Services.BlogServices;
using Square1Blogger.Services.BackroundWorker;
using Square1Blogger.Services.ClientServices;
using Square1Blogger.Hubs;

namespace Square1Blogger
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //Connect and init DB
            string connectionString = Configuration.GetConnectionString("default");
            services.AddDbContext<AppDBContext>(c => c.UseSqlite(connectionString));

            services.AddIdentity<IdentityUser, IdentityRole>().AddEntityFrameworkStores<AppDBContext>();

            //Init Services and repos
            services.AddTransient<IBlogRepository, BlogRepository>();
            services.AddTransient<IBlogService, BlogService>();

            services.AddSingleton<IBlogHttpClientService, BlogHttpClientService>();

            //Worker to fetch post/blogs every x sec
            services.AddHostedService<BlogWorker>();

            services.AddSignalR();
            services.AddHttpClient();
            services.AddControllersWithViews();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");

                endpoints.MapHub<BlogHub>("/BlogHub");
            });
        }
    }
}
