using LangAppApi.Domain.Common;
using System.ComponentModel;

namespace LangAppApi.Domain.Enum
{
    [TypeConverter(typeof(EnumDescriptionTypeConverter))]
    public enum Lang
    {
        [LocalizedDescription("French", typeof(Lang))]
        French,

        [LocalizedDescription("English", typeof(Lang))]
        English,

        [LocalizedDescription("Dutch", typeof(Lang))]
        Dutch
    }
}