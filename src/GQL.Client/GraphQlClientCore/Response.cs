using System;
using System.Collections.Generic;
using GQL.Client.QueryBuilders.Infra;

namespace GQL.Client.GraphQlClientCore
{
    public class Response<T>
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


        public Response(T data)
        {
            _data = data;

            IsCompleted = true;
            Errors = Array.Empty<GraphQlError>();
        }

        public Response(IReadOnlyList<GraphQlError> errors)
        {
            Errors = errors;

            IsCompleted = false;
        }
    }
}