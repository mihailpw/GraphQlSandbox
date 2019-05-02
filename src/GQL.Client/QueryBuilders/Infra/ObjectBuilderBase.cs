using System.Collections.Generic;
using System.Text;

namespace GQL.Client.QueryBuilders.Infra
{
    public abstract class ObjectBuilderBase : BuilderBase
    {
        private readonly string _key;

        protected readonly List<BuilderBase> Includes;


        protected ObjectBuilderBase(string key)
        {
            _key = key;

            Includes = new List<BuilderBase>();
        }


        public override IEnumerable<ArgumentData> EnumerateArguments()
        {
            foreach (var builder in Includes)
            {
                foreach (var argument in builder.EnumerateArguments())
                {
                    yield return argument;
                }
            }
        }

        public sealed override string Build()
        {
            var stringBuilder = new StringBuilder(_key);

            BuildArguments(stringBuilder);

            stringBuilder.Append("{");
            foreach (var include in Includes)
            {
                stringBuilder.Append(include.Build());
                stringBuilder.Append(" ");
            }
            stringBuilder.Append("}");

            return stringBuilder.ToString();
        }


        protected abstract void BuildArguments(StringBuilder builder);

        protected void Include(string fieldName, bool include = true)
        {
            Include(new FieldBuilder(fieldName), include);
        }

        protected void Include(BuilderBase builder, bool include = true)
        {
            if (!include)
            {
                return;
            }

            builder.ThrowIfNotValid();
            Includes.Add(builder);
        }
    }
}