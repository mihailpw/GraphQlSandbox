using System.Text;

namespace GQL.Client.InfraV2
{
    public class OnTypeField : FieldBase
    {
        public OnTypeField(TypeBase type)
            : base($"... on {((INameProvider) type).Name}", type)
        {
        }

        protected override void AppendArguments(StringBuilder builder)
        {
        }
    }
}