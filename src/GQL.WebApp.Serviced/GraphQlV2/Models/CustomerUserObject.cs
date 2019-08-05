using GQL.DAL.Models;
using GraphQl.Server.Annotations;
using GraphQl.Server.Annotations.Attributes;

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