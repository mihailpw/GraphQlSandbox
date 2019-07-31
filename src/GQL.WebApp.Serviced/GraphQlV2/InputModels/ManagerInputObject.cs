using GQL.DAL.Models;
using GQL.Services.Infra;

namespace GQL.WebApp.Serviced.GraphQlV2.InputModels
{
    [GraphQlType("ManagerInput")]
    public class ManagerInputObject
    {
        [GraphQlField(nameof(CustomerUserModel.Name))]
        public string Name { get; set; }

        [GraphQlField(nameof(CustomerUserModel.Email))]
        public string Email { get; set; } = "em";
    }
}