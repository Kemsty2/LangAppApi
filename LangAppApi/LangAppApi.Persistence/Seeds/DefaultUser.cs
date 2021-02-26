using LangAppApi.Domain.Auth;
using LangAppApi.Domain.Enum;
using System.Collections.Generic;

namespace LangAppApi.Persistence.Seeds
{
    public static class DefaultUser
    {
        public static IEnumerable<ApplicationUser> IdentityBasicUserList()
        {
            return new List<ApplicationUser>()
            {
                new ApplicationUser
                {
                    Id = Constants.SuperAdminUser,
                    UserName = "superadmin",
                    Email = "superadmin@gmail.com",
                    FirstName = "Steeve",
                    LastName = "Kemgne",
                    EmailConfirmed = true,
                    PhoneNumberConfirmed = true,
                    // Password@123
                    PasswordHash = "AQAAAAEAACcQAAAAEBLjouNqaeiVWbN0TbXUS3+ChW3d7aQIk/BQEkWBxlrdRRngp14b0BIH0Rp65qD6mA==",
                    NormalizedEmail= "SUPERADMIN@GMAIL.COM",
                    NormalizedUserName="SUPERADMIN"
                },
                new ApplicationUser
                {
                    Id = Constants.BasicUser,
                    UserName = "basicuser",
                    Email = "basicuser@gmail.com",
                    FirstName = "Aymard",
                    LastName = "Moyo",
                    EmailConfirmed = true,
                    PhoneNumberConfirmed = true,
                    // Password@123
                    PasswordHash = "AQAAAAEAACcQAAAAEBLjouNqaeiVWbN0TbXUS3+ChW3d7aQIk/BQEkWBxlrdRRngp14b0BIH0Rp65qD6mA==",
                    NormalizedEmail= "BASICUSER@GMAIL.COM",
                    NormalizedUserName="BASICUSER"
                },
            };
        }
    }
}