namespace GQL.Services.Infra.Common
{
    internal interface IConfig
    {
        bool ThrowIfPropertyNotFound { get; }
        bool ThrowIfPropertiesTypesDifferent { get; }
    }
}