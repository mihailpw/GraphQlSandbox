using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GraphQL.Client;
using GraphQL.Common.Request;
using Newtonsoft.Json.Linq;

namespace GQL.Client.QueryBuilders.Infra
{
    public abstract class GraphQlClientBase<TDto> : IDisposable
    {
        private readonly GraphQlRequestType _requestType;

        private readonly GraphQLClient _client;
        private readonly List<QueryBuilderBase> _queryBuilders;


        protected GraphQlClientBase(string url, GraphQlRequestType requestType)
        {
            _requestType = requestType;

            _client = new GraphQLClient(url);
            _queryBuilders = new List<QueryBuilderBase>();
        }


        public async Task<GraphQlResponse<TDto>> SendAsync()
        {
            var data = _queryBuilders
                .OfType<ObjectQueryBuilderBase>()
                .SelectMany(obb => obb.Arguments)
                .ToList();

            var query = GenerateQuery(data);

            var arguments = data
                .ToDictionary(ma => ma.FieldName, ma => ma.Value);

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
                            e.Locations.Select(l => new GraphQlError.Location(l.Column, l.Line)).ToList(),
                            e.AdditonalEntries.ToDictionary(p => p.Key, p => (object) p.Value)))
                    .ToList();

                return new GraphQlResponse<TDto>(errors);
            }

            var jData = (JToken) response.Data;
            var dto = jData.ToObject<TDto>();

            return new GraphQlResponse<TDto>(dto);
        }

        public void Dispose()
        {
            _client.Dispose();
        }


        protected void AddQueryBuilder(QueryBuilderBase queryBuilder)
        {
            queryBuilder.ThrowIfNotValid();
            _queryBuilders.Add(queryBuilder);
        }


        private string GenerateQuery(IReadOnlyCollection<ObjectQueryBuilderBase.FieldData> data)
        {
            var stringBuilder = new StringBuilder();

            stringBuilder.Append(Utils.ConvertRequestTypeToString(_requestType));

            if (data.Count > 0)
            {
                stringBuilder.Append("(");
                foreach (var fieldData in data)
                {
                    stringBuilder.Append($"${fieldData.FieldName}: {fieldData.FieldTypeName}");
                }
                stringBuilder.Append(")");
            }

            stringBuilder.Append("{");
            foreach (var queryBuilder in _queryBuilders)
            {
                var query = queryBuilder.Build();
                stringBuilder.Append(query);
            }
            stringBuilder.Append("}");

            return stringBuilder.ToString();
        }
    }
}