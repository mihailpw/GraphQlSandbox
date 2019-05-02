using System.Collections.Generic;
using System.Text;

namespace GQL.Client.Infra
{
    public sealed class ScalarType : TypeBase
    {
        private readonly string _fieldName;


        public ScalarType(string fieldName)
        {
            _fieldName = fieldName;
        }


        public override IEnumerable<Argument> GetArguments()
        {
            yield break;
        }

        public override void AppendQuery(StringBuilder builder)
        {
            builder.Append(_fieldName);
        }
    }
}