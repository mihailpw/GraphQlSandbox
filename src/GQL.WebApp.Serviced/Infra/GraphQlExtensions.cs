using System;
using System.Collections.Generic;
using System.Linq;
using GraphQL.Language.AST;

namespace GQL.WebApp.Serviced.Infra
{
    public static class GraphQlExtensions
    {
        public static bool TryFindField(this IEnumerable<INode> fields, string name, out Field field)
        {
            field = fields
                .OfType<Field>()
                .FirstOrDefault(f => f.Name.Equals(name, StringComparison.InvariantCultureIgnoreCase));

            return field != null;
        }
    }
}