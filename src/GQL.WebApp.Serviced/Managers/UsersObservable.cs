using GQL.DAL.Models;
using GQL.WebApp.Serviced.Infra;

namespace GQL.WebApp.Serviced.Managers
{
    public class UsersObservable : Observable<UserModelBase>, IUsersObservable
    {
        public void Notify(UserModelBase userModel)
        {
            NotifyAll(userModel);
        }
    }
}