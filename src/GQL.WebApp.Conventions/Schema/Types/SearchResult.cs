using GraphQL.Conventions;

namespace GQL.WebApp.Conventions.Schema.Types
{
    [Description("A search result")]
    public class SearchResult : Union<Author, Book>
    {
    }
}
