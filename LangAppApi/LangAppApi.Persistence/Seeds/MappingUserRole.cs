using LangAppApi.Domain.Enum;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace LangAppApi.Persistence.Seeds
{
    public static class MappingUserRole
    {
        public static IEnumerable<IdentityUserRole<string>> IdentityUserRoleList()
        {
            return new List<IdentityUserRole<string>>()
            {
                new IdentityUserRole<string>
                {
                    RoleId = Constants.Basic,
                    UserId = Constants.BasicUser
                },
                new IdentityUserRole<string>
                {
                    RoleId = Constants.SuperAdmin,
                    UserId = Constants.SuperAdminUser
                },
                new IdentityUserRole<string>
                {
                    RoleId = Constants.Basic,
                    UserId = Constants.SuperAdminUser
                }
            };
        }
    }
}