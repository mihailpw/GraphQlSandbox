using System;
using System.Threading.Tasks;
using GQL.DAL;
using GQL.DAL.Models;

namespace GQL.WebApp.Typed.Managers
{
    public class UsersManager : IUsersManager
    {
        private readonly AppDbContext _appDbContext;
        private readonly IUsersObservable _usersObservable;


        public UsersManager(AppDbContext appDbContext, IUsersObservable usersObservable)
        {
            _appDbContext = appDbContext;
            _usersObservable = usersObservable;
        }


        public async Task<IObservable<UserModelBase>> OnAddUserAsync()
        {
            return await Task.FromResult(_usersObservable);
        }

        public async Task<ManagerUserModel> CreateManagerAsync(ManagerUserModel manager)
        {
            await _appDbContext.AddAsync(manager);
            await _appDbContext.SaveChangesAsync();

            _usersObservable.Notify(manager);

            return manager;
        }
    }
}