using System;
using GQL.Services.Infra;
using GraphQL.Types;

namespace GQL.WebApp.Serviced.GraphQl.Infra
{
    [AttributeUsage(AttributeTargets.Parameter)]
    public class GraphQlParameterAttribute : Attribute, INameProvider, IDescriptionProvider
    {
        public string Name { get; set; }
        public string Description { get; set; }
    }

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface)]
    public class GraphQlObjectAttribute : Attribute, INameProvider, IDescriptionProvider, IDeprecationReasonProvider
    {
        public string Name { get; }
        public string Description { get; set; }
        public string DeprecationReason { get; set; }

        public GraphQlObjectAttribute(string name = null)
        {
            Name = name;
        }
    }

    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Method)]
    public class GraphQlFieldAttribute : Attribute, INameProvider, IDescriptionProvider, IDeprecationReasonProvider
    {
        public string Name { get; }
        public string Description { get; set; }
        public string DeprecationReason { get; set; }

        public GraphQlFieldAttribute(string name = null)
        {
            Name = name;
        }
    }

    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Parameter)]
    public class GraphQlIdAttribute : Attribute, IReturnTypeProvider
    {
        public Type ReturnType => typeof(IdGraphType);
    }
}