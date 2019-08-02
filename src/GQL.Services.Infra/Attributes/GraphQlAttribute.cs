using System;

namespace GQL.Services.Infra.Attributes
{
    [AttributeUsage(
        AttributeTargets.Class
        | AttributeTargets.Interface
        | AttributeTargets.Method
        | AttributeTargets.Property
        | AttributeTargets.Parameter
        | AttributeTargets.Enum)]
    public abstract class GraphQlAttribute : Attribute
    {
    }
}