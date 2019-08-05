using System;
using GQL.Services.Infra.Providers;
using GraphQL.Types;

namespace GQL.Services.Infra.Attributes
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface)]
    public class GraphQlTypeAttribute : GraphQlAttribute, INameProvider, IGraphTypeInfoProvider
    {
        public string Name { get; }
        public string Description { get; set; }
        public string DeprecationReason { get; set; }


        public GraphQlTypeAttribute(string name = null)
        {
            Name = name;
        }


        public void Provide(GraphType graphType, Type type, IServiceProvider serviceProvider)
        {
            graphType.Name = Name ?? type.Name;
            graphType.Description = Description;
            graphType.DeprecationReason = DeprecationReason;
        }
    }
}