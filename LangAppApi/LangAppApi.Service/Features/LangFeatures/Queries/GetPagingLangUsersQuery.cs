using LangAppApi.Domain.Common;
using LangAppApi.Domain.Entities;
using LangAppApi.Domain.Queries;
using LangAppApi.Persistence;
using LangAppApi.Service.Utilities;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace LangAppApi.Service.Features.LangFeatures.Queries
{
    public class GetPagingLangUsersQuery : IRequest<PagingResponse<LangUser>>
    {
        public LangQuery Query { get; set; }

        public class GetPagingRequestsHandler : IRequestHandler<GetPagingLangUsersQuery, PagingResponse<LangUser>>
        {
            private readonly IApplicationDbContext _context;
            private readonly IHttpContextAccessor _httpContextAccessor;

            public GetPagingRequestsHandler(IApplicationDbContext context, IHttpContextAccessor httpContextAccessor)
            {
                _context = context;
                _httpContextAccessor = httpContextAccessor;
            }

            public async Task<PagingResponse<LangUser>> Handle(GetPagingLangUsersQuery request, CancellationToken cancellationToken)
            {
                var user = _httpContextAccessor.GetUserId();

                var othersRestrictions = new List<Expression<Func<LangUser, bool>>>()
                {
                    (x => !x.IsDeleted),
                    (x => x.UserGuid == user.UserId)
                };

                if (request.Query.Language.HasValue)
                {
                    othersRestrictions.Add((x => x.Language == request.Query.Language.Value));
                }

                if (request.Query.SpeakLevel.HasValue)
                {
                    othersRestrictions.Add((x => x.SpeakLevel == request.Query.SpeakLevel.Value));
                }

                if (request.Query.ComprehensionLevel.HasValue)
                {
                    othersRestrictions.Add((x => x.ComprehensionLevel == request.Query.ComprehensionLevel.Value));
                }

                if (request.Query.WriteLevel.HasValue)
                {
                    othersRestrictions.Add((x => x.WriteLevel == request.Query.WriteLevel.Value));
                }

                var query = _context.LangUsers.AsQueryable();

                query = query.ApplyRestrictions(request.Query, othersRestrictions);

                var totalItems = await query.CountAsync(cancellationToken);
                var items = await query.GetItems(request.Query);

                return new PagingResponse<LangUser>(totalItems, items, request.Query);
            }
        }
    }
}