using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GraphQL.Client;
using GraphQL.Common.Request;
using Newtonsoft.Json.Linq;

namespace GQL.Client.QueryBuilders.Infra
{
    public abstract class RootObjectBuilderBase<TDto> : ObjectBuilderBase, IClient<TDto>
    {
        private readonly string _key;

        private readonly GraphQLClient _client;


        protected RootObjectBuilderBase(string url, string key)
        {
            _key = key;

            _client = new GraphQLClient(url);
        }


        public override void ThrowIfNotValid()
        {
        }

        public sealed override string Build()
        {
            var stringBuilder = new StringBuilder(_key);

            var arguments = EnumerateArguments().ToList();

            if (arguments.Count > 0)
            {
                stringBuilder.Append("(");
                foreach (var argument in arguments)
                {
                    stringBuilder.Append($"${argument.ArgumentName}:{argument.FieldTypeName},");
                }
                stringBuilder.Length--;
                stringBuilder.Append(")");
            }

            stringBuilder.Append("{");
            foreach (var include in Includes)
            {
                stringBuilder.Append(include.Build());
                stringBuilder.Append(" ");
            }
            stringBuilder.Append("}");

            return stringBuilder.ToString();
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
    }
}