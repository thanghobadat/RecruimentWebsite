using Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Data.Configuration
{
    public class CompanyAvatarConfiguration : IEntityTypeConfiguration<CompanyAvatar>
    {
        public void Configure(EntityTypeBuilder<CompanyAvatar> builder)
        {
            builder.ToTable("CompanyAvatars");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).UseIdentityColumn();
            builder.Property(x => x.DateCreated).IsRequired();
            builder.Property(x => x.ImagePath).IsRequired();
            builder.Property(x => x.Caption).IsRequired();
            builder.Property(x => x.FizeSize).IsRequired();
            builder.HasOne(ci => ci.CompanyInformation).WithOne(ca => ca.CompanyAvatar)
                .HasForeignKey<CompanyAvatar>(pc => pc.CompanyId);
        }
    }
}
