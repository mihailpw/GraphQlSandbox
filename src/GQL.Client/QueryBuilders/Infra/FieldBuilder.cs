using System.Collections.Generic;

namespace GQL.Client.QueryBuilders.Infra
{
    public sealed class FieldBuilder : BuilderBase
    {
        private readonly string _field;


        public FieldBuilder(string field)
        {
            _field = field;
        }


        public override void ThrowIfNotValid()
        {
        }

        public override string Build()
        {
            return _field;
        }

        public override IEnumerable<ArgumentData> EnumerateArguments()
        {
            yield break;
        }
    }
}