using LangAppApi.Domain.Common;
using System.ComponentModel;

namespace LangAppApi.Domain.Enum
{
    [TypeConverter(typeof(EnumDescriptionTypeConverter))]
    public enum LangLevel
    {
        [LocalizedDescription("Starter", typeof(LangLevel))]
        Starter,

        [LocalizedDescription("Basic", typeof(LangLevel))]
        Basic,

        [LocalizedDescription("PreIntermediate", typeof(LangLevel))]
        PreIntermediate,

        [LocalizedDescription("Intermediate", typeof(LangLevel))]
        Intermediate,

        [LocalizedDescription("UpperIntermediate", typeof(LangLevel))]
        UpperIntermediate,

        [LocalizedDescription("Advanced", typeof(LangLevel))]
        Advanced
    }
}