using System.Linq;
using GraphQL.Types;

namespace GQL.Services.Infra.Common.Schemas
{
    internal abstract class SchemaBase : Schema
    {
        private readonly IGraphQlTypeRegistry _typeRegistry;


        protected SchemaBase(IGraphQlTypeRegistry typeRegistry)
        {
            _typeRegistry = typeRegistry;
        }


        protected void PopulateAdditionalTypes()
        {
            var registerTypeMethodInfo = GetType().GetMethods().First(mi => mi.Name == nameof(RegisterType) && mi.IsGenericMethod);
            foreach (var graphQlType in _typeRegistry.ResolveAll())
            {
                if (!AllTypes.Any(it => graphQlType.IsInstanceOfType(it)))
                {
                    registerTypeMethodInfo.MakeGenericMethod(graphQlType).Invoke(this, new object[0]);
                }
            }
        }
    }
}