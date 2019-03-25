using Newtonsoft.Json.Linq;

namespace GQL.WebApp.Typed.GraphQl
{
    public class GraphQlQueryFormModel
    {
        public string OperationName { get; set; }

        public string NamedQuery { get; set; }

        public string Query { get; set; }

        public JObject Variables { get; set; }
    }
}