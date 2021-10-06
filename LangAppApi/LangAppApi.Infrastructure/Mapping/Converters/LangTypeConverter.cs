using AutoMapper;
using LangAppApi.Domain.Enum;
using System.ComponentModel;

namespace LangAppApi.Infrastructure.Mapping.Converters
{
    public class LangTypeConverter : ITypeConverter<Lang, int>
    {
        public int Convert(Lang source, int destination, ResolutionContext context)
        {
            //var converter = TypeDescriptor.GetConverter(typeof(Lang));
            return (int)source;
        }
    }
}