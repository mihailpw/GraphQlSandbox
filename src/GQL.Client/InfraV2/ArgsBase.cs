using System.Collections.Generic;
using System.Text;

namespace GQL.Client.InfraV2
{
    public abstract class ArgsBase : IRequestBuilder
    {
        private readonly List<Argument> _arguments;


        protected ArgsBase(List<Argument> arguments)
        {
            _arguments = arguments;
        }


        void IRequestBuilder.AppendRequest(StringBuilder builder)
        {
            if (_arguments.Count == 0)
            {
                return;
            }

            builder.Append("(");

            foreach (var argument in _arguments)
            {
                builder.Append($"{argument.Name}:${argument.Id},");
                builder.Length--;
            }

            builder.Append(")");
        }
    }
}