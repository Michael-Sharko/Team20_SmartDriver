using System;
using Shark.Gameplay.Physics;
using UnityEngine;

namespace Shark.Gameplay.Player
{
    // тут уже не знаю, стоит ли разбивать этот класс на более мелкие
    [Serializable]
    public class CarPhysics
    {
        [SerializeField] private float _frontWheelsRotationSpeed = .1f;
        [field: SerializeField] public Wheels wheels { get; private set; }
        [field: SerializeField] public CarPhysicsData data { get; private set; }

        [SerializeField] private TouchingSlidingSurfaceController _touchingSlidingSurface;

        private Rigidbody _rb;
        private CarInput _carInput;

        private float _frontWheelsRotation;
        private float _currentBreakForce;
        private float _currentSteerAngle;

        public float Speed => _rb.velocity.magnitude;
        public float SpeedKmh => Speed * 3.6f;
        public float SpeedMph => Speed * 2.23694f;


        public void Init(Rigidbody rigidbody, CarInput carInput)
        {
            _rb = rigidbody;
            _carInput = carInput;
        }
        public void ApplyCarData()
        {
            _rb.mass = data.mass;
            _rb.centerOfMass = data.centerOfMass;

            _rb.drag = data.drag;
            _rb.angularDrag = data.angularDrag;

            wheels.ApplyWheelData();
        }
        public void Update()
        {
            RotateForwardWheels(_carInput.hInput);

            HandleMotor();
            HandleSteering();

            ApplyMaterialPhysicsOnWheels();
            UpdateWheels();
        }
        private void RotateForwardWheels(float horizontalInput)
        {
            _frontWheelsRotation = Mathf.MoveTowards(_frontWheelsRotation, horizontalInput, _frontWheelsRotationSpeed);
        }

        void HandleMotor()
        {
            ApplyDrive(_carInput.vInput * data.motorForce);
            ApplyBreaking();
        }

        void ApplyDrive(float motorTorque)
        {
            switch (data.driveType)
            {
                case CarPhysicsData.CarDriveType.FrontWheelDrive: ApplyFWD(motorTorque); break;
                case CarPhysicsData.CarDriveType.RearWheelDrive: ApplyRWD(motorTorque); break;
                case CarPhysicsData.CarDriveType.AllWheelDrive: ApplyAWD(motorTorque); break;
            }
        }

        void ApplyFWD(float torque)
        {
            ApplyMotorTorque(wheels[Wheels.Part.FL].whellCollider, torque);
            ApplyMotorTorque(wheels[Wheels.Part.FR].whellCollider, torque);
        }

        void ApplyRWD(float torque)
        {
            ApplyMotorTorque(wheels[Wheels.Part.RL].whellCollider, torque);
            ApplyMotorTorque(wheels[Wheels.Part.RR].whellCollider, torque);
        }

        void ApplyAWD(float torque)
        {
            ApplyFWD(torque * 0.5f);
            ApplyRWD(torque * 0.5f);
        }

        void ApplyMotorTorque(WheelCollider wheel, float torque)
        {
            wheel.motorTorque = torque;
        }

        private float CalculateBreakForce()
        {
            return _carInput.spaceInput ? data.breakForce : 0f;
        }

        private void ApplyBreaking()
        {
            _currentBreakForce = CalculateBreakForce();

            // торможение "ручником" применяется только к задним колесам для более правильной физики
            // когда применялось ко всем колесам, то машинка не реагировала на поворот руля при торможении

            wheels[Wheels.Part.RL].whellCollider.brakeTorque = _currentBreakForce;
            wheels[Wheels.Part.RR].whellCollider.brakeTorque = _currentBreakForce;
        }
        private void ApplyMaterialPhysicsOnWheels()
        {
            for (Wheels.Part wheelid = 0; wheelid < Wheels.Part.Count; ++wheelid)
            {
                ApplyPhysicsMaterialOnWheel(wheels[wheelid], wheels[wheelid].originalForwardStiffness, wheels[wheelid].originalSidewaysStiffness);
            }
        }

        private void ApplyPhysicsMaterialOnWheel(Wheels.WheelData wheelData, float originalForwardStiffness, float originalSidewaysStiffness)
        {
            var collider = wheelData.whellCollider;

            if (!collider.GetGroundHit(out WheelHit hit))
                return;

            var forwardFrictionStiffness = hit.collider.material.staticFriction * originalForwardStiffness;
            var sidewaysFrictionStiffness = hit.collider.material.staticFriction * originalSidewaysStiffness;

            if (_touchingSlidingSurface.TryCalculateSlidingToWheel(wheelData, hit, out var stiffness))
            {

                forwardFrictionStiffness = stiffness.forwardFrictionStiffness;
                sidewaysFrictionStiffness = stiffness.sidewaysFrictionStiffness;

            }

            WheelFrictionCurve forwardFriction = collider.forwardFriction;
            forwardFriction.stiffness = forwardFrictionStiffness;
            collider.forwardFriction = forwardFriction;

            WheelFrictionCurve sidewaysFriction = collider.sidewaysFriction;
            sidewaysFriction.stiffness = sidewaysFrictionStiffness;
            collider.sidewaysFriction = sidewaysFriction;

        }

        private void UpdateWheels()
        {
            for (Wheels.Part wheelid = 0; wheelid < Wheels.Part.Count; ++wheelid)
            {
                UpdateWheelPositionAndRotation(wheels[wheelid]);
            }
        }

        private void UpdateWheelPositionAndRotation(Wheels.WheelData wheel)
        {
            wheel.whellCollider.GetWorldPose(out var position, out var rotation);
            wheel.transform.SetPositionAndRotation(position, rotation);
        }

        private void HandleSteering()
        {
            _currentSteerAngle = CalculateSteeringAngle();

            wheels[Wheels.Part.FL].whellCollider.steerAngle = _currentSteerAngle;
            wheels[Wheels.Part.FR].whellCollider.steerAngle = _currentSteerAngle;
        }

        private float CalculateSteeringAngle()
        {
            return data.maxSteerAngle * _frontWheelsRotation;
        }
    }
}