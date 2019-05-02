using System;

namespace GQL.Client.QueryBuilders.Infra
{
    public class ArgumentData
    {
        public string FieldName { get; }

        public string FieldTypeName { get; }

        public object Value { get; }

        public string ArgumentName { get; }


        public ArgumentData(string fieldName, string fieldTypeName, object value)
        {
            FieldName = fieldName;
            FieldTypeName = fieldTypeName;
            Value = value;

            ArgumentName = $"{FieldName}_{Guid.NewGuid():N}";
        }
    }
}