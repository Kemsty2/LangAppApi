using AutoMapper;
using LangAppApi.Domain.Enum;
using System.ComponentModel;

namespace LangAppApi.Infrastructure.Mapping.Converters
{
    public class LangLevelTypeConverter : ITypeConverter<LangLevel, string>
    {
        public string Convert(LangLevel source, string destination, ResolutionContext context)
        {
            var converter = TypeDescriptor.GetConverter(typeof(LangLevel));
            return converter.ConvertToString(source);
        }
    }
}