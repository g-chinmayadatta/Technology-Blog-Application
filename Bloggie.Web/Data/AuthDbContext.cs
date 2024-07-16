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
            // seed some data user roles (Super Admin, admin,user)
            var adminRoleId = "3b09544b-aa7a-43e3-b53f-8d947a8bfa8e";
            var superAdminRoleId = "8faee7f4-2c93-4363-acdd-f998c7d7ea79";
            var userRoleId = "e8142597-0426-4fd1-b954-be2aca09b953";
            var roles = new List<IdentityRole>
            {
                new IdentityRole
                {
                    Name="Admin",
                    NormalizedName="Admin",
                    Id=adminRoleId,
                    ConcurrencyStamp=adminRoleId

                },
                 new IdentityRole
                {
                    Name="SuperAdmin",
                    NormalizedName="SuperAdmin",
                    Id=superAdminRoleId,
                    ConcurrencyStamp=superAdminRoleId

                },
                  new IdentityRole
                {
                    Name="User",
                    NormalizedName="User",
                    Id=userRoleId,
                    ConcurrencyStamp=userRoleId

                }
            };

            builder.Entity<IdentityRole>().HasData(roles);
            // seed superAdmin
            var superAdminId = "5eb802c2-0d3f-4701-a443-84bfc4010acb";
            var superAdminUser = new IdentityUser
            {
                UserName = "superadmin@bloggie.com",
                Email = "superadmin@bloggie.com",
                NormalizedEmail = "superadmin@bloggie.com".ToUpper(),
                NormalizedUserName= "superadmin@bloggie.com".ToUpper(),
                Id=superAdminId
            };

            // create a password for the user
            superAdminUser.PasswordHash = new PasswordHasher<IdentityUser>()
                .HashPassword(superAdminUser, "Superadmin@123#asd");

            builder.Entity<IdentityUser>().HasData(superAdminUser);
            // add all the roles to super admin
            var superAdminRoles = new List<IdentityUserRole<string>>
             {
                 new IdentityUserRole<string>
                 {
                    RoleId=adminRoleId,
                    UserId=superAdminId

                 },
                 new IdentityUserRole<string>
                 {
                    RoleId=superAdminRoleId,
                    UserId=superAdminId

                 },
                 new IdentityUserRole<string>
                 {
                    RoleId=userRoleId,
                    UserId=superAdminId

                 },
             };
            builder.Entity<IdentityUserRole<string>>().HasData(superAdminRoles);
        }
    }
}
