using GQL.Client.QueryBuilders.Infra;

namespace GQL.Client.QueryBuilders
{
    public interface IUsersBuilder
    {
        IUsersBuilder IncludeEmail(bool include = true);
        IUsersBuilder IncludeId(bool include = true);
    }

    public class UsersBuilder : InnerObjectBuilderBase, IUsersBuilder
    {
        public UsersBuilder()
            : base("users")
        {
        }


        public IUsersBuilder IncludeId(bool include = true)
        {
            Include("id");
            return this;
        }

        public IUsersBuilder IncludeEmail(bool include = true)
        {
            Include("email");
            return this;
        }
    }
}