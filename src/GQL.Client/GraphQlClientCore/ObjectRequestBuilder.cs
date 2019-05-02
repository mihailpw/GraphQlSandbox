using System.Collections.Generic;
using System.Linq;

namespace GQL.Client.GraphQlClientCore
{
    public class ObjectRequestBuilder<TObject> : RequestBuilder<TObject>
        where TObject : new()
    {
        public ObjectRequestBuilder(string key)
            : base(key)
        {
        }


        protected override IEnumerable<(string key, string value)> ResolveArguments()
        {
            return Arguments
                .Select(a => (FieldName: a.Name, $"${a.ArgumentName}"));
        }
    }
}