using LangAppApi.Domain.Entities;
using LangAppApi.Persistence;
using System.Collections.Generic;

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
                new LangUser(),
                new LangUser(),
                new LangUser(),
            };
        }
    }
}