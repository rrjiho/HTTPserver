using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ServerAPI.Entities;

namespace ServerAPI.Configurations
{
    public class ResourcesConfiguration : IEntityTypeConfiguration<Resources>
    {
        public void Configure(EntityTypeBuilder<Resources> builder)
        {
            builder.HasKey(r => r.UserId);
        }
    }
}
