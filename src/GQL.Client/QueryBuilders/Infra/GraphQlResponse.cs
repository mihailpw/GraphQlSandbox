using System;
using System.Collections.Generic;

namespace GQL.Client.QueryBuilders.Infra
{
    public class GraphQlResponse<T>
    {
        private readonly T _data;


        public bool IsCompleted { get; }

        public bool IsFailed => !IsCompleted;

        public T Data
        {
            get
            {
                if (IsFailed)
                {
                    throw new InvalidOperationException("Can not get data from failed response.");
                }

                return _data;
            }
        }

        public IReadOnlyList<GraphQlError> Errors { get; }


        public GraphQlResponse(T data)
        {
            _data = data;

            IsCompleted = true;
            Errors = Array.Empty<GraphQlError>();
        }

        public GraphQlResponse(IReadOnlyList<GraphQlError> errors)
        {
            Errors = errors;

            IsCompleted = false;
        }
    }
}