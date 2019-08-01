using GQL.WebApp.Conventions.Schema.Types;
using GraphQL.Conventions;
using GraphQL.Conventions.Relay;

namespace GQL.WebApp.Conventions.Schema.Output
{
    [Description("The result of an add-author operation.")]
    public class AddAuthorResult : IRelayMutationOutputObject
    {
        public string ClientMutationId { get; set; }

        [Description("The author added.")]
        public Author Author { get; set; }
    }
}
