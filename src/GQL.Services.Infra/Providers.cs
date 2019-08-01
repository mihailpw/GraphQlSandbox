using System;
using System.Reflection;
using GraphQL.Types;

namespace GQL.Services.Infra
{
    public interface IGraphTypeInfoProvider
    {
        void Provide(GraphType graphType, Type type);
    }

    public interface IFieldTypeInfoProvider
    {
        void Provide(FieldType fieldType, PropertyInfo propertyInfo);
        void Provide(FieldType fieldType, MethodInfo methodInfo);
    }
}