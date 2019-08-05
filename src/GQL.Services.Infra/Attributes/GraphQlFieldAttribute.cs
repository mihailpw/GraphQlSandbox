using System;
using System.Reflection;
using GQL.Services.Infra.Providers;
using GraphQL.Types;

namespace GQL.Services.Infra.Attributes
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Method)]
    public class GraphQlFieldAttribute : GraphQlAttribute, INameProvider, IFieldTypeInfoProvider
    {
        public string Name { get; }
        public string Description { get; set; }
        public string DeprecationReason { get; set; }
        public object DefaultValue { get; set; }


        public GraphQlFieldAttribute(string name = null)
        {
            Name = name;
        }


        public void Provide(FieldType fieldType, PropertyInfo propertyInfo, IServiceProvider serviceProvider)
        {
            fieldType.Name = Name ?? propertyInfo.Name;
            fieldType.Description = Description;
            fieldType.DeprecationReason = DeprecationReason;
            if (DefaultValue != null)
            {
                fieldType.DefaultValue = DefaultValue;
            }
        }

        public void Provide(FieldType fieldType, MethodInfo methodInfo, IServiceProvider serviceProvider)
        {
            fieldType.Name = Name ?? methodInfo.Name;
            fieldType.Description = Description;
            fieldType.DeprecationReason = DeprecationReason;
            if (DefaultValue != null)
            {
                fieldType.DefaultValue = DefaultValue;
            }
        }
    }
}