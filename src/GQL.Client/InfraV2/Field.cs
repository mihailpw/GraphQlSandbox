using System.Collections.Generic;
using System.Text;

namespace GQL.Client.InfraV2
{
    public class Field : FieldBase
    {
        private readonly List<Argument> _arguments;


        public Field(string key, List<Argument> arguments, TypeBase type)
            : base(key, type)
        {
            _arguments = arguments;
        }


        protected sealed override void AppendArguments(StringBuilder builder)
        {
            if (_arguments.Count > 0)
            {
                builder.Append("(");
                foreach (var argument in _arguments)
                {
                    builder.Append($"{argument.Name}:${argument.Id},");
                }
                builder.Length--;
                builder.Append(")");
            }
        }

        protected override IEnumerable<Argument> GetArguments()
        {
            foreach (var argument in base.GetArguments())
            {
                yield return argument;
            }

            foreach (var argument in _arguments)
            {
                yield return argument;
            }
        }
    }
}