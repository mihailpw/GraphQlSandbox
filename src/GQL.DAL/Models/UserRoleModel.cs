namespace GQL.DAL.Models
{
    public class UserRoleModel : EntityModelBase
    {
        public string UserId { get; set; }

        public UserModelBase User { get; set; }

        public int RoleId { get; set; }

        public RoleModel Role { get; set; }
    }
}