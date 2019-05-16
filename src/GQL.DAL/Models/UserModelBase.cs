using System.Collections.Generic;

namespace GQL.DAL.Models
{
    public abstract class UserModelBase : EntityModelBase
    {
        public string Name { get; set; }

        public string Email { get; set; }

        public UserType Type { get; set; }

        public ICollection<UserRoleModel> Roles { get; set; }

        public ICollection<UserFriendModel> Friends { get; set; }
    }
}