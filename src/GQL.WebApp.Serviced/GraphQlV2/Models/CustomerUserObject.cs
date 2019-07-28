using GQL.DAL.Models;
using GQL.Services.Infra;

namespace GQL.WebApp.Serviced.GraphQlV2.Models
{
    [GraphQlType("Customer")]
    public class CustomerUserObject : UserObjectBase
    {
        [GraphQlField(
            nameof(CustomerUserModel.IsActive),
            Description = "Defines customer is active or not",
            IsRequired = true)]
        public bool IsActive { get; set; }
    }
}