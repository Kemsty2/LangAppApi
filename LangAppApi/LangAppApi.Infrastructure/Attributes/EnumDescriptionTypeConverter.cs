using System;
using System.ComponentModel;
using System.Globalization;

namespace LangAppApi.Infrastructure.Attributes
{
    public class EnumDescriptionTypeConverter : EnumConverter
    {
        public EnumDescriptionTypeConverter(Type type) : base(type)
        {
        }

        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            if (destinationType != typeof(string)) return base.ConvertTo(context, culture, value, destinationType);
            if (value == null) return base.ConvertTo(context, culture, value, destinationType);
            var fi = value.GetType().GetField(value.ToString() ?? string.Empty);

            if (fi != null)
            {
                return fi.GetCustomAttributes(typeof(DescriptionAttribute), false) is DescriptionAttribute[] attributes && (attributes.Length > 0 && !string.IsNullOrEmpty(attributes[0].Description))
                    ? attributes[0].Description
                    : value.ToString();
            }

            return base.ConvertTo(context, culture, value, destinationType);
        }
    }
}