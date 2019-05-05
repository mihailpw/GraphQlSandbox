using System.Collections.Generic;
using GQL.Client.InfraV2;

namespace GQL.Client.GeneratedClientV2
{
    public class CustomerType : UserInterface
    {
        public CustomerType() : base("Customer")
        {
        }


        public CustomerType IsActive()
        {
            IncludeField(
                "isActive",
                new List<Argument>
                {
                },
                null);

            return this;
        }
    }
}