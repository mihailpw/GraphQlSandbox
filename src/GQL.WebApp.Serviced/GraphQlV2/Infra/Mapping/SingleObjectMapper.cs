using System.Reflection;

namespace GQL.WebApp.Serviced.GraphQlV2.Infra.Mapping
{
    public class SingleObjectMapper : ObjectMapperBase
    {
        public SingleObjectMapper(IReflect targetType, IReflect sourceType)
            : base(targetType, sourceType)
        {
        }


        public override object Populate(object targetType, object sourceType)
        {
            return PopulateInternal(targetType, sourceType);
        }
    }
}