using AutoMapper;
using LangAppApi.Domain.Enum;
using System.ComponentModel;

namespace LangAppApi.Infrastructure.Mapping.Converters
{
    public class LangLevelTypeConverter : ITypeConverter<LangLevel, int>
    {
        public int Convert(LangLevel source, int destination, ResolutionContext context)
        {
            var converter = TypeDescriptor.GetConverter(typeof(LangLevel));
            return (int)source;
        }
    }
}