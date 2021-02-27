using System;
using LangAppApi.Domain.Entities;
using LangAppApi.Persistence;
using System.Collections.Generic;
using LangAppApi.Domain.Enum;

namespace LangAppApi.Test.Integration.Common
{
    public static class Utilities
    {
        public static void InitializeDbForTests(ApplicationDbContext db)
        {
            db.LangUsers.AddRange(GetSeedingLanguages());
            db.SaveChanges();
        }

        public static void ReinitializeDbForTests(ApplicationDbContext db)
        {
            db.LangUsers.RemoveRange(db.LangUsers);
            InitializeDbForTests(db);
        }

        public static IEnumerable<LangUser> GetSeedingLanguages()
        {
            return new List<LangUser>
            {
                new LangUser()
                {
                    Id = new Guid("30792a4d-3915-4bd9-b4f9-7683bd64ecce"),
                    Language = Lang.Dutch,
                    SpeakLevel = LangLevel.Advanced,
                    WriteLevel = LangLevel.Basic,
                    ComprehensionLevel = LangLevel.Intermediate,
                    UserGuid = "58883ada-76f9-45fd-b860-c214ae3670a9",
                    CreatedBy = "admin",
                    UpdatedBy = "admin"
                },
                new LangUser()
                {
                    Id = new Guid("d451ca14-73d8-4bea-9501-d228be43005d"),
                    Language = Lang.Dutch,
                    SpeakLevel = LangLevel.Advanced,
                    WriteLevel = LangLevel.Basic,
                    ComprehensionLevel = LangLevel.Intermediate,
                    UserGuid = "58883ada-76f9-45fd-b860-c214ae3670a8",
                    CreatedBy = "superadmin",
                    UpdatedBy = "superadmin"
                },
                new LangUser()
                {
                    Id = new Guid("13f0347d-2e80-4f4e-a38f-9c56e44d21a7"),
                    Language = Lang.Dutch,
                    SpeakLevel = LangLevel.Advanced,
                    WriteLevel = LangLevel.Basic,
                    ComprehensionLevel = LangLevel.Intermediate,
                    UserGuid = "58883ada-76f9-45fd-b860-c214ae3670a8",
                    CreatedBy = "superadmin",
                    UpdatedBy = "superadmin"
                },
            };
        }
    }
}