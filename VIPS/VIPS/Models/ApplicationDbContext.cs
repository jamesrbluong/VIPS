using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using VIPS.Models.Data;

namespace VIPS.Models
{
    public class ApplicationDbContext : IdentityDbContext<AppUser, IdentityRole<Guid>, Guid>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        { }
        public DbSet<Data.AppUser> AppUsers { get; set; }
        public DbSet<CSV> CSVs { get; set; }
        public DbSet<Visualization> Visualizations { get; set; }
        public DbSet<Contract> Contracts { get; set; }
        public DbSet<School> Schools { get; set;}
        public DbSet<Department> Departments { get; set; }
        public DbSet<Partner> Partners { get; set; }

    }
}
