using Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Data.Configuration
{
    public class BranchRecruitmentConfiguration : IEntityTypeConfiguration<BranchRecruitment>
    {
        public void Configure(EntityTypeBuilder<BranchRecruitment> builder)
        {
            builder.HasKey(t => new { t.BranchId, t.RecruimentId });

            builder.ToTable("BranchRecruiments");

            builder.HasOne(t => t.Branch).WithMany(pc => pc.BranchRecruiments)
                .HasForeignKey(pc => pc.BranchId);

            builder.HasOne(t => t.Recruitment).WithMany(pc => pc.BranchRecruiments)
              .HasForeignKey(pc => pc.RecruimentId);
        }
    }
}
