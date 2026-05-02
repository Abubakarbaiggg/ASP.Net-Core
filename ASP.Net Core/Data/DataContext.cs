using ASP.Net_Core.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ASP.Net_Core.ViewModels;

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
    }
}
