using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GQL.Client.QueryBuilders.Infra
{
    public abstract class ObjectQueryBuilderBase : QueryBuilderBase
    {
        private readonly string _key;

        private readonly string _builderName;
        private readonly List<string> _requiredArguments;
        private readonly List<FieldData> _arguments;
        private readonly List<QueryBuilderBase> _includes;


        public IReadOnlyList<FieldData> Arguments => _arguments;


        protected ObjectQueryBuilderBase(string key)
        {
            _key = key;

            _builderName = GetType().Name;
            _requiredArguments = new List<string>();
            _arguments = new List<FieldData>();
            _includes = new List<QueryBuilderBase>();
        }


        public override void ThrowIfNotValid()
        {
            var addedRequiredArguments = _requiredArguments
                .Where(ra => _arguments.Any(a => a.FieldName == ra))
                .ToList();
            if (addedRequiredArguments.Count != _requiredArguments.Count)
            {
                var notAddedArguments = _requiredArguments.Except(addedRequiredArguments);
                throw new RequiredArgumentNotAddedException(_builderName, notAddedArguments);
            }
        }

        public override string Build()
        {
            var stringBuilder = new StringBuilder(_key);

            if (_arguments.Count > 0)
            {
                stringBuilder.Append("(");
                foreach (var argument in _arguments)
                {
                    stringBuilder.Append($"{argument.FieldName}:${argument.FieldName}");
                }
                stringBuilder.Append(")");
            }

            stringBuilder.Append("{");
            foreach (var include in _includes)
            {
                stringBuilder.Append(include);
                stringBuilder.Append(" ");
            }
            stringBuilder.Append("}");

            return stringBuilder.ToString();
        }


        protected void AddRequiredArgument(string fieldName)
        {
            _requiredArguments.Add(fieldName);
        }

        protected void AddArgument(string fieldName, string typeName, object value)
        {
            _arguments.Add(new FieldData(_builderName, fieldName, typeName, value));
        }

        protected void Include(string fieldName, bool include = true)
        {
            Include(new SimpleQueryBuilder(fieldName), include);
        }

        protected void Include(QueryBuilderBase queryBuilder, bool include = true)
        {
            if (include)
            {
                _includes.Add(queryBuilder);
            }
        }



        public class FieldData
        {
            public readonly string BuilderName;
            public readonly string FieldName;
            public readonly string FieldTypeName;
            public readonly object Value;


            public FieldData(string builderName, string fieldName, string fieldTypeName, object value)
            {
                BuilderName = builderName;
                FieldName = fieldName;
                FieldTypeName = fieldTypeName;
                Value = value;
            }
        }
    }
}