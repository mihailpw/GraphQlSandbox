using System.Collections.Generic;

namespace GQL.DAL.Models
{
    public class UserModel : EntityModelBase
    {
        public string Name { get; set; }

        public string Email { get; set; }

        public ICollection<UserRoleModel> Roles { get; set; }
    }
}