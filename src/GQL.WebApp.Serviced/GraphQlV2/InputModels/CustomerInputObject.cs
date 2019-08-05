using System.Collections.Generic;
using GQL.DAL.Models;
using GraphQl.Server.Annotations;
using GraphQl.Server.Annotations.Attributes;

namespace GQL.WebApp.Serviced.GraphQlV2.InputModels
{
    [GraphQlType("CustomerInput")]
    public class CustomerInputObject
    {
        [GraphQlField(nameof(CustomerUserModel.Name))]
        public string Name { get; set; }

        [GraphQlField(nameof(CustomerUserModel.Email))]
        public NonNull<string> Email { get; set; } = "em";

        [GraphQlField(nameof(CustomerUserModel.Roles))]
        public ICollection<string> Roles { get; set; }
    }
}