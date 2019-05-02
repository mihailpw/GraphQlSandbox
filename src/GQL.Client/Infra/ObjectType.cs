using System.Collections.Generic;
using System.Text;

namespace GQL.Client.Infra
{
    public abstract class ObjectType : TypeBase
    {
        private readonly string _fieldName;
        private readonly List<Argument> _arguments;

        private readonly List<TypeBase> _types;


        protected ObjectType(string fieldName, List<Argument> arguments)
        {
            _fieldName = fieldName;
            _arguments = arguments;

            _types = new List<TypeBase>();
        }


        public sealed override IEnumerable<Argument> GetArguments()
        {
            foreach (var argument in _arguments)
            {
                yield return argument;
            }

            foreach (var type in _types)
            {
                foreach (var argument in type.GetArguments())
                {
                    yield return argument;
                }
            }
        }

        public sealed override void AppendQuery(StringBuilder builder)
        {
            builder.Append(_fieldName);

            if (_arguments.Count > 0)
            {
                builder.Append("(");
                foreach (var argument in _arguments)
                {
                    builder.Append($"{argument.Name}:${argument.ArgumentName},");
                }
                builder.Length--;
                builder.Append(")");
            }

            builder.Append("{");
            foreach (var type in _types)
            {
                type.AppendQuery(builder);
                builder.Append(" ");
            }
            builder.Append("}");
        }


        protected void IncludeField(string fieldName)
        {
            _types.Add(new ScalarType(fieldName));
        }

        protected void IncludeObject(ObjectType objectType)
        {
            _types.Add(objectType);
        }
    }
}