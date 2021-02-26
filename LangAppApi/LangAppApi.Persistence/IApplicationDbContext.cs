using LangAppApi.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace LangAppApi.Persistence
{
    public interface IApplicationDbContext
    {
        DbSet<LangUser> LangUsers { get; set; }

        Task<int> SaveChangesAsync();
    }
}