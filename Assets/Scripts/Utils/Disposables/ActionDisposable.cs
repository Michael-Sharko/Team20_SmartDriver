using System;

namespace Scripts.Utils.Disposables
{
    public class ActionDisposable : IDisposable
    {
        private Action _onDispose;
        public ActionDisposable(Action action)
        {
            _onDispose = action;
        }
        public void Dispose()
        {
            _onDispose?.Invoke();
            _onDispose = null;
        }
    }
}