using System;
using System.Reflection;
using GQL.Annotations.Providers;
using GraphQL.Types;

namespace GQL.Annotations.Attributes
{
    public class GraphQlInputTypeAttribute : GraphQlAttribute, IPropertyFieldTypeProvider
    {
        public bool TryGetFieldDescription(PropertyInfo propertyInfo, IServiceProvider serviceProvider, out FieldType fieldType)
        {
            if (propertyInfo.)
            {
                
            }
        }
    }
}