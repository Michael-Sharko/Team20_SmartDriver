using Shark.Gameplay.Physics;
using System;
using System.Threading;
using UnityEngine;

namespace Shark.Gameplay.Player
{
    public class CarController : MonoBehaviour
    {
        public enum CarDriveType
        {
            FrontWheelDrive,
            RearWheelDrive,
            AllWheelDrive
        }

        private const string INPUT_HORIZONTAL = "Horizontal";
        private const string INPUT_VERTICAL = "Vertical";

        private float _hInput;
        private float _vInput;
        private bool _isBreaking;

        private Rigidbody _rb;

        [SerializeField]
        private CarPhysicsData _data;

        [SerializeField]
        private Wheel _wheels;

#if UNITY_EDITOR
        public CarPhysicsData carPhysics => _data;
        public WheelPhysicsData wheelPhysics => _wheels.wheelPhysics;
#endif

        private float _currentBreakForce;
        private float _currentSteerAngle;

        private float Speed => _rb.velocity.magnitude;
        public float SpeedKmh => Speed * 3.6f;
        public float SpeedMph => Speed * 2.23694f;

        private void Start()
        {
            Initialize();
        }

        private void OnValidate()
        {
            Initialize();
        }

        private void Initialize()
        {
            _rb = GetComponent<Rigidbody>();

            ApplyCarData();
        }

        private void ApplyCarData()
        {
            _rb.mass = _data.mass;
            _rb.drag = _data.drag;
            _rb.angularDrag = _data.angularDrag;

            _wheels.ApplyWheelData();
        }

        void FixedUpdate()
        {
            HandleInput();
            HandleMotor();
            HandleSteering();
            UpdateWheels();
        }

        void HandleInput()
        {
            _hInput = Input.GetAxis(INPUT_HORIZONTAL);
            _vInput = Input.GetAxis(INPUT_VERTICAL);
            _isBreaking = Input.GetKey(KeyCode.Space);
        }

        void HandleMotor()
        {
            ApplyDrive(_vInput * _data.motorForce);
            ApplyBrakingIfPressed();
        }

        void ApplyDrive(float motorTorque)
        {
            switch (_data.driveType)
            {
            case CarDriveType.FrontWheelDrive: ApplyFWD(motorTorque); break;
            case CarDriveType.RearWheelDrive: ApplyRWD(motorTorque); break;
            case CarDriveType.AllWheelDrive: ApplyAWD(motorTorque); break;
            }
        }

        void ApplyFWD(float torque)
        {
            ApplyMotorTorque(_wheels[Wheel.Part.FL].collider, torque);
            ApplyMotorTorque(_wheels[Wheel.Part.FR].collider, torque);
        }

        void ApplyRWD(float torque)
        {
            ApplyMotorTorque(_wheels[Wheel.Part.RL].collider, torque);
            ApplyMotorTorque(_wheels[Wheel.Part.RR].collider, torque);
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

        private void ApplyBrakingIfPressed()
        {
            if (_isBreaking)
            {
                CalculateBreakForce();
                ApplyBreaking();
            }
        }

        private void CalculateBreakForce()
        {
            _currentBreakForce = _isBreaking ? _data.breakForce : 0f;
        }

        private void ApplyBreaking()
        {
            for (Wheel.Part wheelid = 0; wheelid < Wheel.Part.Count; ++wheelid)
            {
                _wheels[wheelid].collider.brakeTorque = _currentBreakForce;
            }
        }

        private void UpdateWheels()
        {
            for (Wheel.Part wheelid = 0; wheelid < Wheel.Part.Count; ++wheelid)
            {
                UpdateWheelPositionAndRotation(_wheels[wheelid]);
            }
        }

        private void UpdateWheelPositionAndRotation(Wheel.WheelData wheel)
        {
            wheel.collider.GetWorldPose(out var position, out var rotation);
            wheel.transform.SetPositionAndRotation(position, rotation);
        }

        private void HandleSteering()
        {
            CalculateSteeringAngle();

            _wheels[Wheel.Part.FL].collider.steerAngle = _currentSteerAngle;
            _wheels[Wheel.Part.FR].collider.steerAngle = _currentSteerAngle;
        }

        private void CalculateSteeringAngle()
        {
            _currentSteerAngle = _data.maxSteerAngle * _hInput;
        }
    }
}