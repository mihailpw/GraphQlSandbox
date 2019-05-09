using System;
using System.Collections.Generic;
using System.Reactive.Disposables;

namespace GQL.WebApp.Typed.Infra
{
    public abstract class Observable<T> : IObservable<T>
    {
        private readonly List<IObserver<T>> _observers;


        protected Observable()
        {
            _observers = new List<IObserver<T>>();
        }


        public IDisposable Subscribe(IObserver<T> observer)
        {
            _observers.Add(observer);
            return Disposable.Create(() => _observers.Remove(observer));
        }

        protected void NotifyAll(T data)
        {
            foreach (var observer in _observers)
            {
                observer.OnNext(data);
            }
        }
    }
}