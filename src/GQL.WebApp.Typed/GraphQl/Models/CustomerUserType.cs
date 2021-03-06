﻿using GQL.DAL.Models;

namespace GQL.WebApp.Typed.GraphQl.Models
{
    public class CustomerUserType : UserTypeBase<CustomerUserModel>
    {
        public CustomerUserType()
        {
            Name = "Customer";

            Field(x => x.IsActive, nullable: false);

            Interface<UserInterface>();
        }
    }
}