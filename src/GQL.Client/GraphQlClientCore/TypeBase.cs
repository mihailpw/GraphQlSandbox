using System;
using System.Collections;
using System.Collections.Generic;

namespace GQL.Client.GraphQlClientCore
{
    public class TypeBase : IEnumerable<RequestBuilderBase>
    {
        private readonly List<RequestBuilderBase> _builders;


        public TypeBase()
        {
            _builders = new List<RequestBuilderBase>();
        }


        IEnumerator<RequestBuilderBase> IEnumerable<RequestBuilderBase>.GetEnumerator()
        {
            return _builders.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _builders.GetEnumerator();
        }


        protected void AddField(string fieldName, bool include)
        {
            if (!include)
            {
                return;
            }

            _builders.Add(new FieldRequestBuilder(fieldName));
        }

        protected void AddObject<TObject>(string fieldName, Action<RequestBuilder<TObject>> setupAction, bool include)
            where TObject : TypeBase, new()
        {
            if (!include)
            {
                return;
            }

            var builder = new ObjectRequestBuilder<TObject>(fieldName);
            setupAction(builder);
            builder.Arguments.ThrowIfNotValid();

            _builders.Add(builder);
        }
    }
}