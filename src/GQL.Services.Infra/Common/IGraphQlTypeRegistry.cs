using System;
using System.Collections.Generic;

namespace GQL.Services.Infra.Common
{
    internal interface IGraphQlTypeRegistry
    {
        bool IsRegistered(Type type);
        Type Resolve(Type type);
        bool TryResolve(Type type, out Type graphQlType);
        IEnumerable<Type> ResolveAdditional(Type type);
        IEnumerable<Type> ResolveAll();

        Type RegisterInputObject(Type type);
        Type RegisterObject(Type type);
        Type RegisterInterface(Type type);
        void DirectRegister(Type type, Type graphQlType);
    }
}