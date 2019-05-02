using GQL.Client.QueryBuilders.Infra;

namespace GQL.Client.QueryBuilders
{
    public interface IUserFieldSelector
    {
        IUserFieldSelector IncludeEmail(bool include = true);
        IUserFieldSelector IncludeId(bool include = true);
    }

    public class UserFieldSelector : InnerObjectBuilderBase, IUserFieldSelector
    {
        public UserFieldSelector(string key)
            : base(key)
        {
        }

        public IUserFieldSelector IncludeId(bool include = true)
        {
            Include("id");
            return this;
        }

        public IUserFieldSelector IncludeEmail(bool include = true)
        {
            Include("email");
            return this;
        }
    }
}