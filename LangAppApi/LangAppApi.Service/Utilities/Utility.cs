using LangAppApi.Domain;
using LangAppApi.Domain.Auth;
using LangAppApi.Domain.Exceptions;
using LangAppApi.Domain.Queries;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace LangAppApi.Service.Utilities
{
    public static class Utility
    {
        public static User GetUserId(this IHttpContextAccessor httpContextAccessor)
        {
            var userClaim = httpContextAccessor.HttpContext.User.FindFirst("uid");
            var firstNameClaim = httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier);

            var userId = userClaim != null ? userClaim.Value : "";
            var firstName = firstNameClaim != null ? firstNameClaim.Value : "";
            return new User(firstName, userId);
        }

        public static IQueryable<T> ApplyRestrictions<T>(this IQueryable<T> query, BaseQuery pagingParams, IReadOnlyCollection<Expression<Func<T, bool>>> expressions = null) where T : BaseEntity
        {
            var openDate = pagingParams.OpenDate ?? DateTime.MinValue;
            var closeDate = pagingParams.CloseDate ?? DateTime.MaxValue;

            if (expressions != null)
            {
                query = expressions.Aggregate(query, (current, expression) => current.Where(expression));
            }

            if (Convert.ToDateTime(openDate) > Convert.ToDateTime(closeDate))
            {
                throw new BadRequestException("OpenDate must be less than or equal to CloseDate!");
            }

            openDate = Convert.ToDateTime(Convert.ToDateTime(openDate).ToString("yyyy-MM-dd 00:00:00"));
            closeDate = Convert.ToDateTime(Convert.ToDateTime(closeDate).ToString("yyyy-MM-dd 23:59:59"));

            return query.Where(o => o.CreatedAt >= openDate)
                .Where(o => o.CreatedAt <= closeDate)
                .Where(o => !o.IsDeleted)
                .OrderByDescending(t => t.CreatedAt);
        }

        public static async Task<List<T>> GetItems<T>(this IQueryable<T> query, PaginationQuery pagingParams) where T : class
        {
            return await query
                .Skip(pagingParams.PageSize * (pagingParams.PageNumber - 1))
                .Take(pagingParams.PageSize)
                .ToListAsync();
        }
    }
}