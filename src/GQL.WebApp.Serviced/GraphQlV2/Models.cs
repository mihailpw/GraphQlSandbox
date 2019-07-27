using System.Collections.Generic;
using System.Threading.Tasks;
using GQL.Services.Infra;

namespace GQL.WebApp.Serviced.GraphQlV2
{
    public class InputObject
    {
        [GraphQlField(IsRequired = true)]
        public int? Int { get; set; }
    }

    public class QueryRootService
    {
        [GraphQlField("get", typeof(InnerService))]
        public async Task<object> GetAsync(
            [GraphQlParameter(IsRequired = true)] int? id,
            [GraphQlParameter(IsRequired = true)] InputObject input)
        {
            await Task.CompletedTask;
            return new
            {
                IntProperty = 22,
                StringProperty = "344",
            };
        }

        [GraphQlField("getall", typeof(InnerService))]
        public IEnumerable<object> GetMany()
        {
            yield return new
            {
                IntProperty = 22,
                StringProperty = "k44jk",
            };
            yield return new
            {
                IntProperty = 22322,
                StringProperty = "hghgh",
            };
        }
    }

    public class InnerService
    {
        [GraphQlField]
        public string StringProperty { get; set; }
        [GraphQlField]
        public int IntProperty { get; set; }

        [GraphQlField("getasync", typeof(Inner2Service))]
        public async Task<object> GetAsync()
        {
            await Task.CompletedTask;
            return new
            {
                LongProperty = (long) 22,
            };
        }
    }

    public class Inner2Service
    {
        [GraphQlField]
        public long LongProperty { get; set; }
    }
}