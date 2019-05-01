using GQL.Client.QueryBuilders.Infra;

namespace GQL.Client.QueryBuilders
{
    public class UserQueryBuilder : ObjectQueryBuilderBase
    {
        public UserQueryBuilder()
            : base("user")
        {
            AddRequiredArgument("id");
        }


        public UserQueryBuilder FilterId(string id)
        {
            AddArgument("id", "ID!", id);
            return this;
        }

        public UserQueryBuilder IncludeId(bool include = true)
        {
            Include("id");
            return this;
        }

        public UserQueryBuilder IncludeEmail(bool include = true)
        {
            Include("email");
            return this;
        }
    }
}