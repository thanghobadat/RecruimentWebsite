using Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Data.Configuration
{
    public class CommentConfiguration : IEntityTypeConfiguration<Comment>
    {
        public void Configure(EntityTypeBuilder<Comment> builder)
        {
            builder.HasKey(t => t.Id);

            builder.ToTable("Comments");
            builder.Property(x => x.Content).IsRequired();

            builder.HasOne(t => t.UserInformation).WithMany(pc => pc.Comments)
                .HasForeignKey(pc => pc.UserId);

            builder.HasOne(t => t.Recruitment).WithMany(pc => pc.Comments)
              .HasForeignKey(pc => pc.RecruimentId);
        }
    }
}
