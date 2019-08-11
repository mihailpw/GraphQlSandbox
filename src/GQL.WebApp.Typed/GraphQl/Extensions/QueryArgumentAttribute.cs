using System;
using GraphQL;

namespace GQL.WebApp.Typed.GraphQl.Extensions
{
    public class QueryArgumentAttribute : Attribute
    {
        public string Name { get; }

        public Type Type { get; }


        public QueryArgumentAttribute(string name = null, Type type = null)
            : this(type)
        {
            Name = name;
        }

        public QueryArgumentAttribute(Type type)
        {
            Type = type;

            if (type != null && !type.IsGraphType())
            {
                throw new InvalidOperationException($"Type {type.Name} is not graph type");
            }
        }
    }
}