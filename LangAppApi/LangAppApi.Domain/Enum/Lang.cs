using LangAppApi.Domain.Common;
using System.ComponentModel;

namespace LangAppApi.Domain.Enum
{
    [TypeConverter(typeof(EnumDescriptionTypeConverter))]
    public enum Lang
    {
        [LocalizedDescription("French", typeof(Lang))]
        French = 1,

        [LocalizedDescription("English", typeof(Lang))]
        English = 2,

        [LocalizedDescription("Dutch", typeof(Lang))]
        Dutch = 3
    }
}