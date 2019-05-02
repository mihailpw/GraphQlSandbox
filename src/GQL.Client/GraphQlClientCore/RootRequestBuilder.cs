using System.Collections.Generic;
using System.Linq;

namespace GQL.Client.GraphQlClientCore
{
    public class RootRequestBuilder<TDto, TType> : RequestBuilder<TType>
    {
        private readonly string _url;
        private readonly bool _useGetResponse;


        public RootRequestBuilder(string url, bool useGetResponse, string key) : base(key)
        {
            _url = url;
            _useGetResponse = useGetResponse;
        }


        public IClient<TDto> Build()
        {
            var query = GenerateQuery();
            var arguments = GetArguments().ToDictionary(a => a.ArgumentName, a => a.Value);

            return new Client<TDto>(_url, query, arguments, _useGetResponse);
        }


        protected override IEnumerable<(string key, string value)> ResolveArguments()
        {
            return GetArguments()
                .Select(a => ($"${a.ArgumentName}", FieldTypeName: a.Type));
        }
    }
}