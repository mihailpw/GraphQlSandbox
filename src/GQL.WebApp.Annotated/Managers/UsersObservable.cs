using GQL.DAL.Models;
using GQL.WebApp.Annotated.Infra;

namespace GQL.WebApp.Annotated.Managers
{
    public class UsersObservable : Observable<UserModelBase>, IUsersObservable
    {
        public void Notify(UserModelBase userModel)
        {
            NotifyAll(userModel);
        }
    }
}