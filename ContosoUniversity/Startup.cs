using ContosoUniversity.Data;
using ContosoUniversity.Infrastructure;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace ContosoUniversity
{
    public class Startup
    {
        public IConfiguration Config { get; }
        public Startup(IConfiguration configuration)
        {
            Config = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();

            services.AddScoped<IRepositoryWrapper, RepositoryWrapper>();
            services.AddScoped<IPasswordValidator<IdentityUser>, CustomPasswordValidator>();
            services.AddScoped<IUserValidator<IdentityUser>, CustomUserValidator>();

            services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(Config.GetConnectionString("DefaultConnection")));

            services.AddDbContext<AppIdentityDbContext>(options =>
                options.UseSqlServer(Config.GetConnectionString("IdentityConnection")));

            services.AddIdentity<IdentityUser, IdentityRole>(opts =>
            {
                opts.Password.RequiredLength = 6;
                opts.Password.RequireNonAlphanumeric = false;
                opts.Password.RequireLowercase = false;
                opts.Password.RequireUppercase = false;
                opts.Password.RequireDigit = false;
                opts.User.RequireUniqueEmail = true;
                opts.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";
            })
                .AddEntityFrameworkStores<AppIdentityDbContext>();

        }


        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseStaticFiles();
            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "sortingpage", 
                    pattern: "Students/OrderBy{sortBy}/Page{page}",
                    defaults: new {Controller = "Students", action = "Index", page = 1 });

                endpoints.MapControllerRoute(
                    name: "sorting", 
                    pattern: "Students/OrderBy{sortBy}",
                    defaults: new { Controller = "Students", action = "Index" });

                // e.g. /Students/Page2)
                endpoints.MapControllerRoute(
                    name: "pagination", 
                    pattern: "Students/Page{page}",
                    defaults: new { Controller = "Students", action = "Index", page = 1 });

                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });

            SeedData.EnsurePopulated(app);
            SeedDataIdentity.EnsurePopulated(app);
        }
    }
}
