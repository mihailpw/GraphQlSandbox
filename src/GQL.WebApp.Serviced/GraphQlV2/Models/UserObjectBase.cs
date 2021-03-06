﻿using System.Collections.Generic;
using System.Linq;
using GQL.DAL.Models;
using GQL.Services.Infra;
using GraphQL.Types;

namespace GQL.WebApp.Serviced.GraphQlV2.Models
{
    public abstract class UserObjectBase : EntityObjectBase, IUserObject
    {
        [GraphQlField(
            nameof(UserModelBase.Name),
            Description = "User name")]
        public string Name { get; set; }

        [GraphQlField(
            nameof(UserModelBase.Email),
            Description = "User email")]
        public string Email { get; set; }

        [GraphQlField(
            nameof(UserModelBase.Type),
            Description = "The type of user")]
        public UserType Type { get; set; }

        [GraphQlField(
            nameof(UserModelBase.Roles),
            Description = "User roles")]
        public IEnumerable<string> GetRoles(ResolveFieldContext<UserModelBase> context)
        {
            return context.Source.Roles.Select(r => r.Role.Name);
        }

        [GraphQlField(
            nameof(UserModelBase.Friends),
            Description = "User friends")]
        public IEnumerable<UserModelBase> GetFriends(ResolveFieldContext<UserModelBase> context, string email = null)
        {
            if (context.Source.Friends == null)
            {
                return null;
            }

            var friends = context
                .Source
                .Friends
                .Select(r => r.Friend);

            if (!string.IsNullOrEmpty(email))
            {
                friends = friends.Where(f => f.Email == email);
            }

            return friends;
        }
    }
}