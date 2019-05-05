using System.Collections.Generic;
using System.Linq;
using System.Text;
using GQL.Client.Infra;

namespace GQL.Client.InfraV2
{
    public class ClientFactory<TDto>
    {
        private readonly string _url;
        private readonly string _requestType;
        private readonly TypeBase _type;


        public ClientFactory(string url, string requestType, TypeBase type)
        {
            _url = url;
            _requestType = requestType;
            _type = type;
        }


        public IClient<TDto> CreateClient()
        {
            var arguments = ((IArgumentsProvider) _type).GetArguments().ToList();
            var query = BuildQuery(arguments);
            var variables = arguments.ToDictionary(a => a.Id, a => a.Value);

            return new Client<TDto>(_url, query, variables, usePostResponse: true);
        }


        private string BuildQuery(List<Argument> arguments)
        {
            var stringBuilder = new StringBuilder(_requestType);

            if (arguments.Count > 0)
            {
                stringBuilder.Append("(");
                foreach (var argument in arguments)
                {
                    stringBuilder.Append($"${argument.Id}:{argument.Type},");
                }
                stringBuilder.Length--;
                stringBuilder.Append(")");
            }

            stringBuilder.Append("{");
            ((IRequestBuilder) _type).AppendRequest(stringBuilder);
            stringBuilder.Append("}");

            return stringBuilder.ToString();
        }
    }
}