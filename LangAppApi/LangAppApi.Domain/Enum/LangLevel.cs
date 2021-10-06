using LangAppApi.Domain.Common;
using System.ComponentModel;

namespace LangAppApi.Domain.Enum
{
    [TypeConverter(typeof(EnumDescriptionTypeConverter))]
    public enum LangLevel
    {
        [LocalizedDescription("Starter", typeof(LangLevel))]
        Starter = 1,

        [LocalizedDescription("Basic", typeof(LangLevel))]
        Basic = 2,

        [LocalizedDescription("PreIntermediate", typeof(LangLevel))]
        PreIntermediate = 3,

        [LocalizedDescription("Intermediate", typeof(LangLevel))]
        Intermediate = 4,

        [LocalizedDescription("UpperIntermediate", typeof(LangLevel))]
        UpperIntermediate = 5,

        [LocalizedDescription("Advanced", typeof(LangLevel))]
        Advanced = 6
    }
}