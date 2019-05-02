using System;
using GQL.Client.QueryBuilders.Dto;
using GQL.Client.QueryBuilders.Infra;

namespace GQL.Client.QueryBuilders
{
    public interface IQueryBuilder : IClient<MutationDto>
    {
        IQueryBuilder IncludeUser(Action<IUserFieldSelector> selectAction, bool include = true);
        IQueryBuilder IncludeUsers(Action<IUserFieldSelector> selectAction, bool include = true);
    }

    public class QueryBuilder : RootObjectBuilderBase<MutationDto>, IQueryBuilder
    {
        public QueryBuilder(string url)
            : base(url, "query")
        {
        }


        public IQueryBuilder IncludeUser(
            Action<IUserFieldSelector> selectAction,
            bool include = true)
        {
            if (include)
            {
                var builder = new UserFieldSelector("user");
                selectAction(builder);
                Include(builder);
            }
            return this;
        }

        public IQueryBuilder IncludeUsers(
            Action<IUserFieldSelector> selectAction,
            bool include = true)
        {
            if (include)
            {
                var builder = new UserFieldSelector("users");
                selectAction(builder);
                Include(builder);
            }
            return this;
        }
    }
}