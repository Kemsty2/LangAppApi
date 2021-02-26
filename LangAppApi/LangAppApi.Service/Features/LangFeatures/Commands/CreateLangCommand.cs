using AutoMapper;
using LangAppApi.Domain.Entities;
using LangAppApi.Domain.Enum;
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
    public class CreateLangCommand : IRequest<LangUser>
    {
        [Required]
        public Lang Language { get; set; }

        [Required]
        public LangLevel WriteLevel { get; set; }

        [Required]
        public LangLevel SpeakLevel { get; set; }

        [Required]
        public LangLevel ComprehensionLevel { get; set; }

        public class CreateLangCommandHandler : IRequestHandler<CreateLangCommand, LangUser>
        {
            private readonly IApplicationDbContext _context;
            private readonly IHttpContextAccessor _httpContextAccessor;

            public CreateLangCommandHandler(IApplicationDbContext context, IMapper mapper, IHttpContextAccessor httpContextAccessor)
            {
                _context = context;
                _httpContextAccessor = httpContextAccessor;
            }

            public async Task<LangUser> Handle(CreateLangCommand request, CancellationToken cancellationToken)
            {
                var user = _httpContextAccessor.GetUserId();
                var langUser = new LangUser
                {
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow,
                    CreatedBy = user.FirstName,
                    UpdatedBy = user.FirstName,
                    UserGuid = user.UserId,
                    Language = request.Language,
                    SpeakLevel = request.SpeakLevel,
                    WriteLevel = request.WriteLevel,
                    ComprehensionLevel = request.ComprehensionLevel
                };

                await _context.LangUsers.AddAsync(langUser, cancellationToken);
                await _context.SaveChangesAsync();
                return langUser;
            }
        }
    }
}