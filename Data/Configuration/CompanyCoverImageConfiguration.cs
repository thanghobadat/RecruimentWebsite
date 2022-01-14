using Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Data.Configuration
{
    public class CompanyCoverImageConfiguration : IEntityTypeConfiguration<CompanyCoverImage>
    {
        public void Configure(EntityTypeBuilder<CompanyCoverImage> builder)
        {
            builder.ToTable("CompanyCoverImages");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).UseIdentityColumn();
            builder.Property(x => x.DateCreated).IsRequired();
            builder.Property(x => x.ImagePath).IsRequired();
            builder.Property(x => x.Caption).IsRequired();
            builder.Property(x => x.FizeSize).IsRequired();
            builder.HasOne(ci => ci.CompanyInformation).WithOne(ca => ca.CompanyCoverImage)
                .HasForeignKey<CompanyCoverImage>(pc => pc.CompanyId);
        }
    }
}
