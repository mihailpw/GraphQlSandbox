using GQL.Client.QueryBuilders.Infra;

namespace GQL.Client.QueryBuilders
{
    public class UsersQueryBuilder : ObjectQueryBuilderBase
    {
        public UsersQueryBuilder()
            : base("users")
        {
        }


        public UsersQueryBuilder IncludeId(bool include = true)
        {
            Include("id");
            return this;
        }

        public UsersQueryBuilder IncludeEmail(bool include = true)
        {
            Include("email");
            return this;
        }
    }
}