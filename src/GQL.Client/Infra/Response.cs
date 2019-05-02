using System;
using System.Collections.Generic;

namespace GQL.Client.Infra
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

        public IReadOnlyList<Error> Errors { get; }


        public Response(T data)
        {
            _data = data;

            IsCompleted = true;
            Errors = Array.Empty<Error>();
        }

        public Response(IReadOnlyList<Error> errors)
        {
            Errors = errors;

            IsCompleted = false;
        }
    }
}