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
            var user1User = new UserModel
            {
                Name = "User-01",
                Email = "user-01@user.us",
            };
            var user2User = new UserModel
            {
                Name = "User-02",
                Email = "user-02@user.us",
            };
            var user3User = new UserModel
            {
                Name = "User-03",
                Email = "user-03@user.us",
            };

            dbContext.AddRange(
                new UserRoleModel { User = userAdmin, Role = roleUser },
                new UserRoleModel { User = userAdmin, Role = roleAdmin },
                new UserRoleModel { User = user1User, Role = roleUser },
                new UserRoleModel { User = user2User, Role = roleUser },
                new UserRoleModel { User = user3User, Role = roleUser });
            dbContext.AddRange(
                new UserFriendModel { User = userAdmin, Friend = user1User },
                new UserFriendModel { User = userAdmin, Friend = user2User },
                new UserFriendModel { User = userAdmin, Friend = user3User },
                new UserFriendModel { User = user1User, Friend = userAdmin },
                new UserFriendModel { User = user1User, Friend = user2User });
            dbContext.AddRange(userAdmin, user1User, user2User, user3User);

            dbContext.SaveChanges();
        }
    }
}