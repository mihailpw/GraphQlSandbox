using GQL.DAL.Models;
using GQL.Services.Infra;

namespace GQL.WebApp.Serviced.GraphQlV2.Models
{
    [GraphQlType(
        "Manager",
        Description = "Manager user type")]
    public class ManagerUserObject : UserObjectBase
    {
        [GraphQlField(
            nameof(ManagerUserModel.NumberOfSales),
            Description = "Number of sales",
            IsRequired = true)]
        public int NumberOfSales { get; set; }
    }
}