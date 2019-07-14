using System;
using GQL.WebApp.Serviced.GraphQl.Infra.Providers;
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
    public class GraphQlObjectAttribute : Attribute, INameProvider, IDescriptionProvider, IInterfaceHolder, IDeprecationReasonProvider
    {
        public string Name { get; }
        public string Description { get; set; }
        public bool IsInterface { get; set; }
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
    public class GraphQlIdAttribute : Attribute, ITypeHolder
    {
        public Type Type => typeof(IdGraphType);
    }
}