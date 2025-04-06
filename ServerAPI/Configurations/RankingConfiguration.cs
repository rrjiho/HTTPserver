using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ServerAPI.Entities;

namespace ServerAPI.Configurations
{
    public class RankingConfiguration : IEntityTypeConfiguration<Ranking>
    {
        public void Configure(EntityTypeBuilder<Ranking> builder)
        {
            builder.HasKey(r => r.UserId);
        }
    }

}

