using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Bloggie.Web.Data
{
    public class AuthDbContext : IdentityDbContext
    {
        public AuthDbContext(DbContextOptions<AuthDbContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            //=============={ Seeding Roles - (User, Admin & SuperAdmin) }==================

            var adminRoleId = "a47a89df-a2f3-486c-b238-71b5bfdff993";
            var superAdminRoleId = "d55697ae-7bf2-4704-a29b-70e84000a0ed";
            var userRoleId = "8f1900bc-bef2-46e0-be4f-650b9b747402";

            // creating list of diff roles
            var roles = new List<IdentityRole> {

                // admin role
                new IdentityRole()
                {
                    Name = "Admin",
                    NormalizedName = "Admin",
                    Id = adminRoleId,
                    ConcurrencyStamp = adminRoleId
                },

                // super admin role
                new IdentityRole()
                {
                    Name = "SuperAdmin",
                    NormalizedName = "SuperAdmin",
                    Id = superAdminRoleId,
                    ConcurrencyStamp = superAdminRoleId
                },

                // user role
                new IdentityRole()
                {
                    Name = "User",
                    NormalizedName = "User",
                    Id = userRoleId,
                    ConcurrencyStamp = userRoleId
                }
            };

            // upon execution, this roles will be seeded into database bt EF
            builder.Entity<IdentityRole>().HasData(roles);


            //======================={ Seeding SuperAdminUser }==========================

            var superAdminId = "1fe2de45-6071-4b42-b315-a15164b65d8b";

            var superAdminUser = new IdentityUser()
            {
                UserName = "superadmin@bloggie.com",
                Email = "superadmin@bloggie.com",
                NormalizedUserName = "superadmin@bloggie.com".ToUpper(),
                NormalizedEmail = "superadmin@bloggie.com".ToUpper(),
                Id = superAdminId,
            };

            string rawPassword = "Superadmin@2024";
            superAdminUser.PasswordHash = new PasswordHasher<IdentityUser>().HashPassword(superAdminUser, rawPassword);

            builder.Entity<IdentityUser>().HasData(superAdminUser);

            //=============={ Assigning all created roles to SuperAdminUser }==================

            var superAdminRoels = new List<IdentityUserRole<string>>()
            {
                new IdentityUserRole<string>()
                {
                    RoleId = adminRoleId,
                    UserId = superAdminId
                },

                new IdentityUserRole<string>()
                {
                    RoleId = superAdminRoleId,
                    UserId = superAdminId
                },

                new IdentityUserRole<string>()
                {
                    RoleId = userRoleId,
                    UserId = superAdminId
                }
            };

            builder.Entity<IdentityUserRole<string>>().HasData(superAdminRoels);
        }
    }
}
