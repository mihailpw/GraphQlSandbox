using System;
using System.Reactive;
using System.Reactive.Disposables;
using System.Threading.Tasks;
using GQL.WebApp.Typed.GraphQl.Infra;
using GQL.WebApp.Typed.GraphQl.Models;
using GraphQL.Subscription;

namespace GQL.WebApp.Typed.GraphQl.Schemas.Users
{
    internal class UsersSubscription : GraphSubscription
    {
        public UsersSubscription()
        {
            FieldSubscribeAsync<UserType>(
                "addUser",
                subscribeAsync: SubscribeAddUserAsync);
        }

        private async Task<IObservable<object>> SubscribeAddUserAsync(ResolveEventStreamContext context)
        {
            return await Task.FromResult(new AnonymousObservable<object>(observer => Disposable.Empty));
        }
    }
}