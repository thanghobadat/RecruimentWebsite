using Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

namespace Data.Configuration
{
    public class NotificationConfiguration : IEntityTypeConfiguration<Notification>
    {
        public void Configure(EntityTypeBuilder<Notification> builder)
        {
            builder.HasKey(t => t.Id);

            builder.ToTable("Notifications");
            builder.Property(x => x.Content).IsRequired();
            builder.Property(x => x.DateCreated).IsRequired().HasDefaultValue(DateTime.Now);

            builder.HasOne(t => t.AppUser).WithMany(pc => pc.Notifications)
                .HasForeignKey(pc => pc.AccountId);
            //.OnDelete(DeleteBehavior.Restrict)



        }
    }
}
