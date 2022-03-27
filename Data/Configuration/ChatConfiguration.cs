using Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

namespace Data.Configuration
{
    public class ChatConfiguration : IEntityTypeConfiguration<Chat>
    {
        public void Configure(EntityTypeBuilder<Chat> builder)
        {
            builder.ToTable("Chats");

            builder.HasKey(t => t.Id);
            builder.Property(t => t.Id).UseIdentityColumn();

            builder.Property(t => t.Content).IsRequired();
            builder.Property(t => t.Performer).IsRequired();
            builder.Property(t => t.DateCreated).IsRequired().HasDefaultValue(DateTime.Now);

            builder.HasOne(t => t.UserInformation).WithMany(pc => pc.Chats)
                .HasForeignKey(pc => pc.UserId).OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(t => t.CompanyInformation).WithMany(pc => pc.Chats)
              .HasForeignKey(pc => pc.CompanyId).OnDelete(DeleteBehavior.Restrict);
        }
    }
}
