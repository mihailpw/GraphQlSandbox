using System.Collections.Generic;
using System.Text;

namespace GQL.Client.InfraV2
{
    public abstract class TypeBase : IRequestBuilder, IArgumentsProvider, INameProvider
    {
        private readonly string _name;
        private readonly List<FieldBase> _fields;


        string INameProvider.Name => _name;


        protected TypeBase(string name)
        {
            _name = name;
            _fields = new List<FieldBase>();
        }


        void IRequestBuilder.AppendRequest(StringBuilder builder)
        {
            foreach (var field in _fields)
            {
                var requestBuilder = (IRequestBuilder) field;
                if (requestBuilder != null)
                {
                    requestBuilder.AppendRequest(builder);
                    builder.Append(" ");
                }
            }
        }

        IEnumerable<Argument> IArgumentsProvider.GetArguments()
        {
            foreach (var field in _fields)
            {
                var argumentsProvider = (IArgumentsProvider) field;
                if (argumentsProvider != null)
                {
                    foreach (var argument in argumentsProvider.GetArguments())
                    {
                        yield return argument;
                    }
                }
            }
        }


        protected void IncludeField(string fieldName, List<Argument> arguments, TypeBase type)
        {
            _fields.Add(new Field(fieldName, arguments, type));
        }

        protected void Include(FieldBase field)
        {
            _fields.Add(field);
        }
    }
}