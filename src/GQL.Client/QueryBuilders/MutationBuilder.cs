using GQL.Client.QueryBuilders.Dto;
using GQL.Client.QueryBuilders.Infra;

namespace GQL.Client.QueryBuilders
{
    public class MutationBuilder : RootObjectBuilderBase<MutationDto>
    {
        public MutationBuilder(string url)
            : base(url, "mutation")
        {
        }
        
    }
}