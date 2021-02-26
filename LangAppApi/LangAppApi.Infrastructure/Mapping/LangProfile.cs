using AutoMapper;
using LangAppApi.Domain.Entities;
using LangAppApi.Service.Features.LangFeatures.Commands;

namespace LangAppApi.Infrastructure.Mapping
{
    public class LangProfile : Profile
    {
        public LangProfile()
        {
            CreateMap<CreateLangCommand, LangUser>().ReverseMap();
        }
    }
}