using System.Threading.Tasks;
using GQL.WebApp.Typed.GraphQl;
using GQL.WebApp.Typed.GraphQl.Schemas;
using GraphQL;
using Microsoft.AspNetCore.Mvc;

namespace GQL.WebApp.Typed.Controllers
{
    [Route("graphql")]
    public class GraphQlController : ControllerBase
    {
        private readonly IDocumentExecuter _documentExecuter;
        private readonly AppSchema _appSchema;


        public GraphQlController(IDocumentExecuter documentExecuter, AppSchema appSchema)
        {
            _documentExecuter = documentExecuter;
            _appSchema = appSchema;
        }


        [HttpPost]
        public async Task<IActionResult> Post([FromBody] GraphQlQueryFormModel model)
        {
            var options = new ExecutionOptions
            {
                Schema = _appSchema,
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