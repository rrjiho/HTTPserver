using Microsoft.EntityFrameworkCore;

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
            modelBuilder.Entity<Resources>()
                .HasKey(r => r.UserId);

            modelBuilder.Entity<Resources>()
                .HasOne(r => r.User) 
                .WithOne(u => u.Resources) 
                .HasForeignKey<Resources>(r => r.UserId);

            modelBuilder.Entity<Ranking>()
                .HasOne(r => r.User)
                .WithOne(u => u.Ranking)
                .HasForeignKey<Ranking>(r => r.UserId); 
        }
    }
}
