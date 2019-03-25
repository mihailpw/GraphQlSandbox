using System.Diagnostics;
using GQL.DAL.Models;

namespace GQL.DAL
{
    public static class DbSeeder
    {
        [Conditional("DEBUG")]
        public static void Seed(AppDbContext dbContext)
        {
            var roleAdmin = new RoleModel { Id = 1, Name = "Admin" };
            var roleUser = new RoleModel { Id = 2, Name = "User" };

            var userAdmin = new UserModel
            {
                Name = "Admin-01",
                Email = "admin-01@user.us"
            };
            var userUser = new UserModel
            {
                Name = "User-01",
                Email = "user-01@user.us",
            };

            dbContext.AddRange(
                new UserRoleModel { Role = roleUser, User = userAdmin },
                new UserRoleModel { Role = roleAdmin, User = userAdmin },
                new UserRoleModel { Role = roleUser, User = userUser });
            dbContext.AddRange(userAdmin, userUser);

            dbContext.SaveChanges();
        }
    }
}