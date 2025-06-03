using System;
using System.Collections.Generic;

namespace Scripts.Utils.Disposables
{
    public class CompositeDisposable : IDisposable
    {
        private readonly List<IDisposable> _disposables = new();

        public void Retain(params IDisposable[] disposable)
        {
            foreach (var item in disposable)
            {
                _disposables.Add(item);
            }
        }
        public void Dispose()
        {
            foreach (var disposable in _disposables)
            {
                disposable.Dispose();
            }
            _disposables.Clear();
        }
    }
}