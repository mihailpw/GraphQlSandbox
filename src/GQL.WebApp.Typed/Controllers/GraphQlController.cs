using System.Threading.Tasks;
using GQL.WebApp.Typed.GraphQl;
using GQL.WebApp.Typed.GraphQl.Query;
using GraphQL;
using GraphQL.Types;
using Microsoft.AspNetCore.Mvc;

namespace GQL.WebApp.Typed.Controllers
{
    [Route("graphql")]
    public class GraphQlController : ControllerBase
    {
        private readonly IDocumentExecuter _documentExecuter;
        private readonly DefaultQuery _defaultQuery;


        public GraphQlController(IDocumentExecuter documentExecuter, DefaultQuery defaultQuery)
        {
            _documentExecuter = documentExecuter;
            _defaultQuery = defaultQuery;
        }


        [HttpPost]
        public async Task<IActionResult> Post([FromBody] GraphQlQueryFormModel model)
        {
            var options = new ExecutionOptions
            {
                Schema = new Schema
                {
                    Query = _defaultQuery,
                },
                Query = model.Query,
                OperationName = model.OperationName,
                Inputs = model.Variables.ToInputs(),
                CancellationToken = HttpContext.RequestAborted,
#if DEBUG
                EnableMetrics = true,
                ExposeExceptions = true,
#endif
            };

            var result = await _documentExecuter.ExecuteAsync(options);

            return Ok(result);
        }
    }
}