namespace GQL.DAL.Models
{
    public class UserFriendModel : EntityModelBase
    {
        public string UserId { get; set; }

        public UserModel User { get; set; }

        public string FriendId { get; set; }

        public UserModel Friend { get; set; }
    }
}