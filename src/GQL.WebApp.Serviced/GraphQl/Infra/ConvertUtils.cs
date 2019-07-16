using System;
using System.ComponentModel;

namespace GQL.WebApp.Serviced.GraphQl.Infra
{
    public class ConvertUtils
    {
        public static object ChangeTypeTo(object value, Type toType)
        {
            if (value == null)
            {
                return null;
            }

            var underlyingType = toType;

            if (underlyingType.CheckIfNullable())
            {
                var converter = new NullableConverter(underlyingType);
                underlyingType = converter.UnderlyingType;
            }

            if (underlyingType == typeof(Guid))
            {
                return new Guid(value.ToString());
            }

            return underlyingType.IsInstanceOfType(value)
                ? Convert.ChangeType(value, underlyingType)
                : Convert.ChangeType(value.ToString(), underlyingType);
        }

    }
}