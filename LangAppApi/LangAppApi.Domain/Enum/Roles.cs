using System;

namespace LangAppApi.Domain.Enum
{
    public enum Roles
    {
        SuperAdmin,
        Basic
    }

    public static class Constants
    {
        public static readonly string SuperAdmin = Guid.NewGuid().ToString();
        public static readonly string Basic = Guid.NewGuid().ToString();

        public static readonly string SuperAdminUser = Guid.NewGuid().ToString();
        public static readonly string BasicUser = Guid.NewGuid().ToString();
    }
}