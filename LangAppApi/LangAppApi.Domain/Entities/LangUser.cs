using LangAppApi.Domain.Enum;

namespace LangAppApi.Domain.Entities
{
    public class LangUser : BaseEntity
    {
        public Lang Language { get; set; }
        public LangLevel WriteLevel { get; set; }
        public LangLevel SpeakLevel { get; set; }
        public LangLevel ComprehensionLevel { get; set; }
        public string UserGuid { get; set; }
    }
}