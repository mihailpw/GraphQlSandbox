using GQL.DAL.Models;
using GQL.WebApp.Typed.Infra;

namespace GQL.WebApp.Typed.Managers
{
    public class UsersObservable : Observable<UserModelBase>, IUsersObservable
    {
        public void Notify(UserModelBase userModel)
        {
            NotifyAll(userModel);
        }
    }
}