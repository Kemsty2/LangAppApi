using System;

namespace LangAppApi.Infrastructure.ViewModel
{
    public class LangViewModel
    {
        public Guid Id { get; set; }
        public string Language { get; set; }
        public string WriteLevel { get; set; }
        public string SpeakLevel { get; set; }
        public string ComprehensionLevel { get; set; }

    }
}