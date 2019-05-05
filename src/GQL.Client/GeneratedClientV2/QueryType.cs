using System;
using System.Collections.Generic;
using GQL.Client.InfraV2;

namespace GQL.Client.GeneratedClientV2
{
    public class QueryType : TypeBase
    {
        public QueryType() : base("QueryType")
        {
        }


        public Func<Action<UserInterface>, QueryType> User(string id)
        {
            return a =>
            {
                var type = new UserInterface();
                a(type);
                IncludeField(
                    "user",
                    new List<Argument>
                    {
                        new Argument("id", "ID!", id),
                    },
                    type);

                return this;
            };
        }

        public Func<Action<UserInterface>, QueryType> Users()
        {
            return a =>
            {
                var type = new UserInterface();
                a(type);
                IncludeField(
                    "users",
                    new List<Argument>
                    {
                    },
                    type);

                return this;
            };
        }
    }
}