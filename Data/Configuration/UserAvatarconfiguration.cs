using Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Data.Configuration
{
    public class UserAvatarconfiguration : IEntityTypeConfiguration<UserAvatar>
    {
        public void Configure(EntityTypeBuilder<UserAvatar> builder)
        {
            builder.ToTable("UserAvatars");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).UseIdentityColumn();
            builder.Property(x => x.DateCreated).IsRequired();
            builder.Property(x => x.ImagePath).IsRequired();
            builder.Property(x => x.Caption).IsRequired();
            builder.Property(x => x.FizeSize).IsRequired();
            builder.HasOne(ci => ci.UserInformation).WithOne(ca => ca.UserAvatar)
                .HasForeignKey<UserAvatar>(pc => pc.UserId);
        }
    }
}
