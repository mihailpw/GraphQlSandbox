using System;
using System.Threading.Tasks;
using GQL.DAL.Models;

namespace GQL.WebApp.Typed.Managers
{
    public interface IUsersManager
    {
        Task<IObservable<UserModelBase>> OnAddUserAsync();

        Task<ManagerUserModel> CreateManagerAsync(ManagerUserModel manager);
    }
}