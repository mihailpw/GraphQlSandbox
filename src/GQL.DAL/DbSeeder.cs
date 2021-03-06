﻿using System.Diagnostics;
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

            var userAdmin = new ManagerUserModel
            {
                Name = "Admin-01",
                Email = "admin-01@user.us",
                NumberOfSales = 1,
                Type = UserType.Nobody,
            }.WithId();
            var user1User = new CustomerUserModel
            {
                Name = "User-01",
                Email = "user-01@user.us",
                IsActive = true,
                Type = UserType.GoodGuy,
            }.WithId();
            var user2User = new CustomerUserModel
            {
                Name = "User-02",
                Email = "user-02@user.us",
                IsActive = true,
                Type = UserType.BadGuy,
            }.WithId();
            var user3User = new CustomerUserModel
            {
                Name = "User-03",
                Email = "user-03@user.us",
                IsActive = true,
                Type = UserType.BadGuy,
            }.WithId();

            dbContext.AddRange(
                new UserRoleModel { User = userAdmin, Role = roleUser }.WithId(),
                new UserRoleModel { User = userAdmin, Role = roleAdmin }.WithId(),
                new UserRoleModel { User = user1User, Role = roleUser }.WithId(),
                new UserRoleModel { User = user2User, Role = roleUser }.WithId(),
                new UserRoleModel { User = user3User, Role = roleUser }.WithId());
            dbContext.AddRange(
                new UserFriendModel { User = userAdmin, Friend = user1User }.WithId(),
                new UserFriendModel { User = userAdmin, Friend = user2User }.WithId(),
                new UserFriendModel { User = userAdmin, Friend = user3User }.WithId(),
                new UserFriendModel { User = user1User, Friend = userAdmin }.WithId(),
                new UserFriendModel { User = user1User, Friend = user2User }.WithId());
            dbContext.AddRange(userAdmin, user1User, user2User, user3User);

            dbContext.SaveChanges();
        }
    }
}