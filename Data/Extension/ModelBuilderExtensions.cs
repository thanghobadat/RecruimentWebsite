using Data.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;

namespace Data.Extension
{
    public static class ModelBuilderExtensions
    {
        public static void Seed(this ModelBuilder modelBuilder)
        {


            // any guid
            //var roleId = new Guid("8D04DCE2-969A-435D-BBA4-DF3F325983DC");
            //var companyId = new Guid("69BD714F-9576-45BA-B5B7-F00649BE00DE");
            //modelBuilder.Entity<AppRole>().HasData(new AppRole
            //{
            //    Id = roleId,
            //    Name = "company",
            //    NormalizedName = "company",
            //    Description = "Company role"
            //});

            //var hasher = new PasswordHasher<AppUser>();
            //modelBuilder.Entity<AppUser>().HasData(new AppUser
            //{
            //    Id = companyId,
            //    UserName = "admin",
            //    NormalizedUserName = "admin",
            //    Email = "hoangthanh01022000@gmail.com",
            //    NormalizedEmail = "hoangthanh01022000@gmail.com",
            //    EmailConfirmed = true,
            //    PasswordHash = hasher.HashPassword(null, "Admin@123"),
            //    SecurityStamp = string.Empty,
            //    DateCreated = DateTime.Now,
            //    IsSave = false
            //});

            //modelBuilder.Entity<IdentityUserRole<Guid>>().HasData(new IdentityUserRole<Guid>
            //{
            //    RoleId = roleId,
            //    UserId = companyId
            //});


            //var roleId1 = Guid.NewGuid();
            //var userId = Guid.NewGuid();
            //modelBuilder.Entity<AppRole>().HasData(new AppRole
            //{
            //    Id = roleId1,
            //    Name = "user",
            //    NormalizedName = "user",
            //    Description = "User role"
            //});

            //var hasher1 = new PasswordHasher<AppUser>();
            //modelBuilder.Entity<AppUser>().HasData(new AppUser
            //{
            //    Id = userId,
            //    UserName = "user",
            //    NormalizedUserName = "user",
            //    Email = "hoangthanh01022000@gmail.com",
            //    NormalizedEmail = "hoangthanh01022000@gmail.com",
            //    EmailConfirmed = true,
            //    PasswordHash = hasher1.HashPassword(null, "Admin@123"),
            //    SecurityStamp = string.Empty,
            //    DateCreated = DateTime.Now,
            //    IsSave = false
            //});

            //modelBuilder.Entity<IdentityUserRole<Guid>>().HasData(new IdentityUserRole<Guid>
            //{
            //    RoleId = roleId1,
            //    UserId = userId
            //});

            var roleId2 = Guid.NewGuid();
            var adminId = Guid.NewGuid();
            modelBuilder.Entity<AppRole>().HasData(new AppRole
            {
                Id = roleId2,
                Name = "admin",
                NormalizedName = "admin",
                Description = "Admin role"
            });

            var hasher2 = new PasswordHasher<AppUser>();
            modelBuilder.Entity<AppUser>().HasData(new AppUser
            {
                Id = adminId,
                UserName = "admin",
                NormalizedUserName = "admin",
                Email = "hoangthanh01022000@gmail.com",
                NormalizedEmail = "hoangthanh01022000@gmail.com",
                EmailConfirmed = true,
                PasswordHash = hasher2.HashPassword(null, "Admin@123"),
                SecurityStamp = string.Empty,
                DateCreated = DateTime.Now,
                IsSave = false
            });

            modelBuilder.Entity<IdentityUserRole<Guid>>().HasData(new IdentityUserRole<Guid>
            {
                RoleId = roleId2,
                UserId = adminId
            });

        }
    }
}
