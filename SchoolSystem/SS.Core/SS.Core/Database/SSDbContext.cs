using Microsoft.EntityFrameworkCore;
using SS.Core.Models;

namespace SS.Core.Database
{
    public class SSDbContext : DbContext
    {
        public SSDbContext(DbContextOptions options) : base(options)
        {
        }

        public SSDbContext()
        {
        }

        public DbSet<Activity> Activities { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<StudentsCourses> StudentsCourses { get; set; }

        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    optionsBuilder.UseSqlServer(@"Server=.\SQLEXPRESS;Database=SchoolSystem;Trusted_Connection=True;");
        //}

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<StudentsCourses>()
                .HasOne(bc => bc.Student)
                .WithMany(b => b.StudentsCourses)
                .HasForeignKey(bc => bc.StudentId);

            modelBuilder.Entity<StudentsCourses>()
                .HasOne(bc => bc.Course)
                .WithMany(c => c.StudentsCourses)
                .HasForeignKey(bc => bc.CourseId);

            base.OnModelCreating(modelBuilder);
        }
    }
}
