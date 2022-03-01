using Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Data.Configuration
{
    public class CareerRecruitmentConfiguration : IEntityTypeConfiguration<CareerRecruitment>
    {
        public void Configure(EntityTypeBuilder<CareerRecruitment> builder)
        {
            builder.HasKey(t => new { t.CareerId, t.RecruimentId });

            builder.ToTable("CareerRecruitments");

            builder.HasOne(t => t.Career).WithMany(pc => pc.CareerRecruitments)
                .HasForeignKey(pc => pc.CareerId);

            builder.HasOne(t => t.Recruitment).WithMany(pc => pc.CareerRecruitments)
              .HasForeignKey(pc => pc.RecruimentId);
        }
    }
}
