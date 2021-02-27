using AutoMapper;
using LangAppApi.Domain.Entities;
using LangAppApi.Domain.Enum;
using LangAppApi.Infrastructure.ViewModel;
using System.ComponentModel;

namespace LangAppApi.Infrastructure.Mapping.Resolvers
{
    public class LangEnumResolver : IValueResolver<LangUser, LangViewModel, string>
    {
        public string Resolve(LangUser source, LangViewModel destination, string destMember, ResolutionContext context)
        {
            var converter = TypeDescriptor.GetConverter(typeof(Lang));
            return converter.ConvertToString(source.Language);
        }
    }
}