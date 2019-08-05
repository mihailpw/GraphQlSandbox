using GQL.DAL.Models;
using GraphQl.Server.Annotations;
using GraphQl.Server.Annotations.Attributes;

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