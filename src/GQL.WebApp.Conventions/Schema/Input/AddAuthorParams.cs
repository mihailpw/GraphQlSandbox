using GraphQL.Conventions;
using GraphQL.Conventions.Relay;

namespace GQL.WebApp.Conventions.Schema.Input
{
    [Description("Operation for adding a new author.")]
    public class AddAuthorParams : IRelayMutationInputObject
    {
        public string ClientMutationId { get; set; }

        [Description("The author to add.")]
        public NonNull<AuthorInput> Author { get; set; }
    }
}
