namespace GQL.DAL.Models
{
    public class UserFriendModel : EntityModelBase
    {
        public string UserId { get; set; }

        public UserModelBase User { get; set; }

        public string FriendId { get; set; }

        public UserModelBase Friend { get; set; }
    }
}