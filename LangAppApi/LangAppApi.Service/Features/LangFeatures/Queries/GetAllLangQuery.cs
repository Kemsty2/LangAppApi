using LangAppApi.Domain.Entities;
using LangAppApi.Persistence;
using LangAppApi.Service.Utilities;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace LangAppApi.Service.Features.LangFeatures.Queries
{
    public class GetAllLangQuery : IRequest<IEnumerable<LangUser>>
    {
        public class GetAllLangQueryHandler : IRequestHandler<GetAllLangQuery, IEnumerable<LangUser>>
        {
            private readonly IApplicationDbContext _context;
            private readonly IHttpContextAccessor _httpContextAccessor;

            public GetAllLangQueryHandler(IApplicationDbContext context, IHttpContextAccessor httpContextAccessor)
            {
                _context = context;
                _httpContextAccessor = httpContextAccessor;
            }

            public async Task<IEnumerable<LangUser>> Handle(GetAllLangQuery request, CancellationToken cancellationToken)
            {
                var user = _httpContextAccessor.GetUserId();

                var restrictions = new List<Expression<Func<LangUser, bool>>>
                {
                    (x => !x.IsDeleted),
                    (x => x.UserGuid == user.UserId)
                };

                var query = _context.LangUsers.AsQueryable();
                query = restrictions.Aggregate(query, (current, expression) => current.Where(expression));

                var langList = await query.ToListAsync(cancellationToken);
                return langList?.AsReadOnly();
            }
        }
    }
}