using Microsoft.EntityFrameworkCore;
using Core.Models;
using CourseM = Core.Models;
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
        public DbSet<CourseModule> Modules { get; set;}
        public DbSet<Organization> Organizations { get; set;}
        public DbSet<Question> Questions { get; set;}
        public DbSet<Subscription> Subscriptions { get; set;}
        public DbSet<Test> Tests { get; set;}
        
        public QuantEdDbContext(DbContextOptions<QuantEdDbContext> options) : base(options)
        {
            
        }


        //public DbSet<Course> Courses { get; set; }
        //protected override void OnModelCreating(ModelBuilder modelBuilder)
        //{
        //    modelBuilder.Entity<Att>()
        //        .HasOne(a => a.Test)
        //        .WithMany(t => t.Atts)
        //        .OnDelete(DeleteBehavior.Restrict);
        //}
    }
}
