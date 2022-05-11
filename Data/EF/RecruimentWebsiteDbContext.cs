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
            modelBuilder.ApplyConfiguration(new BranchConfiguration());
            modelBuilder.ApplyConfiguration(new BranchRecruitmentConfiguration());
            modelBuilder.ApplyConfiguration(new CareerConfiguration());
            modelBuilder.ApplyConfiguration(new CareerRecruitmentConfiguration());
            modelBuilder.ApplyConfiguration(new ChatConfiguration());
            modelBuilder.ApplyConfiguration(new CommentConfiguration());
            modelBuilder.ApplyConfiguration(new CompanyAvatarConfiguration());
            modelBuilder.ApplyConfiguration(new CompanyBranchConfiguration());
            modelBuilder.ApplyConfiguration(new CompanyCoverImageConfiguration());
            modelBuilder.ApplyConfiguration(new CompanyImageConfiguration());
            modelBuilder.ApplyConfiguration(new CompanyInformationConfiguration());
            modelBuilder.ApplyConfiguration(new CurriculumVitaeConfiguration());
            modelBuilder.ApplyConfiguration(new FollowConfiguration());
            modelBuilder.ApplyConfiguration(new NotificationConfiguration());
            modelBuilder.ApplyConfiguration(new RecruitmentConfiguration());
            modelBuilder.ApplyConfiguration(new UserAvatarconfiguration());
            modelBuilder.ApplyConfiguration(new UserInformationConfiguration());
            modelBuilder.ApplyConfiguration(new MailSettingConfiguration());
            modelBuilder.Entity<IdentityUserClaim<Guid>>().ToTable("AppUserClaims");
            modelBuilder.Entity<IdentityUserRole<Guid>>().ToTable("AppUserRoles").HasKey(x => new { x.UserId, x.RoleId });
            modelBuilder.Entity<IdentityUserLogin<Guid>>().ToTable("AppUserLogins").HasKey(x => x.UserId);
            modelBuilder.Entity<IdentityRoleClaim<Guid>>().ToTable("AppRoleClaims");
            modelBuilder.Entity<IdentityUserToken<Guid>>().ToTable("AppUserTokens").HasKey(x => x.UserId);
            modelBuilder.Seed();
        }

        public DbSet<UserInformation> UserInformations { get; set; }
        public DbSet<MailSetting> MailSettings { get; set; }
        public DbSet<CompanyInformation> CompanyInformations { get; set; }
        public DbSet<CompanyAvatar> CompanyAvatars { get; set; }
        public DbSet<CompanyBranch> CompanyBranches { get; set; }
        public DbSet<CompanyImage> CompanyImages { get; set; }
        public DbSet<UserAvatar> UserAvatars { get; set; }
        public DbSet<CompanyCoverImage> CompanyCoverImages { get; set; }
        public DbSet<Branch> Branches { get; set; }
        public DbSet<BranchRecruitment> BranchRecruitments { get; set; }
        public DbSet<Career> Careers { get; set; }
        public DbSet<CareerRecruitment> CareerRecruitments { get; set; }
        public DbSet<Chat> Chats { get; set; }
        public DbSet<CurriculumVitae> CurriculumVitaes { get; set; }
        public DbSet<Follow> Follows { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<Recruitment> Recruitments { get; set; }
        public DbSet<Comment> Comments { get; set; }

    }
}
