using UnityEngine;

namespace Shark.Gameplay.Player
{
    public class Skid
    {
        private readonly Rigidbody _rigidbody;
        private readonly CarInput _input;
        private readonly float _minAngle;
        private readonly float _minSpeed;

        private Vector3 _physicMovement;

        private bool IsCarBraking => _input.spaceInput;
        private bool IsLowSpeed => _physicMovement.magnitude < _minSpeed;


        public Skid(Rigidbody rigidbody, CarInput input, float minAngle, float minSpeed)
        {
            _rigidbody = rigidbody;
            _input = input;
            _minAngle = minAngle;
            _minSpeed = minSpeed;
        }
        public bool IsSkid()
        {
            CalculatePhysicMovement();

            if (IsLowSpeed)
            {
                return false;
            }

            if (IsCarBraking)
                return true;
            
            var angle = GetDeltaAngle();

            return angle > _minAngle;
        }
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