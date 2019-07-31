using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GQL.DAL;
using GQL.DAL.Models;

namespace GQL.WebApp.Serviced.Managers
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

        public async Task<IEnumerable<CustomerUserModel>> CreateCustomersAsync(IList<CustomerUserModel> customers)
        {
            await _appDbContext.AddRangeAsync(customers);
            await _appDbContext.SaveChangesAsync();

            foreach (var customer in customers)
            {
                _usersObservable.Notify(customer);
            }

            return customers;
        }
    }
}