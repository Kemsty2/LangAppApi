using AutoMapper;
using LangAppApi.Domain.Enum;
using System.ComponentModel;

namespace LangAppApi.Infrastructure.Mapping.Converters
{
    public class LangTypeConverter : ITypeConverter<Lang, string>
    {
        public string Convert(Lang source, string destination, ResolutionContext context)
        {
            var converter = TypeDescriptor.GetConverter(typeof(Lang));
            return converter.ConvertToString(source);
        }
    }
}