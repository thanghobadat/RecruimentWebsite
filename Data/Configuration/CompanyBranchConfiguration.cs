using Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Data.Configuration
{
    public class CompanyBranchConfiguration : IEntityTypeConfiguration<CompanyBranch>
    {
        public void Configure(EntityTypeBuilder<CompanyBranch> builder)
        {
            builder.ToTable("CompanyBranches");
            builder.HasKey(t => new { t.BranchId, t.CompanyId });
            builder.Property(x => x.Address).IsRequired();
            builder.HasOne(ci => ci.CompanyInformation).WithMany(ca => ca.CompanyBranches)
                .HasForeignKey(pc => pc.CompanyId);
            builder.HasOne(ci => ci.Branch).WithMany(ca => ca.CompanyBranches)
                .HasForeignKey(pc => pc.BranchId);
        }
    }
}
