using System.Reflection;

namespace GQL.Services.Infra.FieldResolvers.Mapping
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