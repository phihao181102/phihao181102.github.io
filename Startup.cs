using BooksStore.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;


namespace BooksStore
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
            services.AddControllersWithViews();
            services.AddDbContext<BooksStoreDbContext>(opts => {
                opts.UseSqlServer(
                Configuration["ConnectionStrings:BooksStoreConnection"]);
            });
            services.AddScoped<IBooksStoreRepository,
EFBooksStoreRepository>();
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
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();
            app.UseAuthorization();
            // old URL: http://localhost:44333/?bookPage=2
            // new URL: https://localhost:44333/Books/2
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute("pagination",
                "Books/{bookPage}",
                new { Controller = "Home", action = "Index" });
                endpoints.MapDefaultControllerRoute();
            });
            SeedData.EnsurePopulated(app);

        }
    }
}
