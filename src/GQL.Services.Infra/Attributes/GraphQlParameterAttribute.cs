using System;
using System.Reflection;
using GQL.Services.Infra.Providers;
using GraphQL.Types;

namespace GQL.Services.Infra.Attributes
{
    [AttributeUsage(AttributeTargets.Parameter)]
    public class GraphQlParameterAttribute : GraphQlAttribute, INameProvider, IQueryArgumentInfoProvider
    {
        public string Name { get; }
        public string Description { get; set; }
        public object DefaultValue { get; set; }


        public GraphQlParameterAttribute(string name = null)
        {
            Name = name;
        }


        public void Provide(QueryArgument queryArgument, ParameterInfo parameterInfo, IServiceProvider serviceProvider)
        {
            queryArgument.Name = Name ?? parameterInfo.Name;
            queryArgument.Description = Description;
            if (DefaultValue != null)
            {
                queryArgument.DefaultValue = DefaultValue;
            }
        }
    }
}