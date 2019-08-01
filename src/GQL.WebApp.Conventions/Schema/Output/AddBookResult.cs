using GQL.WebApp.Conventions.Schema.Types;
using GraphQL.Conventions;
using GraphQL.Conventions.Relay;

namespace GQL.WebApp.Conventions.Schema.Output
{
    [Description("The result of an add-book operation.")]
    public class AddBookResult : IRelayMutationOutputObject
    {
        public string ClientMutationId { get; set; }

        [Description("The book added.")]
        public Book Book { get; set; }
    }
}
