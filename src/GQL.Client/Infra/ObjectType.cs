using System.Collections.Generic;
using System.Text;

namespace GQL.Client.Infra
{
    public abstract class ObjectType : TypeBase
    {
        private readonly List<TypeBase> _types;


        protected ObjectType()
        {
            _types = new List<TypeBase>();
        }


        public sealed override IEnumerable<Argument> GetArguments()
        {
            foreach (var type in _types)
            {
                foreach (var argument in type.GetArguments())
                {
                    yield return argument;
                }
            }
        }

        public override void AppendQuery(StringBuilder builder)
        {
            foreach (var type in _types)
            {
                type.AppendQuery(builder);
                builder.Append(" ");
            }
        }


        protected void IncludeField(string fieldName)
        {
            _types.Add(new ScalarType(fieldName));
        }

        protected void IncludeField(string fieldName, List<Argument> arguments, TypeBase type)
        {
            _types.Add(new FieldType(fieldName, arguments, type));
        }
    }
}