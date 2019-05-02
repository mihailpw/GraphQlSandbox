using System.Collections.Generic;
using System.Text;

namespace GQL.Client.Infra
{
    public class FieldType : TypeBase
    {
        private readonly string _name;
        private readonly List<Argument> _arguments;
        private readonly TypeBase _type;


        public FieldType(string name, List<Argument> arguments, TypeBase type)
        {
            _name = name;
            _arguments = arguments;
            _type = type;
        }


        public sealed override IEnumerable<Argument> GetArguments()
        {
            foreach (var argument in _arguments)
            {
                yield return argument;
            }

            foreach (var argument in _type.GetArguments())
            {
                yield return argument;
            }
        }

        public sealed override void AppendQuery(StringBuilder builder)
        {
            builder.Append(_name);

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
            _type.AppendQuery(builder);
            builder.Append("}");
        }
    }
}