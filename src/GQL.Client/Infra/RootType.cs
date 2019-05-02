using System.Collections.Generic;
using System.Text;

namespace GQL.Client.Infra
{
    public class RootType : TypeBase
    {
        private readonly string _requestType;


        public RootType(string requestType)
        {
            _requestType = requestType;
        }


        public override IEnumerable<Argument> GetArguments()
        {
            yield break;
        }

        public override void AppendQuery(StringBuilder builder)
        {
            builder.Append(_requestType);

            var arguments = 
        }
    }
}