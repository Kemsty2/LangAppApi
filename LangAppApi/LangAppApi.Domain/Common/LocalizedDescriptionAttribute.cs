using System;
using System.ComponentModel;
using System.Resources;

namespace LangAppApi.Domain.Common
{
    public class LocalizedDescriptionAttribute : DescriptionAttribute
    {
        private readonly ResourceManager _resourceManager;
        private readonly string _resourceKey;

        public LocalizedDescriptionAttribute(string resourceKey, Type resourceType)
        {
            _resourceKey = resourceKey;
            _resourceManager = new ResourceManager(resourceType);
        }

        public override string Description
        {
            get
            {
                var description = _resourceManager.GetString(_resourceKey);
                return string.IsNullOrWhiteSpace(description) ? $"[[{_resourceKey}]]" : description;
            }
        }
    }
}