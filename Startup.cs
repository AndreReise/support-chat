using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TechnicalSupport.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.SignalR;
using TechnicalSupport.Models;


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

           
            string connection = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=SupportChat;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";

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


            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
             .AddCookie(options =>
             {

                 options.LoginPath = new Microsoft.AspNetCore.Http.PathString("/Account/Login");
                 options.AccessDeniedPath = new Microsoft.AspNetCore.Http.PathString("/Account/Login");
             });

            services.AddControllersWithViews();
           
            services.AddSignalR(hubOptions =>
            {
                hubOptions.EnableDetailedErrors = true;
                hubOptions.KeepAliveInterval = TimeSpan.FromSeconds(3);
                hubOptions.ClientTimeoutInterval = TimeSpan.FromMinutes(10);
                hubOptions.MaximumReceiveMessageSize = 102400000;
                
            });

        


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
            app.UseCookiePolicy();

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





