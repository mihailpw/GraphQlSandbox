using System.IO;
using System.Threading.Tasks;
using GQL.WebApp.Conventions.Core;
using GQL.WebApp.Conventions.Data.Repositories;
using GQL.WebApp.Conventions.Schema;
using GraphQL.Conventions;
using Microsoft.AspNetCore.Mvc;
using Query = GQL.WebApp.Conventions.Schema.Query;

namespace GQL.WebApp.Conventions.Controllers
{
    [Route("graphql")]
    [ApiController]
    public class GraphQlController : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> Get()
        {
            using var streamReader = new StreamReader(HttpContext.Request.Body);
            var requestBody = streamReader.ReadToEnd();

            var dependencyInjector = new DependencyInjector();
            dependencyInjector.Register(new Query());
            dependencyInjector.Register<IBookRepository>(new BookRepository());
            dependencyInjector.Register<IAuthorRepository>(new AuthorRepository());

            var engine = GraphQLEngine.New<Query, Mutation>();
            var result = await engine
                .NewExecutor()
                .WithDependencyInjector(dependencyInjector)
                .WithUserContext(new UserContext())
                .WithRequest(requestBody)
                .Execute();

            return Ok(result);
        }
    }
}