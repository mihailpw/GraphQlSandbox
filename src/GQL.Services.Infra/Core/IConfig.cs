namespace GQL.Services.Infra.Core
{
    internal interface IConfig
    {
        bool ThrowIfPropertyNotFound { get; }
        bool ThrowIfPropertiesTypesDifferent { get; }
    }
}