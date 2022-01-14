using Data.Configuration;
using Data.Entities;
using Data.Extension;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;

namespace Data.EF
{
    public class RecruimentWebsiteDbContext : IdentityDbContext<AppUser, AppRole, Guid>
    {
        public RecruimentWebsiteDbContext(DbContextOptions options) : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new AppUserConfiguration());
            modelBuilder.ApplyConfiguration(new AppRoleConfiguration());
            modelBuilder.ApplyConfiguration(new CompanyInformationConfiguration());
            modelBuilder.ApplyConfiguration(new UserInformationConfiguration());
            modelBuilder.ApplyConfiguration(new UserAvatarconfiguration());
            modelBuilder.ApplyConfiguration(new CompanyAvatarConfiguration());
            modelBuilder.ApplyConfiguration(new CompanyCoverImageConfiguration());
            modelBuilder.ApplyConfiguration(new CompanyImageConfiguration());
            modelBuilder.ApplyConfiguration(new CompanyBranchConfiguration());

            modelBuilder.Entity<IdentityUserClaim<Guid>>().ToTable("AppUserClaims");
            modelBuilder.Entity<IdentityUserRole<Guid>>().ToTable("AppUserRoles").HasKey(x => new { x.UserId, x.RoleId });
            modelBuilder.Entity<IdentityUserLogin<Guid>>().ToTable("AppUserLogins").HasKey(x => x.UserId);
            modelBuilder.Entity<IdentityRoleClaim<Guid>>().ToTable("AppRoleClaims");
            modelBuilder.Entity<IdentityUserToken<Guid>>().ToTable("AppUserTokens").HasKey(x => x.UserId);
            modelBuilder.Seed();
        }

        public DbSet<UserInformation> UserInformations { get; set; }
        public DbSet<CompanyInformation> CompanyInformations { get; set; }

    }
}
