using Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Data.Configuration
{
    public class BranchConfiguration : IEntityTypeConfiguration<Branch>
    {
        public void Configure(EntityTypeBuilder<Branch> builder)
        {
            builder.ToTable("Branches");

            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).UseIdentityColumn();


            builder.Property(x => x.City).IsRequired();

        }
    }
}
