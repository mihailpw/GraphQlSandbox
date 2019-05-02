using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GQL.Client.Infra
{
    public sealed class RootType : TypeBase
    {
        private readonly string _requestType;
        private readonly TypeBase _type;


        public RootType(string requestType, TypeBase type)
        {
            _requestType = requestType;
            _type = type;
        }


        public override IEnumerable<Argument> GetArguments()
        {
            return _type.GetArguments();
        }

        public override void AppendQuery(StringBuilder builder)
        {
            builder.Append(_requestType);

            var arguments = GetArguments().ToList();
            if (arguments.Count > 0)
            {
                builder.Append("(");
                foreach (var argument in arguments)
                {
                    builder.Append($"${argument.ArgumentName}:{argument.Type},");
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