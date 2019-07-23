namespace GQL.Services.Infra.FieldResolvers.Mapping
{
    public interface IObjectMapper
    {
        object Populate(object targetType, object sourceType);
    }
}