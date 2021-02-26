using LangAppApi.Persistence;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using LangAppApi.Domain.Exceptions;

namespace LangAppApi.Service.Features.LangFeatures.Commands
{
    public class DeleteLangByIdCommand : IRequest<Guid>
    {
        public Guid Id { get; set; }

        public class DeleteLangByIdCommandHandler : IRequestHandler<DeleteLangByIdCommand, Guid>
        {
            private readonly IApplicationDbContext _context;

            public DeleteLangByIdCommandHandler(IApplicationDbContext context)
            {
                _context = context;
            }

            public async Task<Guid> Handle(DeleteLangByIdCommand request, CancellationToken cancellationToken)
            {
                var lang = await _context.LangUsers.FindAsync(request.Id);

                if (lang == null) throw new NotFoundException("Request", request.Id);

                _context.LangUsers.Remove(lang);
                await _context.SaveChangesAsync();
                return lang.Id;
            }
        }
    }
}