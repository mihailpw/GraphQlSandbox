using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GQL.WebApp.Conventions.Core;
using GQL.WebApp.Conventions.Schema.Types;
using GraphQL.Conventions;
using GraphQL.Conventions.Relay;

namespace GQL.WebApp.Conventions.Schema
{
    [ImplementViewer(OperationType.Query)]
    public class Query
    {
        [Description("Retrieve book/author by its globally unique ID.")]
        public Task<INode> Node(UserContext context, Id id) =>
            context.Get<INode>(id);

        [Description("Retrieve book by its globally unique ID.")]
        public Task<Book> Book(UserContext context, Id id) =>
            context.Get<Book>(id);

        [Description("Retrieve books by their globally unique IDs.")]
        public IEnumerable<Task<Book>> Books(UserContext context, IEnumerable<Id> ids) =>
            ids.Select(context.Get<Book>);

        [Description("Retrieve author by his/her globally unique ID.")]
        public Task<Author> Author(UserContext context, Id id) =>
            context.Get<Author>(id);

        [Description("Retrieve authors by their globally unique IDs.")]
        public IEnumerable<Task<Author>> Authors(UserContext context, IEnumerable<Id> ids) =>
            ids.Select(context.Get<Author>);

        [Description("Search for books and authors.")]
        public Connection<SearchResult> Search(
            UserContext context,
            [Description("Title or last name.")] NonNull<string> forString,
            [Description("Only return search results after given cursor.")] Cursor? after,
            [Description("Return the first N results.")] int? first)
        {
            return context
                .Search(forString.Value)
                .Select(node => new SearchResult { Instance = node })
                .ToConnection(first ?? 5, after);
        }
    }
}
