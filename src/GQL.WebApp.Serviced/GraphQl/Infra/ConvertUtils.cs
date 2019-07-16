using System;
using System.ComponentModel;
using GraphQL.Types;

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

            // ReSharper disable once UseMethodIsInstanceOfType
            return underlyingType.IsAssignableFrom(value.GetType())
                ? Convert.ChangeType(value, underlyingType)
                : Convert.ChangeType(value.ToString(), underlyingType);
        }

        public static object ChangeResolveFieldContextTypeTo(ResolveFieldContext context, Type sourceType)
        {
            return Activator.CreateInstance(typeof(ResolveFieldContext<>).MakeGenericType(sourceType), context);
        }
    }
}