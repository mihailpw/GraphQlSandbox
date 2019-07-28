using System;

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
    public class GraphQlTypeAttribute : GraphQlAttribute, INameProvider, IDescriptionProvider, IDeprecationReasonProvider
    {
        public string Name { get; }
        public string Description { get; set; }
        public string DeprecationReason { get; set; }

        public GraphQlTypeAttribute(string name = null)
        {
            Name = name;
        }
    }

    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Method)]
    public class GraphQlFieldAttribute : GraphQlAttribute, INameProvider, IDescriptionProvider, IDeprecationReasonProvider, IReturnTypeProvider, IRequiredProvider
    {
        public string Name { get; }
        public string Description { get; set; }
        public string DeprecationReason { get; set; }
        public Type ReturnType { get; }
        public bool IsRequired { get; set; }

        public GraphQlFieldAttribute(string name = null, Type returnType = null)
        {
            Name = name;
            ReturnType = returnType;
        }
    }

    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Method)]
    public class GraphQlIdFieldAttribute : GraphQlFieldAttribute
    {
        public GraphQlIdFieldAttribute(string name = null)
            : base(name, typeof(IdObject))
        {
        }
    }
}