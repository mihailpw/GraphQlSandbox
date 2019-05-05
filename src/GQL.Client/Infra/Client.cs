﻿using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GraphQL.Client;
using GraphQL.Common.Request;
using GraphQL.Common.Response;
using Newtonsoft.Json.Linq;

namespace GQL.Client.Infra
{
    public interface IClient
    {
        Task<Response<TDto>> RequestAsync<TDto>();
    }

    public interface IClient<T> : IClient
    {
        Task<Response<T>> RequestAsync();
    }

    public class Client : IClient
    {
        private readonly bool _usePostResponse;

        private readonly GraphQLClient _client;
        private readonly GraphQLRequest _request;


        public Client(string url, string query, Dictionary<string, object> variables, bool usePostResponse)
        {
            _usePostResponse = usePostResponse;

            _client = new GraphQLClient(url);
            _request = new GraphQLRequest
            {
                Query = query,
                Variables = variables,
            };
        }


        public async Task<Response<TDto>> RequestAsync<TDto>()
        {
            var graphQlResponse = await SendAsync();
            var response = ProcessResponse<TDto>(graphQlResponse);

            return response;
        }


        private async Task<GraphQLResponse> SendAsync()
        {
            return _usePostResponse
                ? await _client.PostAsync(_request)
                : await _client.GetAsync(_request);
        }

        private static Response<TDto> ProcessResponse<TDto>(GraphQLResponse graphQlResponse)
        {
            if (graphQlResponse.Errors != null && graphQlResponse.Errors.Length > 0)
            {
                var errors = graphQlResponse.Errors.Select(
                        e => new Error(
                            e.Message,
                            e.Locations?.Select(l => new Error.Location(l.Column, l.Line)).ToList(),
                            e.AdditonalEntries?.ToDictionary(p => p.Key, p => (object)p.Value)))
                    .ToList();

                return new Response<TDto>(errors);
            }

            var jData = (JToken)graphQlResponse.Data;
            var dto = jData.ToObject<TDto>();

            return new Response<TDto>(dto);
        }
    }

    public class Client<T> : Client, IClient<T>
    {
        public Client(
            string url,
            string query,
            Dictionary<string, object> variables,
            bool usePostResponse)
            : base(url, query, variables, usePostResponse)
        {
        }


        public async Task<Response<T>> RequestAsync()
        {
            return await RequestAsync<T>();
        }
    }
}