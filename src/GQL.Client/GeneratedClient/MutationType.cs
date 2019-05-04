using System.Collections.Generic;
using GQL.Client.GeneratedClient.Dto;
using GQL.Client.Infra;

namespace GQL.Client.GeneratedClient
{
    public interface IMutationType
    {
        ITypeConfigurator<IMutationType, IUserType> CreateUser(UserInputDto user);
    }

    public class MutationType : ObjectType, IMutationType
    {
        public ITypeConfigurator<IMutationType, IUserType> CreateUser(UserInputDto user)
        {
            return new ObjectTypeConfigurator<IMutationType, IUserType>(
                this,
                "createUser",
                new List<Argument>
                {
                    new Argument("user", "UserInput!", user),
                },
                () => new UserType(),
                IncludeField);
        }
    }
}