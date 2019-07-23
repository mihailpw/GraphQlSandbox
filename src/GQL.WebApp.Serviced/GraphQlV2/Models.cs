using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GQL.WebApp.Serviced.GraphQlV2
{
    public class QueryRootService
    {
        [ReturnType(typeof(IInnerService))]
        public InnerModel GetAsync(int? id)
        {
            return new InnerModel
            {
                IntProperty = 22,
                StringProperty = "344",
            };
        }

        [ReturnType(typeof(IInnerService))]
        public IEnumerable<InnerModel> GetMany(int? id)
        {
            yield return new InnerModel
            {
                IntProperty = 22,
                StringProperty = "344",
            };
            yield return new InnerModel
            {
                IntProperty = 22322,
                StringProperty = "34334",
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