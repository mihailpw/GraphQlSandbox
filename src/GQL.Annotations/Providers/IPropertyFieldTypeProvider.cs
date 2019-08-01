using System;
using System.Reflection;
using GraphQL.Types;

namespace GQL.Annotations.Providers
{
    public interface IPropertyFieldTypeProvider
    {
        bool TryGetFieldDescription(PropertyInfo propertyInfo, IServiceProvider serviceProvider, out FieldType fieldType);
    }
}