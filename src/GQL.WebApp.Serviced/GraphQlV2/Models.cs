using System.Collections.Generic;

namespace GQL.WebApp.Serviced.GraphQlV2
{
    public class QueryRootService
    {
        [ReturnType(typeof(IInnerService))]
        public object GetMany(int? id)
        {
            return new InnerModel
            {
                IntProperty = 22,
                StringProperty = "344",
            };
        }
    }

    public class InnerModel
    {
        public string StringProperty { get; set; }
        public int IntProperty { get; set; }
    }

    public interface IInnerService
    {
        string StringProperty { get; set; }
        int IntProperty { get; set; }
    }

    public class InnerService : IInnerService
    {
        public string StringProperty { get; set; }
        public int IntProperty { get; set; }
    }
}