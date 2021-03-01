using System;
using System.Globalization;

using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;

using Microsoft.EntityFrameworkCore;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using Microsoft.AspNetCore.SignalR;

using TechnicalSupport.Models;
using TechnicalSupport.Services;
using TechnicalSupport.Data;
using TechnicalSupport.Middleware;

namespace TechnicalSupport
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;

        }
      
        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {


            //string connection = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=SupportChat;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
            string connection = @"Data Source=.;Initial Catalog=chat_db;Integrated Security=True";
            services.AddDbContext<ChatContext>(options => options.UseSqlServer(connection),
                 ServiceLifetime.Singleton
                );



            services.AddSingleton<IUserIdProvider, CustomUserIdProvider>(sp=>
            {
                using (var scope = sp.CreateScope())
                {
                    var dbContext = scope.ServiceProvider.GetService<ChatContext>();
                    return new CustomUserIdProvider(dbContext);
                }
            }
                );

            services.AddSingleton<AutoDialog>();

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();


            //Provide operation under user secret data
            services.AddScoped<ICryptoProvider, CryptoProvider>( (options) =>
                new CryptoProvider()
            );

            //Provide user auth service
            services.AddScoped<IAuthService, AuthService>( (options) =>
                new AuthService(
                    options.GetRequiredService<ChatContext>(),
                    options.GetRequiredService<ICryptoProvider>(),
                    options.GetRequiredService<IHttpContextAccessor>()
                    )
            );

            //Provide user join service
            services.AddScoped<IJoinService, JoinService>((options) =>
                new JoinService(
                    options.GetRequiredService<ChatContext>(),
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
                .AddDataAnnotationsLocalization()
                .AddViewLocalization();
           
            services.AddSignalR(hubOptions =>
            {
                hubOptions.EnableDetailedErrors = true;
                hubOptions.KeepAliveInterval = TimeSpan.FromSeconds(3);
                hubOptions.ClientTimeoutInterval = TimeSpan.FromMinutes(10);
                hubOptions.MaximumReceiveMessageSize = 102400000;
                
            });

        


        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {

            //app.UseMiddleware<CultureMiddleware>();


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
                new CultureInfo("ru"),
                new CultureInfo("en")
            };
            app.UseRequestLocalization(new RequestLocalizationOptions
            {
                DefaultRequestCulture = new RequestCulture("ru"),
                SupportedCultures = supportedCultures,
                SupportedUICultures = supportedCultures
            });

            //app.UseCulture();


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
                endpoints.MapHub<MessageHub>("/chat");
            });


            
        }
    }


 }





