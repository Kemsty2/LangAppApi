using System;

namespace LangAppApi.Infrastructure.ViewModel
{
    public class LangViewModel
    {
        public Guid Id { get; set; }
        public int Language { get; set; }
        public int WriteLevel { get; set; }
        public int SpeakLevel { get; set; }
        public int ComprehensionLevel { get; set; }

    }
}