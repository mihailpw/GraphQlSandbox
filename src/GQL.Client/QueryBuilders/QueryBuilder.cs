using System;
using GQL.Client.QueryBuilders.Infra;

namespace GQL.Client.QueryBuilders
{
    public interface IQueryBuilder : IClient<QueryDto>
    {
        IQueryBuilder IncludeUser(Action<IUserBuilder> buildAction, bool include = true);
        IQueryBuilder IncludeUsers(Action<IUsersBuilder> buildAction, bool include = true);
    }

    public class QueryBuilder : RootObjectBuilderBase<QueryDto>, IQueryBuilder
    {
        public QueryBuilder(string url)
            : base(url, "query")
        {
        }


        public IQueryBuilder IncludeUser(
            Action<IUserBuilder> buildAction,
            bool include = true)
        {
            if (include)
            {
                var builder = new UserBuilder();
                buildAction(builder);
                Include(builder);
            }
            return this;
        }

        public IQueryBuilder IncludeUsers(
            Action<IUsersBuilder> buildAction,
            bool include = true)
        {
            if (include)
            {
                var builder = new UsersBuilder();
                buildAction(builder);
                Include(builder);
            }
            return this;
        }
    }
}