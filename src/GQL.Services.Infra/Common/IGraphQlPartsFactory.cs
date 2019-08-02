using System;
using System.Reflection;
using GraphQL.Resolvers;
using GraphQL.Types;

namespace GQL.Services.Infra.Common
{
    internal interface IGraphQlPartsFactory
    {
        FieldType CreateFieldType(PropertyInfo propertyInfo, IFieldResolver fieldResolver = null);
        FieldType CreateFieldType(MethodInfo methodInfo, IFieldResolver fieldResolver = null);
        Type CreateInterfaceType(Type interfaceType);
        Func<object, bool> CreateIsTypeOfFunc(Type type);
    }
}