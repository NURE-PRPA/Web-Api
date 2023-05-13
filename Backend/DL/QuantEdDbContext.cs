using Microsoft.EntityFrameworkCore;
using Core.Models;
using CourseM = Core.Models;
using System.Diagnostics.Metrics;

namespace DL
{
    public class QuantEdDbContext : DbContext
    {
        public DbSet<Administrator> Administrators { get; set; }
        public DbSet<Answer> Answers { get; set; }
        public DbSet<Ban> Bans { get; set; }
        public DbSet<Certificate> Certificates { get; set; }
        public DbSet<ContentContainer> Containers { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<Feedback> Feedbacks { get; set; }
        public DbSet<Listener> Listeners { get; set;}
        public DbSet<Lecturer> Lecturers { get; set; }
        public DbSet<CourseModule> Modules { get; set;}
        public DbSet<Organization> Organizations { get; set;}
        public DbSet<Question> Questions { get; set;}
        public DbSet<Subscription> Subscriptions { get; set;}
        public DbSet<Test> Tests { get; set;}

        public QuantEdDbContext()
        {
            
        }

        public QuantEdDbContext(DbContextOptions<QuantEdDbContext> options) : base(options)
        {
            
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseMySQL("Server=34.118.57.162;Port=3306;Database=testdb;Uid=Sentinel;Pwd=sen24tinel;");
            }
        }

        //public DbSet<Course> Courses { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Subscription>()
            .HasOne(s => s.Certificate)
            .WithOne(c => c.Subscription)
            .HasForeignKey<Certificate>(s => s.Id);

            modelBuilder.Entity<CourseModule>()
            .HasOne(m => m.Test)
            .WithOne(t => t.Module)
            .HasForeignKey<Test>(m => m.Id);

            modelBuilder.Entity<AbstractUser>().UseTpcMappingStrategy();

            //modelBuilder.Entity<AbstractUser>().HasKey(e => e.Id);
            //modelBuilder.Entity<Administrator>().HasKey(e => e.Id);
            //modelBuilder.Entity<Answer>().HasKey(e => e.Id);
            //modelBuilder.Entity<Ban>().HasKey(e => e.Id);
            //modelBuilder.Entity<Certificate>().HasKey(e => e.Id);
            //modelBuilder.Entity<ContentContainer>().HasKey(e => e.Id);
            //modelBuilder.Entity<Course>().HasKey(e => e.Id);
            //modelBuilder.Entity<CourseModule>().HasKey(e => e.Id);
            //modelBuilder.Entity<Feedback>().HasKey(e => e.Id);
            //modelBuilder.Entity<Lecturer>().HasKey(e => e.Id);
            //modelBuilder.Entity<Listener>().HasKey(e => e.Id);
            //modelBuilder.Entity<Organization>().HasKey(e => e.Id);
            //modelBuilder.Entity<Question>().HasKey(e => e.Id);
            //modelBuilder.Entity<Subscription>().HasKey(e => e.Id);
            //modelBuilder.Entity<Test>().HasKey(e => e.Id);
        }
    }
}
