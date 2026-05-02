using ASP.Net_Core.Models;
using ASP.Net_Core.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ASP.Net_Core.Data
{
    public class DataContext : IdentityDbContext<ApplicationUser>
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
            
        }

        public DbSet<Student> Students { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<StudentCourse> StudentCourses { get; set; }
        public DbSet<ASP.Net_Core.ViewModels.RoleStore> RoleStore { get; set; } = default!;
        public DbSet<ASP.Net_Core.ViewModels.AppUserViewModel> AppUserViewModel { get; set; } = default!;
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<Course>().HasData(
                new Course { Id = 4, Title = "Business Administration", Code = "BA" },
                new Course { Id = 5, Title = "Urdu Literature", Code = "URD" },
                new Course { Id = 6, Title = "Islamic Studies", Code = "ISL" }
            );
        }
    }
}
