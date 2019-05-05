using System;
using System.Collections.Generic;
using GQL.Client.InfraV2;

namespace GQL.Client.GeneratedClientV2
{
    public class UserInterface : InterfaceBase
    {
        public UserInterface() : base("UserInterface")
        {
        }

        protected UserInterface(string name) : base(name)
        {
        }


        public UserInterface Id()
        {
            IncludeField(
                "id",
                new List<Argument>
                {
                },
                null);
            return this;
        }

        public UserInterface Name()
        {
            IncludeField(
                "name",
                new List<Argument>
                {
                },
                null);
            return this;
        }

        public Func<Action<UserInterface>, UserInterface> Friends(string email = null)
        {
            return a =>
            {
                var type = new UserInterface();
                a(type);
                IncludeField(
                    "friends",
                    new List<Argument>
                    {
                        new Argument("email", "String", email),
                    },
                    type);

                return this;
            };
        }

        public UserInterface OnCustomerType(Action<CustomerType> setupAction)
        {
            var type = new CustomerType();
            setupAction(type);
            IncludeOnTypeField(type);
            return this;
        }
    }
}