using System;
using GQL.Client.QueryBuilders.Infra;

namespace GQL.Client.QueryBuilders
{
    public class AppClient : GraphQlClientBase<AppRequestDto>
    {
        public AppClient(string url)
            : base(url, GraphQlRequestType.Query)
        {
        }


        public AppClient IncludeUser(
            Action<UserQueryBuilder> buildAction,
            bool include = true)
        {
            var builder = new UserQueryBuilder();
            buildAction(builder);
            AddQueryBuilder(builder);
            return this;
        }

        public AppClient IncludeUsers(
            Action<UsersQueryBuilder> buildAction,
            bool include = true)
        {
            var builder = new UsersQueryBuilder();
            buildAction(builder);
            AddQueryBuilder(builder);
            return this;
        }
    }
}