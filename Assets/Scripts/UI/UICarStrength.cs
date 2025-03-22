using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Shark.Gameplay.UI
{
    [Serializable]
    public class UICarStrength
    {
        [Serializable]
        private struct StrengthState
        {
            [SerializeField]
            public Texture texture;

            [SerializeField]
            public float checkpoint;
        }

        [SerializeField]
        private RawImage _strengthIcon;

        [SerializeField]
        private Transform _strengthArrowAnchor;

        [SerializeField]
        private Transform _strengthArrow;

        [SerializeField]
        private StrengthState[] _states;

        [Header("Settings"), SerializeField]
        public float _gearRotationSpeed = 4;

        [SerializeField]
        private float _arrowSpeed = 1;

        [SerializeField]
        private float _arrowMinRotation = -90;

        [SerializeField]
        private float _arrowMaxRotation = 90;

        private MonoBehaviour _monoBehaviour;
        private Coroutine _animRoutine;


        public void Init(MonoBehaviour monoBehaviour)
        {
            _monoBehaviour = monoBehaviour;
        }
        public void UpdateStrengthView(float strength, float maxStrength)
        {
            UpdateStrengthArrow(strength, maxStrength);
            UpdateStrengthIcon(strength);
        }
        public void UpdateStrengthRotation(float carSpeed)
        {
            if (_strengthIcon == null) return;

            _strengthIcon.transform.Rotate(new Vector3(0, 0, carSpeed * _gearRotationSpeed * Time.deltaTime));
        }
        private void UpdateStrengthArrow(float strength, float maxStrength)
        {
            var targetRotationZ = Mathf.Lerp(_arrowMaxRotation, _arrowMinRotation, strength / maxStrength);
            var targetRotation = Quaternion.Euler(0, 0, targetRotationZ);

            if (_animRoutine != null)
                _monoBehaviour.StopCoroutine(_animRoutine);
            _animRoutine = _monoBehaviour.StartCoroutine(AnimateStrengthArrowRotation(targetRotation));
        }
        private void UpdateStrengthIcon(float strength)
        {
            if (_strengthIcon == null || strength < 0) return;

            foreach (var state in _states)
            {
                if (strength >= state.checkpoint)
                {
                    _strengthIcon.texture = state.texture;
                    break;
                }
            }
            if (strength < 0) _gearRotationSpeed = 0;
        }
        private IEnumerator AnimateStrengthArrowRotation(Quaternion targetRotation)
        {
            var startRotation = _strengthArrowAnchor.rotation;
            float lerp = 0;

            do
            {
                lerp += Time.deltaTime * _arrowSpeed;
                _strengthArrowAnchor.rotation = Quaternion.Lerp(startRotation, targetRotation, lerp);

                yield return null;

            } while (lerp <= 1);

            _animRoutine = null;
        }
    }
}