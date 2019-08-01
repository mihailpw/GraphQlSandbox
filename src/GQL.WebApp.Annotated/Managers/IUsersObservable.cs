using System;
using GQL.DAL.Models;

namespace GQL.WebApp.Annotated.Managers
{
    public interface IUsersObservable : IObservable<UserModelBase>
    {
        void Notify(UserModelBase userModel);
    }
}