using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using TechnicalSupport.Models;

namespace TechnicalSupport.Data
{
    public class ChatContext : DbContext
    {

            public DbSet<User> Users { get; set; }
            public DbSet<Role> Roles { get; set; }
            public DbSet<Dialog> Dialogs { get; set; }
            public DbSet<Message> Messages { get; set; }
            public DbSet<Status> Statuses { get; set; }
            public DbSet<Employee> Employees { get; set; }
      



        public virtual DbSet<Client> Clients { get; set; }
        public virtual DbSet<Application> Applications { get; set; }
     

   
        public virtual DbSet<EmployeeTask> Tasks { get; set; }
        public virtual DbSet<WorkTime> WorkTimes { get; set; }




        public ChatContext(DbContextOptions<ChatContext> options)
                : base(options)
            {
          //  Database.EnsureDeleted();
          //  Database.EnsureCreated();
        }



        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    if (!optionsBuilder.IsConfigured)
        //    {
        //        IConfigurationRoot configuration = new ConfigurationBuilder()
        //                              .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
        //                              .AddJsonFile("appsettings.json")
        //                              .Build();
        //        optionsBuilder.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
        //    }
        //}

        //protected override void OnModelCreating(ModelBuilder modelBuilder)
        //{



        //string adminRoleName = "admin";
        //string userRoleName = "user";

        //// добавляем тестовые роли
        //Role adminRole = new Role { Id = 1, Name = adminRoleName };
        //Role userRole = new Role { Id = 2, Name = userRoleName };

        ////// добавляем тестовых пользователей
        //User adminUser1 = new User { Id = Guid.NewGuid(), Email = "madmin@mail.com", Password = "123456", RoleId = adminRole.Id };
        //User simpleUser1 = new User { Id = Guid.NewGuid(), Email = "bob@mail.com", Password = "123456", RoleId = userRole.Id };
        //User simpleUser2 = new User { Id = Guid.NewGuid(), Email = "sam@mail.com", Password = "123456", RoleId = userRole.Id };


        //Employee employee1 = new Employee { Id = Guid.NewGuid(), Email = "admin@mail.com", Password = "123456", RoleId = adminRole.Id, Name = "admin", StatusOnline = false };
        //Employee employee2 = new Employee { Id = Guid.NewGuid(), Email = "employee@mail.com", Password = "123456", RoleId = adminRole.Id, Name = "employee", StatusOnline = false };

        //WorkTime workTime = new WorkTime { Id = 2, From = TimeSpan.FromMinutes(200), To = TimeSpan.FromMinutes(900), Employees = new List<Employee> { employee1, employee2 } };


        //modelBuilder.Entity<Role>().HasData(new Role[] { adminRole, userRole
        // });
        //    modelBuilder.Entity<User>().HasData(new User[] { adminUser1, simpleUser1, simpleUser2
        //});
        //modelBuilder.Entity<Employee>().HasData(new Employee[] { employee1, employee2 });
        //base.OnModelCreating(modelBuilder);



       

         
        //}

     



    }
}
