using Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Data.Configuration
{
    public class RecruitmentConfiguration : IEntityTypeConfiguration<Recruitment>
    {
        public void Configure(EntityTypeBuilder<Recruitment> builder)
        {
            builder.ToTable("Recruitments");

            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).UseIdentityColumn();


            builder.Property(x => x.Name).IsRequired();
            builder.Property(x => x.Rank).IsRequired();
            builder.Property(x => x.Experience).IsRequired().HasDefaultValue("none");
            builder.Property(x => x.Salary).IsRequired().HasDefaultValue(0);
            builder.Property(x => x.Education).IsRequired();
            builder.Property(x => x.Type).IsRequired();
            builder.Property(x => x.Description).IsRequired();
            builder.Property(x => x.ExpirationDate).IsRequired();
            builder.Property(x => x.DateCreated).IsRequired();
            builder.HasOne(ci => ci.CompanyInformation).WithMany(ca => ca.Recruitments)
                .HasForeignKey(pc => pc.CompanyId);

        }
    }
}
