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
            var roleId = Guid.NewGuid();
            modelBuilder.Entity<AppRole>().HasData(new AppRole
            {
                Id = roleId,
                Name = "company",
                NormalizedName = "company",
                Description = "Company role"
            });

           


            var roleId1 = Guid.NewGuid();
            modelBuilder.Entity<AppRole>().HasData(new AppRole
            {
                Id = roleId1,
                Name = "user",
                NormalizedName = "user",
                Description = "User role"
            });

            

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
