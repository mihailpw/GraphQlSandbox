using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GraphQL.Client;
using GraphQL.Common.Request;
using Newtonsoft.Json.Linq;

namespace GQL.Client.QueryBuilders.Infra
{
    public class RootObjectBuilderBase<TDto> : ObjectBuilderBase, IClient<TDto>
    {
        private readonly GraphQLClient _client;


        protected RootObjectBuilderBase(string url, string key)
            : base(key)
        {
            _client = new GraphQLClient(url);
        }


        public override void ThrowIfNotValid()
        {
        }

        public async Task<GraphQlResponse<TDto>> SendAsync()
        {
            var data = EnumerateArguments().ToList();

            var query = Build();

            var arguments = data
                .ToDictionary(ma => ma.ArgumentName, ma => ma.Value);

            var request = new GraphQLRequest
            {
                Query = query,
                Variables = arguments,
            };
            var response = await _client.PostAsync(request);

            if (response.Errors != null && response.Errors.Length > 0)
            {
                var errors = response.Errors.Select(
                        e => new GraphQlError(
                            e.Message,
                            e.Locations?.Select(l => new GraphQlError.Location(l.Column, l.Line)).ToList(),
                            e.AdditonalEntries?.ToDictionary(p => p.Key, p => (object)p.Value)))
                    .ToList();

                return new GraphQlResponse<TDto>(errors);
            }

            var jData = (JToken)response.Data;
            var dto = jData.ToObject<TDto>();

            return new GraphQlResponse<TDto>(dto);
        }


        protected override void BuildArguments(StringBuilder builder)
        {
            var arguments = EnumerateArguments().ToList();

            if (arguments.Count == 0)
            {
                return;
            }

            builder.Append("(");
            foreach (var argument in arguments)
            {
                builder.Append($"${argument.ArgumentName}:{argument.FieldTypeName},");
            }
            builder.Length--;
            builder.Append(")");
        }
    }
}