using System.Collections.Generic;
using GQL.DAL.Models;
using GQL.Services.Infra;
using GQL.Services.Infra.Attributes;
using GraphQL.Types;

namespace GQL.WebApp.Serviced.GraphQlV2.Models
{
    [GraphQlType("UserInterface")]
    public interface IUserObject
    {
        [GraphQlField(
            nameof(UserModelBase.Id),
            Description = "The identificator")]
        Id<string> Id { get; set; }

        [GraphQlField(
            nameof(UserModelBase.Name),
            Description = "User name")]
        string Name { get; set; }

        [GraphQlField(
            nameof(UserModelBase.Email),
            Description = "User email")]
        string Email { get; set; }

        [GraphQlField(
            nameof(UserModelBase.Type),
            Description = "The type of user")]
        UserType Type { get; set; }

        [GraphQlField(
            nameof(UserModelBase.Roles),
            Description = "User roles")]
        IEnumerable<string> GetRoles(ResolveFieldContext<UserModelBase> context);

        [GraphQlField(
            nameof(UserModelBase.Friends),
            Description = "User friends")]
        IEnumerable<UserModelBase> GetFriends(ResolveFieldContext<UserModelBase> context, string email = null);
    }
}