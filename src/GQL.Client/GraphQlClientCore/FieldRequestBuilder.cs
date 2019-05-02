using System.Collections.Generic;

namespace GQL.Client.GraphQlClientCore
{
    public class FieldRequestBuilder : RequestBuilderBase
    {
        private readonly string _fieldName;


        public FieldRequestBuilder(string fieldName)
        {
            _fieldName = fieldName;
        }


        public override string GenerateQuery()
        {
            return _fieldName;
        }
    }
}