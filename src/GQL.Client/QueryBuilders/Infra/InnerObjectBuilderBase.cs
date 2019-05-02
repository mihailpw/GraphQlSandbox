using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GQL.Client.QueryBuilders.Infra
{
    public abstract class InnerObjectBuilderBase : ObjectBuilderBase
    {
        private readonly List<string> _requiredArguments;
        private readonly List<ArgumentData> _arguments;


        protected InnerObjectBuilderBase(string key)
            : base(key)
        {
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


        protected override void BuildArguments(StringBuilder builder)
        {
            if (_arguments.Count == 0)
            {
                return;
            }

            builder.Append("(");
            foreach (var argument in _arguments)
            {
                builder.Append($"{argument.FieldName}:${argument.ArgumentName},");
            }
            builder.Length--;
            builder.Append(")");
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