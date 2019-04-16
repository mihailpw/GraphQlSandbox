using System.Collections.Generic;
using GraphQL.Types;

namespace GQL.WebApp.Typed.GraphQl.Infra
{
    public sealed class CompositeObjectGraphType : ObjectGraphType
    {
        public CompositeObjectGraphType(IEnumerable<ObjectGraphType> objectGraphTypes)
        {
            foreach (var objectGraphType in objectGraphTypes)
            {
                foreach (var resolvedInterface in objectGraphType.ResolvedInterfaces)
                {
                    AddResolvedInterface(resolvedInterface);
                }

                foreach (var @interface in objectGraphType.Interfaces)
                {
                    Interface(@interface);
                }

                foreach (var field in objectGraphType.Fields)
                {
                    AddField(field);
                }
            }
        }
    }
}