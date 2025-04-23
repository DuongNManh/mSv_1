using Microsoft.EntityFrameworkCore;
using PlatformService.Models.Entities;

namespace PlatformService.Contexts
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("PlatformService");

            modelBuilder.Entity<Platform>()
                .Property(p => p.Id)
                .ValueGeneratedOnAdd();
        }
        public DbSet<Platform> Platforms { get; set; }
    }
}
