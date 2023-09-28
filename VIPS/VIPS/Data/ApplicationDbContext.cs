
using VIPS.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;


namespace VIPS.Data
{
    public class ApplicationDbContex : DbContext
    {
        public ApplicationDbContex(DbContextOptions<ApplicationDbContex> options) : base(options)
        {
        }

        public DbSet<CSV> CSVs { get; set; }

    }

}
