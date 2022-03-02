using Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

namespace Data.Configuration
{
    public class CurriculumVitaeConfiguration : IEntityTypeConfiguration<CurriculumVitae>
    {
        public void Configure(EntityTypeBuilder<CurriculumVitae> builder)
        {
            builder.HasKey(t => new { t.UserId, t.RecruimentId });

            builder.ToTable("CurriculumVitaes");
            builder.Property(x => x.FilePath).IsRequired();
            builder.Property(x => x.DateCreated).IsRequired().HasDefaultValue(DateTime.Now);

            builder.HasOne(t => t.UserInformation).WithMany(pc => pc.CurriculumVitaes)
                .HasForeignKey(pc => pc.UserId).OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(t => t.Recruitment).WithMany(pc => pc.CurriculumVitaes)
              .HasForeignKey(pc => pc.RecruimentId).OnDelete(DeleteBehavior.Restrict);
        }
    }
}
