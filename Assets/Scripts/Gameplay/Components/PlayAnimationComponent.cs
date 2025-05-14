using System.Collections.Generic;
using UnityEngine;

namespace Scripts.Gameplay.Components
{
    public class PlayAnimationComponent
    {
        private readonly Dictionary<string, int> stringToHash = new();
        private readonly Animator _animator;

        public PlayAnimationComponent(Animator animator)
        {
            _animator = animator;

            for (int i = 0; i < _animator.parameters.Length; i++)
            {
                string parameterName = _animator.parameters[i].name;
                stringToHash.Add(parameterName, Animator.StringToHash(parameterName));
            }
        }
        // если вызвать бул как триггер или триггер как бул то это может сработать
        // но думаю нет смысла это фиксить
        public void Play(string id)
        {
            _animator.SetTrigger(stringToHash[id]);
        }
        public void Play(string id, bool value)
        {
            _animator.SetBool(stringToHash[id], value);
        }
        public void Play(string id, int value)
        {
            _animator.SetInteger(stringToHash[id], value);
        }
        public void Play(string id, float value)
        {
            _animator.SetFloat(stringToHash[id], value);
        }
    }
}