using System;

namespace Shark.Gameplay.Player
{
    public class Get<T>
    {
        public T Value => getValue.Invoke();

        private Func<T> getValue;

        public Get(Func<T> function)
        {
            getValue = function;
        }
    }
}