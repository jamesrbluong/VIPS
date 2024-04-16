using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using VIPS.Common.Models.Entities;

namespace VIPS.Common.Data
{
    public class ApplicationDbContext : IdentityDbContext<AppUser, IdentityRole<Guid>, Guid>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<AppUser> AppUsers => Set<AppUser>();

        public DbSet<CSV> CSVs => Set<CSV>();

        public DbSet<Visualization> Visualizations => Set<Visualization>();

        public DbSet<Contract> Contracts => Set<Contract>();

        public DbSet<School> Schools => Set<School>();

        public DbSet<Department> Departments => Set<Department>();

        public DbSet<Partner> Partners => Set<Partner>();
    }
}
