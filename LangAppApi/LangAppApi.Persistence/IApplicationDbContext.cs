using LangAppApi.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace LangAppApi.Persistence
{
    public interface IApplicationDbContext
    {
        DbSet<Customer> Customers { get; set; }

        Task<int> SaveChangesAsync();
    }
}