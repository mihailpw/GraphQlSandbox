using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GQL.DAL.Models;
using GQL.Services.Infra;
using GQL.WebApp.Serviced.GraphQlV2.InputModels;
using GQL.WebApp.Serviced.Managers;

namespace GQL.WebApp.Serviced.GraphQlV2
{
    public class UsersMutation
    {
        private readonly IUsersManager _usersManager;


        public UsersMutation(IUsersManager usersManager)
        {
            _usersManager = usersManager;
        }


        [GraphQlField("createManager", IsRequired = true)]
        public async Task<ManagerUserModel> CreateManagerAsync(
            [GraphQlParameter(IsRequired = true)] ManagerInputObject manager)
        {
            var managerModel = new ManagerUserModel
            {
                Name = manager.Name,
                Email = manager.Email,
            };

            return await _usersManager.CreateManagerAsync(managerModel);
        }

        [GraphQlField("createCustomers")]
        public async Task<IEnumerable<CustomerUserModel>> CreateCustomersAsync(
            [GraphQlParameter(IsRequired = true)] ICollection<CustomerInputObject> customers)
        {
            var customerModels = customers.Select(c => new CustomerUserModel
            {
                Name = c.Name,
                Email = c.Email,
            }).ToList();

            return await _usersManager.CreateCustomersAsync(customerModels);
        }
    }
}