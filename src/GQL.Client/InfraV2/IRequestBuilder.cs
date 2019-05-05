using System.Text;

namespace GQL.Client.InfraV2
{
    public interface IRequestBuilder
    {
        void AppendRequest(StringBuilder builder);
    }
}