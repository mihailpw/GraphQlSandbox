using System;
using System.Collections.Generic;
using GQL.Client.Dto;
using GQL.Client.Infra;

namespace GQL.Client.GeneratedClient
{
    public interface IMutationType
    {
        IMutationType CreateUser(UserInputDto user, Action<IUserType> setupAction, bool include = true);
    }

    public class MutationType : ObjectType, IMutationType
    {
        public IMutationType CreateUser(UserInputDto user, Action<IUserType> setupAction, bool include = true)
        {
            if (include)
            {
                var type = new UserType();
                setupAction(type);
                IncludeField(
                    "createUser",
                    new List<Argument>
                    {
                        new Argument("user", "UserInput!", user),
                    },
                    type);
            }

            return this;
        }
    }
}