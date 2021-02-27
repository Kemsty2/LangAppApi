using LangAppApi.Domain.Entities;
using LangAppApi.Persistence;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using LangAppApi.Domain.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace LangAppApi.Service.Features.LangFeatures.Queries
{
    public class GetLangByIdQuery : IRequest<LangUser>
    {
        public Guid Id { get; set; }

        public class GetLangByIdQueryHandler : IRequestHandler<GetLangByIdQuery, LangUser>
        {
            private readonly IApplicationDbContext _context;

            public GetLangByIdQueryHandler(IApplicationDbContext context)
            {
                _context = context;
            }

            public async Task<LangUser> Handle(GetLangByIdQuery request, CancellationToken cancellationToken)
            {
                var lang = await _context.LangUsers.FirstOrDefaultAsync(x => x.Id == request.Id && !x.IsDeleted, cancellationToken);
                if (lang == null) throw new NotFoundException("Lang", request.Id);
                return lang;
            }
        }
    }

}