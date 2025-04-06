using Microsoft.EntityFrameworkCore;
using ServerAPI.Entities;

namespace ServerAPI.DB
{
    public class AppDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Resources> Resources { get; set; }
        public DbSet<Ranking> Rankings { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
        }
    }
}
