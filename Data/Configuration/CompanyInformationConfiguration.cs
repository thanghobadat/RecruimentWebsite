using Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Data.Configuration
{
    public class CompanyInformationConfiguration : IEntityTypeConfiguration<CompanyInformation>
    {
        public void Configure(EntityTypeBuilder<CompanyInformation> builder)
        {
            builder.ToTable("CompanyInformations");

            builder.HasKey(x => x.UserId);

            builder.Property(x => x.Name).IsRequired();
            builder.Property(x => x.WorkerNumber).IsRequired().HasDefaultValue(0);
            builder.Property(x => x.Description).IsRequired();
            builder.Property(x => x.ContactName).IsRequired();
            builder.HasOne(t => t.AppUser).WithOne(pc => pc.CompanyInformation)
                .HasForeignKey<CompanyInformation>(pc => pc.UserId);



        }
    }
}
