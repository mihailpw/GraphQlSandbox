using GQL.DAL.Models;
using GQL.Services.Infra;
using GQL.Services.Infra.Attributes;

namespace GQL.WebApp.Serviced.GraphQlV2.Models
{
    [GraphQlType(
        "Manager",
        Description = "Manager user type")]
    public class ManagerUserObject : UserObjectBase
    {
        [GraphQlField(
            nameof(ManagerUserModel.NumberOfSales),
            Description = "Number of sales")]
        public NonNull<int> NumberOfSales { get; set; }
    }
}