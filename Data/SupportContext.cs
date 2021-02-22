using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using TechnicalSupport.Models;
using Microsoft.Extensions.Configuration;
using System.Configuration;

#nullable disable

namespace TechnicalSupport.Data
{
    public partial class SupportContext : DbContext
    {
        //public GL_SupportContext()
        //{
        //}

        public SupportContext(DbContextOptions<SupportContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Employee> Employees { get; set; }
        public virtual DbSet<Client> Clients { get; set; }
        public virtual DbSet<User> Users { get; set; }



        public virtual DbSet<Application> Applications { get; set; }
        public virtual DbSet<Chat> Chats { get; set; }
        public virtual DbSet<CommunicationType> CommunicationTypes { get; set; }
        public virtual DbSet<Detail> Details { get; set; }
        
        public virtual DbSet<RequestType> RequestTypes { get; set; }
        public virtual DbSet<Sex> Sexes { get; set; }
        public virtual DbSet<Status> Statuses { get; set; }
        public virtual DbSet<EmployeeTask> Tasks { get; set; }
        public virtual DbSet<WorkTime> WorkTimes { get; set; }


        
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                IConfigurationRoot configuration = new ConfigurationBuilder()
                                      .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                                      .AddJsonFile("appsettings.json")
                                      .Build();
                optionsBuilder.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "Cyrillic_General_CI_AS");

            modelBuilder.Entity<Application>(entity =>
            {
                entity.ToTable("Application");

                entity.Property(e => e.Status).HasDefaultValueSql("((1))");

                entity.Property(e => e.Topic)
                    .IsRequired()
                    .HasMaxLength(150);

                entity.HasOne(d => d.ChatNavigation)
                    .WithMany(p => p.Applications)
                    .HasForeignKey(d => d.Chat)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Chat");

                entity.HasOne(d => d.StatusNavigation)
                    .WithMany(p => p.Applications)
                    .HasForeignKey(d => d.Status)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Status");
            });

            modelBuilder.Entity<Chat>(entity =>
            {
                entity.ToTable("Chat");

                entity.Property(e => e.Guidemployee).HasColumnName("GUIDEmployee");

                entity.Property(e => e.Guiduser).HasColumnName("GUIDUser");
            });

            modelBuilder.Entity<CommunicationType>(entity =>
            {
                entity.ToTable("CommunicationType");

                entity.Property(e => e.CommunicationType1)
                    .IsRequired()
                    .HasMaxLength(300)
                    .HasColumnName("CommunicationType");
            });

            modelBuilder.Entity<Detail>(entity =>
            {
                entity.HasKey(e => e.DetailsId)
                    .HasName("PK__Details__BAC8628C5494B5B4");

                entity.Property(e => e.CreatingDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Data).HasMaxLength(4000);

                entity.Property(e => e.Guidinteracting).HasColumnName("GUIDInteracting");

                entity.HasOne(d => d.ChatNavigation)
                    .WithMany(p => p.Details)
                    .HasForeignKey(d => d.Chat)
                    .HasConstraintName("FK_DetailsChat");

                entity.HasOne(d => d.CommunicationTypeNavigation)
                    .WithMany(p => p.Details)
                    .HasForeignKey(d => d.CommunicationType)
                    .HasConstraintName("FK_DetailsCommunicationType");

                entity.HasOne(d => d.RequestTypeNavigation)
                    .WithMany(p => p.Details)
                    .HasForeignKey(d => d.RequestType)
                    .HasConstraintName("FK_RequestType");
            });

            modelBuilder.Entity<Employee>(entity =>
            {
                entity.ToTable("Employee");

                entity.HasIndex(e => e.EmployeeGuid, "IX_EmployeeGUID")
                    .IsUnique();

                entity.Property(e => e.Email).HasMaxLength(100);

                entity.Property(e => e.EmployeeGuid)
                    .HasColumnName("EmployeeGUID")
                    .HasDefaultValueSql("(newid())");

                entity.Property(e => e.FirstName).HasMaxLength(100);

                entity.Property(e => e.LastName).HasMaxLength(100);

                entity.Property(e => e.PasswordHash)
                    .IsRequired()
                    .HasMaxLength(64);

                entity.Property(e => e.Phone).HasMaxLength(22);

                entity.Property(e => e.SecondName).HasMaxLength(100);

                entity.HasOne(d => d.SexNavigation)
                    .WithMany(p => p.Employees)
                    .HasForeignKey(d => d.Sex)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_EmployeeSex");

                entity.HasOne(d => d.WorkTimeNavigation)
                    .WithMany(p => p.Employees)
                    .HasForeignKey(d => d.WorkTime)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_WorkTime");
            });

            modelBuilder.Entity<RequestType>(entity =>
            {
                entity.ToTable("RequestType");

                entity.Property(e => e.RequestType1)
                    .IsRequired()
                    .HasMaxLength(300)
                    .HasColumnName("RequestType");
            });

            modelBuilder.Entity<Sex>(entity =>
            {
                entity.ToTable("Sex");

                entity.Property(e => e.Sex1)
                    .IsRequired()
                    .HasMaxLength(100)
                    .HasColumnName("Sex");
            });

            modelBuilder.Entity<Status>(entity =>
            {
                entity.ToTable("Status");

                entity.Property(e => e.Status1)
                    .IsRequired()
                    .HasMaxLength(100)
                    .HasColumnName("Status");
            });

            modelBuilder.Entity<EmployeeTask>(entity =>
            {
                entity.ToTable("Task");

                entity.Property(e => e.Guidemployy).HasColumnName("GUIDEmployy");

                entity.Property(e => e.TaskCount).HasDefaultValueSql("((1))");

                entity.HasOne(d => d.GuidemployyNavigation)
                    .WithMany(p => p.Tasks)
                    .HasPrincipalKey(p => p.EmployeeGuid)
                    .HasForeignKey(d => d.Guidemployy)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Task_GUIDEmployy");
            });

            modelBuilder.Entity<Client>(entity =>
            {
                entity.ToTable("User");

                //entity.HasIndex(e => e.UserGuid, "IX_UserGUID")
                //    .IsUnique();

                entity.Property(e => e.Email).HasMaxLength(100);

                entity.Property(e => e.FirstName).HasMaxLength(100);

                entity.Property(e => e.LastName).HasMaxLength(100);

                entity.Property(e => e.PasswordHash).HasMaxLength(64);

                entity.Property(e => e.Phone).HasMaxLength(22);

                entity.Property(e => e.SecondName).HasMaxLength(100);

                //entity.Property(e => e.UserGuid)
                //    .HasColumnName("UserGUID")
                //    .HasDefaultValueSql("(newid())");

                entity.Property(e => e.UserIp).HasMaxLength(100);

                entity.HasOne(d => d.SexNavigation)
                    .WithMany(p => p.Users)
                    .HasForeignKey(d => d.Sex)
                    .HasConstraintName("FK_UserSex");
            });

            modelBuilder.Entity<WorkTime>(entity =>
            {
                entity.ToTable("WorkTime");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
