using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using static System.Net.Mime.MediaTypeNames;

namespace DL
{
    public class QuantEdDbContext : DbContext
    {
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
