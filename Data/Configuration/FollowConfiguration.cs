using Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Data.Configuration
{
    public class FollowConfiguration : IEntityTypeConfiguration<Follow>
    {
        public void Configure(EntityTypeBuilder<Follow> builder)
        {
            builder.HasKey(t => new { t.UserId, t.CompanyId });

            builder.ToTable("Follows");

            builder.HasOne(t => t.UserInformation).WithMany(pc => pc.Follows)
                .HasForeignKey(pc => pc.UserId).OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(t => t.CompanyInformation).WithMany(pc => pc.Follows)
              .HasForeignKey(pc => pc.CompanyId).OnDelete(DeleteBehavior.Restrict);
        }
    }
}
