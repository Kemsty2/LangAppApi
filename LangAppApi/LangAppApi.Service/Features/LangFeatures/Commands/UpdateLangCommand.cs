using LangAppApi.Domain.Entities;
using LangAppApi.Domain.Enum;
using LangAppApi.Domain.Exceptions;
using LangAppApi.Persistence;
using LangAppApi.Service.Utilities;
using MediatR;
using Microsoft.AspNetCore.Http;
using System;
using System.ComponentModel.DataAnnotations;
using System.Threading;
using System.Threading.Tasks;

namespace LangAppApi.Service.Features.LangFeatures.Commands
{
    public class UpdateLangCommand : IRequest<LangUser>
    {
        public Guid Id { get; set; }

        [Required]
        public Lang Language { get; set; }

        [Required]
        public LangLevel WriteLevel { get; set; }

        [Required]
        public LangLevel SpeakLevel { get; set; }

        [Required]
        public LangLevel ComprehensionLevel { get; set; }

        public class UpdateLangCommandHandler : IRequestHandler<UpdateLangCommand, LangUser>
        {
            private readonly IApplicationDbContext _context;
            private readonly IHttpContextAccessor _httpContextAccessor;

            public UpdateLangCommandHandler(IApplicationDbContext context, IHttpContextAccessor httpContextAccessor)
            {
                _context = context;
                _httpContextAccessor = httpContextAccessor;
            }

            public async Task<LangUser> Handle(UpdateLangCommand request, CancellationToken cancellationToken)
            {
                var user = _httpContextAccessor.GetUserId();
                var lang = await _context.LangUsers.FindAsync(request.Id);

                if (lang == null) throw new NotFoundException("Request", request.Id);

                lang.SpeakLevel = request.SpeakLevel;
                lang.ComprehensionLevel = request.ComprehensionLevel;
                lang.WriteLevel = request.WriteLevel;
                lang.Language = request.Language;
                lang.UpdatedAt = DateTime.UtcNow;
                lang.UpdatedBy = user.FirstName;

                _context.LangUsers.Update(lang);
                await _context.SaveChangesAsync();
                return lang;
            }
        }
    }
}