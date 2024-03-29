﻿using AutoMapper;
using LangAppApi.Domain.Entities;
using LangAppApi.Domain.Enum;
using LangAppApi.Infrastructure.Mapping.Converters;
using LangAppApi.Infrastructure.ViewModel;
using LangAppApi.Service.Features.LangFeatures.Commands;

namespace LangAppApi.Infrastructure.Mapping
{
    public class LangProfile : Profile
    {
        public LangProfile()
        {
            CreateMap<CreateLangCommand, LangUser>().ReverseMap();

            CreateMap<Lang, int>().ConvertUsing<LangTypeConverter>();
            CreateMap<LangLevel, int>().ConvertUsing<LangLevelTypeConverter>();

            CreateMap<LangUser, LangViewModel>();
        }
    }
}