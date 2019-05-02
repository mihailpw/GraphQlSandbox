using System.Collections.Generic;

namespace GQL.Client.QueryBuilders.Infra
{
    public abstract class BuilderBase
    {
        public abstract void ThrowIfNotValid();

        public abstract string Build();

        public abstract IEnumerable<ArgumentData> EnumerateArguments();

        public override string ToString()
        {
            return Build();
        }
    }
}