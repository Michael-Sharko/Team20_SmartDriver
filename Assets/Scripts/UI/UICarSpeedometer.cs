using System;
using UnityEngine;

namespace Shark.Gameplay.UI
{
    [Serializable]
    public class UICarSpeedometer
    {
        [SerializeField] private RectTransform _arrowAnchor;
        [SerializeField, Range(0, 1)] private float _arrowRotationSmoothness = 0.05f;
        [SerializeField] private float _minRotation = 101.5f;
        [SerializeField] private float _maxRotation = -101.5f;

        public void Update(float speed)
        {
            var rotationZ = Mathf.Lerp(_minRotation, _maxRotation, speed / 180);

            var targetRotation = Quaternion.Euler(0, 0, rotationZ);
            var lerpedRotation = Quaternion.Lerp(_arrowAnchor.rotation, targetRotation, _arrowRotationSmoothness);
            _arrowAnchor.rotation = lerpedRotation;
        }
    }
}