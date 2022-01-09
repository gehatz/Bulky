using Bulky.Web.Models;
using Microsoft.EntityFrameworkCore;

namespace Bulky.Web.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<Category> Categories { get; set; } 
    }
}
