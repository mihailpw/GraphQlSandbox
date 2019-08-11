using System;
using GraphQL.Types;

namespace GQL.WebApp.Typed.GraphQl.Extensions
{
    public class FieldTypeBuilder
    {
        private readonly FieldType _fieldType;


        public FieldTypeBuilder(FieldType fieldType)
        {
            _fieldType = fieldType;
        }


        public FieldTypeBuilder Name(string name)
        {
            _fieldType.Name = name;
            return this;
        }

        public FieldTypeBuilder Description(string description)
        {
            _fieldType.Description = description;
            return this;
        }

        public FieldTypeBuilder DeprecationReason(string deprecationReason)
        {
            _fieldType.DeprecationReason = deprecationReason;
            return this;
        }

        public FieldTypeBuilder DefaultValue(object defaultValue)
        {
            _fieldType.DefaultValue = defaultValue;
            return this;
        }

        public FieldTypeBuilder Type(Type type)
        {
            _fieldType.Type = type;
            return this;
        }
    }
}