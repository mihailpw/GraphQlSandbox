using System;

namespace GQL.Services.Infra
{
    public interface INameProvider
    {
        string Name { get; }
    }

    public interface IReturnTypeProvider
    {
        Type ReturnType { get; }
    }

    public interface IDescriptionProvider
    {
        string Description { get; }
    }

    public interface IDeprecationReasonProvider
    {
        string DeprecationReason { get; }
    }
}