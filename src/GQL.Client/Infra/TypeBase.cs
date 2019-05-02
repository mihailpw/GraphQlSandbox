using System.Collections.Generic;
using System.Text;

namespace GQL.Client.Infra
{
    public abstract class TypeBase
    {
        public abstract IEnumerable<Argument> GetArguments();

        public abstract void AppendQuery(StringBuilder builder);
    }
}