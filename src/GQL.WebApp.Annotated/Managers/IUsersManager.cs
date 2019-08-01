using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GQL.DAL.Models;

namespace GQL.WebApp.Annotated.Managers
{
    public interface IUsersManager
    {
        Task<IObservable<UserModelBase>> OnAddUserAsync();

        Task<ManagerUserModel> CreateManagerAsync(ManagerUserModel manager);

        Task<IEnumerable<CustomerUserModel>> CreateCustomersAsync(IList<CustomerUserModel> customers);
    }
}