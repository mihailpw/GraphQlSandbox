using System;
using GQL.DAL.Models;

namespace GQL.WebApp.Serviced.Managers
{
    public interface IUsersObservable : IObservable<UserModelBase>
    {
        void Notify(UserModelBase userModel);
    }
}