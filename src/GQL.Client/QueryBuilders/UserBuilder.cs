using GQL.Client.QueryBuilders.Infra;

namespace GQL.Client.QueryBuilders
{
    public interface IUserBuilder
    {
        IUserBuilder FilterId(string id);
        IUserBuilder IncludeEmail(bool include = true);
        IUserBuilder IncludeId(bool include = true);
    }

    public class UserBuilder : InnerObjectBuilderBase, IUserBuilder
    {
        public UserBuilder()
            : base("user")
        {
            AddRequiredArgument("id");
        }


        public IUserBuilder FilterId(string id)
        {
            AddArgument("id", "ID!", id);
            return this;
        }

        public IUserBuilder IncludeId(bool include = true)
        {
            Include("id");
            return this;
        }

        public IUserBuilder IncludeEmail(bool include = true)
        {
            Include("email");
            return this;
        }
    }
}