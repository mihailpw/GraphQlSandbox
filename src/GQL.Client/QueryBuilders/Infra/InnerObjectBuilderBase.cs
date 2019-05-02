using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GQL.Client.QueryBuilders.Infra
{
    public abstract class InnerObjectBuilderBase : ObjectBuilderBase
    {
        private readonly string _key;

        private readonly List<string> _requiredArguments;
        private readonly List<ArgumentData> _arguments;


        protected InnerObjectBuilderBase(string key)
        {
            _key = key;

            _requiredArguments = new List<string>();
            _arguments = new List<ArgumentData>();
        }


        public sealed override void ThrowIfNotValid()
        {
            var addedRequiredArguments = _requiredArguments
                .Where(ra => _arguments.Any(a => a.FieldName == ra))
                .ToList();
            if (addedRequiredArguments.Count != _requiredArguments.Count)
            {
                var notAddedArguments = _requiredArguments.Except(addedRequiredArguments);
                throw new RequiredArgumentNotAddedException(GetType().Name, notAddedArguments);
            }

            foreach (var builder in Includes)
            {
                builder.ThrowIfNotValid();
            }
        }

        public sealed override string Build()
        {
            var stringBuilder = new StringBuilder(_key);

            if (_arguments.Count > 0)
            {
                stringBuilder.Append("(");
                foreach (var argument in _arguments)
                {
                    stringBuilder.Append($"{argument.FieldName}:${argument.ArgumentName},");
                }
                stringBuilder.Length--;
                stringBuilder.Append(")");
            }

            stringBuilder.Append("{");
            foreach (var include in Includes)
            {
                stringBuilder.Append(include.Build());
                stringBuilder.Append(" ");
            }
            stringBuilder.Append("}");

            return stringBuilder.ToString();
        }

        public override IEnumerable<ArgumentData> EnumerateArguments()
        {
            foreach (var argument in _arguments)
            {
                yield return argument;
            }

            foreach (var argument in base.EnumerateArguments())
            {
                yield return argument;
            }
        }


        protected void AddRequiredArgument(string fieldName)
        {
            _requiredArguments.Add(fieldName);
        }

        protected void AddArgument(string fieldName, string typeName, object value)
        {
            _arguments.Add(new ArgumentData(fieldName, typeName, value));
        }
    }
}