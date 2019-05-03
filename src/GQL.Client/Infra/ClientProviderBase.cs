using System.Linq;
using System.Text;

namespace GQL.Client.Infra
{
    public abstract class ClientProviderBase
    {
        private readonly string _url;
        private readonly bool _usePostRequest;

        protected ClientProviderBase(string url, bool usePostRequest)
        {
            _url = url;
            _usePostRequest = usePostRequest;
        }


        protected IClient<TDto> CreateClient<TDto>(string requestType, TypeBase type)
        {
            var rootType = new RootType(requestType, type);

            var stringBuilder = new StringBuilder();
            rootType.AppendQuery(stringBuilder);
            var query = stringBuilder.ToString();

            var variables = rootType.GetArguments().ToDictionary(a => a.ArgumentName, a => a.Value);

            return new Client<TDto>(_url, query, variables, _usePostRequest);
        }
    }
}