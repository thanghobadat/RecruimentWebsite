using Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Data.Configuration
{
    public class CompanyImageConfiguration : IEntityTypeConfiguration<CompanyImage>
    {
        public void Configure(EntityTypeBuilder<CompanyImage> builder)
        {
            builder.ToTable("CompanyImages");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).UseIdentityColumn();
            builder.Property(x => x.DateCreated).IsRequired();
            builder.Property(x => x.ImagePath).IsRequired();
            builder.Property(x => x.Caption).IsRequired();
            builder.Property(x => x.FizeSize).IsRequired();
            builder.HasOne(ci => ci.CompanyInformation).WithMany(ca => ca.CompanyImages)
                .HasForeignKey(pc => pc.CompanyId);
        }
    }
}
