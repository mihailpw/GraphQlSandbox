using System;
using System.Reflection;
using GraphQL.Types;

namespace GQL.Annotations.Providers
{
    public interface IMethodFieldTypeProvider
    {
        bool TryGetFieldDescription(MethodInfo methodInfo, IServiceProvider serviceProvider, out FieldType fieldType);
    }
}