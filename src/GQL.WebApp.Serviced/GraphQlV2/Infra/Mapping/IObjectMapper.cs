namespace GQL.WebApp.Serviced.GraphQlV2.Infra.Mapping
{
    public interface IObjectMapper
    {
        object Populate(object targetType, object sourceType);
    }
}