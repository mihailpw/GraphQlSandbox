using System;
using System.Reflection;
using GQL.Services.Infra.Helpers;
using GraphQL.Types;

namespace GQL.Services.Infra
{
    public abstract class GraphQlAttribute : Attribute
    {
    }

    [AttributeUsage(AttributeTargets.Parameter)]
    public class GraphQlParameterAttribute : GraphQlAttribute, INameProvider, IDescriptionProvider, IRequiredProvider
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsRequired { get; set; }
    }

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface)]
    public class GraphQlTypeAttribute : GraphQlAttribute, IGraphTypeInfoProvider
    {
        public string Name { get; }
        public string Description { get; set; }
        public string DeprecationReason { get; set; }

        public GraphQlTypeAttribute(string name = null)
        {
            Name = name;
        }

        public void Provide(GraphType graphType, Type type)
        {
            graphType.Name = Name ?? type.Name;
            graphType.Description = Description;
            graphType.DeprecationReason = DeprecationReason;
        }
    }

    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Method)]
    public class GraphQlFieldAttribute : GraphQlAttribute, IFieldTypeInfoProvider
    {
        public string Name { get; }
        public string Description { get; set; }
        public string DeprecationReason { get; set; }
        public Type ReturnType { get; }


        public GraphQlFieldAttribute(string name = null, Type returnType = null)
        {
            Name = name;
            ReturnType = returnType;
        }


        public void Provide(FieldType fieldType, PropertyInfo propertyInfo)
        {
            fieldType.Name = Name ?? propertyInfo.Name;
            fieldType.Description = Description;
            fieldType.DeprecationReason = DeprecationReason;
            fieldType.Type = GraphQlUtils.GetGraphQlTypeFor(ReturnType);
        }

        public void Provide(FieldType fieldType, MethodInfo methodInfo)
        {
            throw new NotImplementedException();
        }
    }
}