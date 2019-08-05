using System.Collections.Generic;
using GQL.DAL.Models;
using GQL.Services.Infra;
using GQL.Services.Infra.Attributes;

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
        IEnumerable<string> GetRoles(UserModelBase source);

        [GraphQlField(
            nameof(UserModelBase.Friends),
            Description = "User friends")]
        IEnumerable<UserModelBase> GetFriends(
            UserModelBase source,
            [GraphQlParameter("email")] string email2 = null);
    }
}