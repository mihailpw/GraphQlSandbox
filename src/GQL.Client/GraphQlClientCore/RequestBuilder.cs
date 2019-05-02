using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GQL.Client.GraphQlClientCore
{
    public abstract class RequestBuilder<TType> : RequestBuilderBase, IArgumentsProvider
        where TType : TypeBase, new()
    {
        private readonly string _key;


        public Arguments Arguments { get; }

        public TType Type { get; }


        protected RequestBuilder(string key)
        {
            _key = key;

            var builderName = GetType().Name;
            Arguments = new Arguments(builderName);
            Type = new TType();
        }


        public override string GenerateQuery()
        {
            var stringBuilder = new StringBuilder(_key);

            var arguments = ResolveArguments().ToList();
            if (arguments.Count > 0)
            {
                stringBuilder.Append("(");
                foreach (var (key, value) in arguments)
                {
                    stringBuilder.Append($"{key}:{value},");
                }
                stringBuilder.Length--;
                stringBuilder.Append(")");
            }

            stringBuilder.Append("{");
            foreach (var builder in Type)
            {
                stringBuilder.Append(builder.GenerateQuery());
                stringBuilder.Append(" ");
            }
            stringBuilder.Append("}");

            return stringBuilder.ToString();
        }

        public IEnumerable<Arguments.Entry> GetArguments()
        {
            foreach (var argument in Arguments)
            {
                yield return argument;
            }

            foreach (var builder in Type)
            {
                if (builder is IArgumentsProvider argumentsProvider)
                {
                    foreach (var argument in argumentsProvider.Arguments)
                    {
                        yield return argument;
                    }
                }
            }
        }


        protected abstract IEnumerable<(string key, string value)> ResolveArguments();
    }
}