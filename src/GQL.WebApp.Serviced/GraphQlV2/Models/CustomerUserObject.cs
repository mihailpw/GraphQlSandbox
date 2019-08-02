using GQL.DAL.Models;
using GQL.Services.Infra;
using GQL.Services.Infra.Attributes;

namespace GQL.WebApp.Serviced.GraphQlV2.Models
{
    [GraphQlType("Customer")]
    public class CustomerUserObject : UserObjectBase
    {
        [GraphQlField(
            nameof(CustomerUserModel.IsActive),
            Description = "Defines customer is active or not")]
        public NonNull<bool> IsActive { get; set; }
    }
}