using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GQL.DAL.Models;
using GQL.WebApp.Serviced.GraphQlV2.InputModels;
using GQL.WebApp.Serviced.Managers;
using GraphQl.Server.Annotations;
using GraphQl.Server.Annotations.Attributes;

namespace GQL.WebApp.Serviced.GraphQlV2
{
    [GraphQlType("UsersMutation")]
    public class UsersMutation
    {
        private readonly IUsersManager _usersManager;


        public UsersMutation(IUsersManager usersManager)
        {
            _usersManager = usersManager;
        }


        [GraphQlField("createManager")]
        public async Task<NonNull<ManagerUserModel>> CreateManagerAsync(
            [GraphQlParameter] NonNull<ManagerInputObject> manager)
        {
            var managerModel = new ManagerUserModel
            {
                Name = manager.Value.Name,
                Email = manager.Value.Email,
            };

            return await _usersManager.CreateManagerAsync(managerModel);
        }

        [GraphQlField("createCustomers")]
        public async Task<NonNull<IEnumerable<NonNull<CustomerUserModel>>>> CreateCustomersAsync(
            [GraphQlParameter] NonNull<ICollection<NonNull<CustomerInputObject>>> customers)
        {
            var customerModels = customers.Value.Select(c => new CustomerUserModel
            {
                Name = c.Value.Name,
                Email = c.Value.Email,
            }).ToList();

            return NonNull.ForAndEach(await _usersManager.CreateCustomersAsync(customerModels));
        }
    }
}