using Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Data.Configuration
{
    public class MailSettingConfiguration : IEntityTypeConfiguration<MailSetting>
    {
        public void Configure(EntityTypeBuilder<MailSetting> builder)
        {
            builder.ToTable("MailSettings");

            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).UseIdentityColumn();


            builder.Property(x => x.Email).IsRequired();
            builder.Property(x => x.DisplayName).IsRequired();
            builder.Property(x => x.Host).IsRequired();
            builder.Property(x => x.Password).IsRequired();
            builder.Property(x => x.Port).IsRequired();
        }
    }
}
