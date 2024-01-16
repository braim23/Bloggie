using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Bloggie.Web.Data;

public class AuthDbContext : IdentityDbContext
{
    public AuthDbContext(DbContextOptions<AuthDbContext> options) : base(options)
    {
    }


    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        // Seed the Roles (User, Admin, SuperAdmin
        var adminRoleId = "11f83d08-dac2-4f11-9015-561252c48ab3";
        var superAdminRoleId = "c76bf00a-7cd9-4b93-bdd4-0814d87f3bf7";
        var userRoleId = "e45f82fe-5a61-4f36-90fb-1128b3bd140a";


        var roles = new List<IdentityRole>
        {
            new IdentityRole
            {
                Name = "admin",
                NormalizedName = "admin",
                Id = adminRoleId,
                ConcurrencyStamp = adminRoleId
            },

            new IdentityRole
            {
                Name = "SuperAdmin",
                NormalizedName = "SuperAdmin",
                Id = superAdminRoleId,
                ConcurrencyStamp = superAdminRoleId
            },
             new IdentityRole
             {
                 Name= "user",
                 NormalizedName = "user",
                 Id = userRoleId,
                 ConcurrencyStamp = userRoleId
             }
        };

        builder.Entity<IdentityRole>().HasData(roles);

        // Seed SuperAdminUser
        var superAdminId = "127532ae-b975-4f45-9f4e-a742c2ba333f";
        var superAdminUser = new IdentityUser
        {
            UserName = "superadmin@bloggie.com",
            Email = "superadmin@bloggie.com",
            NormalizedEmail = "superadmin@bloggie.com".ToUpper(),
            NormalizedUserName = "superadmin@bloggie.com".ToUpper(),
            Id = superAdminId
        };
        superAdminUser.PasswordHash = new PasswordHasher<IdentityUser>().HashPassword(superAdminUser, "1q2w3E*");

        builder.Entity<IdentityUser>().HasData(superAdminUser);

        // Add All roles to SuperAdminUser

        var superAdminRoles = new List<IdentityUserRole<string>>
        {
            new IdentityUserRole<string>
            {
                RoleId = superAdminRoleId,
                UserId = superAdminId
            },
            new IdentityUserRole<string>
            {
                RoleId = adminRoleId,
                UserId = superAdminId
            },
            new IdentityUserRole<string>
            {
                RoleId = userRoleId,
                UserId = superAdminId
            }
        };

        builder.Entity<IdentityUserRole<string>>().HasData(superAdminRoles);

    }


}
