//using System;
//using System.Collections.Generic;
//using System.Linq;

//namespace GQL.Services.Infra.FieldResolvers.Mapping
//{
//    public class ManyObjectMapper : ObjectMapperBase
//    {
//        public ManyObjectMapper(Type targetType, Type sourceType)
//            : base(targetType.GetEnumerableElementType(), sourceType.GetEnumerableElementType())
//        {
//        }


//        public override object Populate(object targetType, object sourceType)
//        {
//            var targetTypes = (IEnumerable<object>) targetType;
//            var sourceTypes = ((IEnumerable<object>) sourceType).ToList();

//            var result = targetTypes.Select((t, i) => PopulateInternal(t, sourceTypes[i]));

//            return result;
//        }
//    }
//}