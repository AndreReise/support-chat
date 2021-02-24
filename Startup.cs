using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc.Localization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using TechnicalSupport.Data;
using TechnicalSupport.Services;

namespace TechnicalSupport
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
            //for local testing
            services.AddDbContext<SupportContext>(options =>
            options.UseSqlServer("Data Source=.;Initial Catalog=chat_db;Integrated Security=True"));

            //services.AddDbContext<SupportContext>(options =>
            //options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));


            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();


            services.AddScoped<ICryptoProvider, CryptoProvider>( (options) =>
                new CryptoProvider()
            );

            services.AddScoped<IAuthService, AuthService>( (options) =>
                new AuthService(
                    options.GetRequiredService<SupportContext>(),
                    options.GetRequiredService<ICryptoProvider>(),
                    options.GetRequiredService<IHttpContextAccessor>()
                    )
            );

            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie((options) =>
                {
                    options.LoginPath = new PathString("/Account/Login");
                    options.LogoutPath = new PathString("/Account/Logout");
                });


            services.AddLocalization((options) => options.ResourcesPath = "Resources");
            services.AddControllersWithViews()
                .AddViewLocalization();
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


            var supportedCultures = new[]
            {
                new CultureInfo("ru")
            };

            app.UseRequestLocalization(new RequestLocalizationOptions
            {
                DefaultRequestCulture = new RequestCulture("ru"),
                SupportedCultures = supportedCultures,
                SupportedUICultures = supportedCultures
            });

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();
            app.UseAuthentication();
            //app.SetBasePath(env.ContentRootPath);
            //app.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
            //app.AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true);

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
