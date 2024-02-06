using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Common.Entities;

namespace Common.Data
{
    public class ApplicationDbContext : IdentityDbContext<AppUser, IdentityRole<Guid>, Guid>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        { }
        public DbSet<AppUser> AppUsers { get; set; }
        public DbSet<CSV> CSVs { get; set; }
        public DbSet<Visualization> Visualizations { get; set; }
        public DbSet<Contract> Contracts { get; set; }
        public DbSet<School> Schools { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<Partner> Partners { get; set; }

    }
}
