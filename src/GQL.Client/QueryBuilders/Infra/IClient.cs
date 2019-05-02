using System.Threading.Tasks;

namespace GQL.Client.QueryBuilders.Infra
{
    public interface IClient<TDto>
    {
        Task<GraphQlResponse<TDto>> SendAsync();
    }
}