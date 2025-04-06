using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ServerAPI.Entities;

namespace ServerAPI.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasKey(u => u.Id);

            builder.HasOne(u => u.Resources)
                   .WithOne(r => r.User)
                   .HasForeignKey<Resources>(r => r.UserId);

            builder.HasOne(u => u.Ranking)
                   .WithOne(r => r.User)
                   .HasForeignKey<Ranking>(r => r.UserId);
        }
    }
}
