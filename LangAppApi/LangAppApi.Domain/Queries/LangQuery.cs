using LangAppApi.Domain.Enum;

namespace LangAppApi.Domain.Queries
{
    public class LangQuery : PaginationQuery
    {
        public Lang? Language { get; set; }
        public LangLevel? WriteLevel { get; set; }
        public LangLevel? SpeakLevel { get; set; }
        public LangLevel? ComprehensionLevel { get; set; }
    }
}