using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GQL.Client.InfraV2
{
    public abstract class FieldBase : IRequestBuilder, IArgumentsProvider
    {
        private readonly string _key;


        protected TypeBase Type { get; }


        protected FieldBase(string key, TypeBase type)
        {
            _key = key;
            Type = type;
        }


        void IRequestBuilder.AppendRequest(StringBuilder builder)
        {
            builder.Append(_key);

            AppendArguments(builder);

            if (Type is IRequestBuilder typeRequestBuilder)
            {
                builder.Append("{");
                typeRequestBuilder.AppendRequest(builder);
                builder.Append("}");
            }
        }

        IEnumerable<Argument> IArgumentsProvider.GetArguments()
        {
            return GetArguments();
        }


        protected abstract void AppendArguments(StringBuilder builder);

        protected virtual IEnumerable<Argument> GetArguments()
        {
            return Type != null
                ? ((IArgumentsProvider) Type).GetArguments()
                : Enumerable.Empty<Argument>();
        }
    }
}