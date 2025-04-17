using UnityEngine;

namespace Shark.Gameplay.Player
{
    public class Skid
    {
        private Rigidbody _rigidbody;
        private CarInput _input;

        private Vector3 _physicMovement;

        public Skid(Rigidbody rigidbody, CarInput input)
        {
            _rigidbody = rigidbody;
            _input = input;
        }
        public bool IsSkid(float minAngle, float minSpeed)
        {
            if (IsCarBraking)
                return true;

            CalculatePhysicMovement();

            if (IsLowSpeed(minSpeed))
                return false;

            var angle = GetDeltaAngle();

            return angle > minAngle;
        }
        private bool IsCarBraking => _input.spaceInput;
        private bool IsLowSpeed(float minSpeed) => _physicMovement.magnitude < minSpeed;
        private float GetDeltaAngle()
        {
            var directionForwardCar = CalculateForwardCar();

            _physicMovement.Normalize();

            return Vector3.Angle(directionForwardCar, _physicMovement);
        }
        private Vector3 CalculateForwardCar()
        {
            var input = Mathf.Sign(_input.vInput);
            //Debug.Log(input);
            var directionForwardCar = _rigidbody.transform.forward * input;
            directionForwardCar.y = 0f;

            return directionForwardCar.normalized;
        }
        private void CalculatePhysicMovement()
        {
            _physicMovement = _rigidbody.velocity;
            _physicMovement.y = 0f;
        }
    }
}