using Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Data.Configuration
{
    public class UserInformationConfiguration : IEntityTypeConfiguration<UserInformation>
    {
        public void Configure(EntityTypeBuilder<UserInformation> builder)
        {
            builder.ToTable("UserInformations");

            builder.HasKey(x => x.UserId);
            builder.Property(x => x.FirstName).IsRequired();
            builder.Property(x => x.LastName).IsRequired();
            builder.Property(x => x.Age).IsRequired();
            builder.Property(x => x.Sex).IsRequired();
            builder.Property(x => x.Address).IsRequired();
            builder.Property(x => x.AcademicLevel).IsRequired();
            builder.HasOne(t => t.AppUser).WithOne(pc => pc.UserInformation)
                .HasForeignKey<UserInformation>(pc => pc.UserId);
        }
    }
}
