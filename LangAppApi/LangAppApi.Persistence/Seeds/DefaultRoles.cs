using LangAppApi.Domain.Enum;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace LangAppApi.Persistence.Seeds
{
    public static class DefaultRoles
    {
        public static IEnumerable<IdentityRole> IdentityRoleList()
        {
            return new List<IdentityRole>()
            {
                new IdentityRole
                {
                    Id = Constants.SuperAdmin,
                    Name = Roles.SuperAdmin.ToString(),
                    NormalizedName = Roles.SuperAdmin.ToString()
                },
                new IdentityRole
                {
                    Id = Constants.Basic,
                    Name = Roles.Basic.ToString(),
                    NormalizedName = Roles.Basic.ToString()
                }
            };
        }
    }
}