using System;
using Shark.Gameplay.Physics;
using Shark.Gameplay.WorldObjects;
using UnityEngine;

namespace Shark.Gameplay.Player
{
    public class CarController : MonoBehaviour, IPlayer
    {
        private const string INPUT_HORIZONTAL = "Horizontal";
        private const string INPUT_VERTICAL = "Vertical";

        private float _hInput;
        private float _frontWheelsRotation;
        public float vInput { get; private set; }
        private bool _isBreaking;

        private Rigidbody _rb;

        public event Action OnDamageReceived;
        public event Action OnCarFuelRanOut;
        public event Action OnCarBroken;
        private bool _endGameEventSended = false;

        [field: SerializeField]
        public float maxStrength { get; private set; } = 100;
        [field: SerializeField, HideInInspector]
        public float currentStrength { get; private set; }
        public bool IsBroken => currentStrength <= 0;
        private bool _isFuelOut;

        [field: SerializeField]
        public float fuelCapacity { get; private set; } = 100;

        [HideInInspector]
        public float currentFuel { get; private set; }

        [SerializeField]
        private float _fuelConsuptionMultiplier = 0.01f;

        [SerializeField]
        private float _fuelValueForActivateLowLevelFuelSound = 30f;

        [SerializeField]
        private SoundWhile _lowLevelFuelSound;

        [SerializeField]
        private float _frontWheelsRotationSpeed = .1f;

        [SerializeField]
        private Immunable immunable;

        [SerializeField]
        private CarPhysicsData _data;

        [SerializeField]
        private Wheel _wheels;

        [SerializeField] private TouchingSlidingSurfaceController _touchingSlidingSurface;

#if UNITY_EDITOR
        public CarPhysicsData carPhysics => _data;
        public WheelPhysicsData wheelPhysics => _wheels.wheelPhysics;
#endif

        private float _currentBreakForce;
        private float _currentSteerAngle;

        private float Speed => _rb.velocity.magnitude;
        public float SpeedKmh => Speed * 3.6f;
        public float SpeedMph => Speed * 2.23694f;

        private bool hasFuel => currentFuel > 0;
        private bool isDriving => vInput != 0;

        private void Start()
        {
            Initialize();
            Refuel(fuelCapacity);
            immunable.Init(this);

            _lowLevelFuelSound.Init(
                () => currentFuel <= _fuelValueForActivateLowLevelFuelSound,
                GetComponent<AudioSource>(),
                this);
        }

        private void OnValidate()
        {
            Initialize();
        }

        private void Initialize()
        {
            _rb = GetComponent<Rigidbody>();

            ApplyCarData();

            currentStrength = maxStrength;
        }

        private void ApplyCarData()
        {
            _rb.mass = _data.mass;
            _rb.centerOfMass = _data.centerOfMass;

            _rb.drag = _data.drag;
            _rb.angularDrag = _data.angularDrag;

            _wheels.ApplyWheelData();

#if UNITY_EDITOR
            Refuel(fuelCapacity);
#endif
        }

#if UNITY_EDITOR
        public void ApplyCarDataEditor()
        {
            ApplyCarData();
        }

        //
        // Debug
        //

        [ContextMenu("FuelOut")]
        private void FuelOut()
        {
            currentFuel = 0;
        }
        [ContextMenu("Death")]
        private void Death()
        {
            TakeDamage(float.MaxValue);
        }
#endif

        void FixedUpdate()
        {
            if (!_endGameEventSended)
            {
                HandleOutOfFuel();
                HandleBroken();
            }

            HandleInputOnFuelAndBrokenCheck();

            HandleMotor();
            HandleSteering();
            HandleFuelConsumption();

            ApplyMaterialPhysicsOnWheels();
            UpdateWheels();
        }
      
        void HandleInputOnFuelAndBrokenCheck()
        {
            if (IsBroken || _isFuelOut)
            {
                _hInput = 0;
                vInput = 0;

                return;
            }

            _hInput = Input.GetAxis(INPUT_HORIZONTAL);

            _frontWheelsRotation = Mathf.MoveTowards(_frontWheelsRotation, _hInput, _frontWheelsRotationSpeed);

            vInput = Input.GetAxis(INPUT_VERTICAL);
            _isBreaking = Input.GetKey(KeyCode.Space);
        }

        void HandleMotor()
        {
            ApplyDrive(vInput * _data.motorForce);
            ApplyBreaking();
        }

        void ApplyDrive(float motorTorque)
        {
            switch (_data.driveType)
            {
                case CarPhysicsData.CarDriveType.FrontWheelDrive: ApplyFWD(motorTorque); break;
                case CarPhysicsData.CarDriveType.RearWheelDrive: ApplyRWD(motorTorque); break;
                case CarPhysicsData.CarDriveType.AllWheelDrive: ApplyAWD(motorTorque); break;
            }
        }

        void ApplyFWD(float torque)
        {
            ApplyMotorTorque(_wheels[Wheel.Part.FL].whellCollider, torque);
            ApplyMotorTorque(_wheels[Wheel.Part.FR].whellCollider, torque);
        }

        void ApplyRWD(float torque)
        {
            ApplyMotorTorque(_wheels[Wheel.Part.RL].whellCollider, torque);
            ApplyMotorTorque(_wheels[Wheel.Part.RR].whellCollider, torque);
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
            return _isBreaking ? _data.breakForce : 0f;
        }

        private void ApplyBreaking()
        {
            _currentBreakForce = CalculateBreakForce();

            // торможение "ручником" применяется только к задним колесам для более правильной физики
            // когда применялось ко всем колесам, то машинка не реагировала на поворот руля при торможении

            _wheels[Wheel.Part.RL].whellCollider.brakeTorque = _currentBreakForce;
            _wheels[Wheel.Part.RR].whellCollider.brakeTorque = _currentBreakForce;
        }

        private void ApplyMaterialPhysicsOnWheels()
        {
            for (Wheel.Part wheelid = 0; wheelid < Wheel.Part.Count; ++wheelid)
            {
                ApplyPhysicsMaterialOnWheel(_wheels[wheelid], _wheels[wheelid].originalForwardStiffness, _wheels[wheelid].originalSidewaysStiffness);
            }
        }

        private void ApplyPhysicsMaterialOnWheel(Wheel.WheelData wheelData, float originalForwardStiffness, float originalSidewaysStiffness)
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
            for (Wheel.Part wheelid = 0; wheelid < Wheel.Part.Count; ++wheelid)
            {
                UpdateWheelPositionAndRotation(_wheels[wheelid]);
            }
        }

        private void UpdateWheelPositionAndRotation(Wheel.WheelData wheel)
        {
            wheel.whellCollider.GetWorldPose(out var position, out var rotation);
            wheel.transform.SetPositionAndRotation(position, rotation);
        }

        private void HandleSteering()
        {
            _currentSteerAngle = CalculateSteeringAngle();

            _wheels[Wheel.Part.FL].whellCollider.steerAngle = _currentSteerAngle;
            _wheels[Wheel.Part.FR].whellCollider.steerAngle = _currentSteerAngle;
        }

        private float CalculateSteeringAngle()
        {
            return _data.maxSteerAngle * _frontWheelsRotation;
        }

        private void HandleFuelConsumption()
        {
            CalculateFuelConsumption();
        }

        private void CalculateFuelConsumption()
        {
            currentFuel -= Speed * _fuelConsuptionMultiplier * Time.fixedDeltaTime;

            if (currentFuel < 0)
            {
                _isFuelOut = true;
            }
        }

        private void HandleOutOfFuel()
        {
            if (!hasFuel)
            {
                OnCarFuelRanOut?.Invoke();
                _endGameEventSended = true;
            }
        }

        private void HandleBroken()
        {
            if (IsBroken)
            {
                OnCarBroken?.Invoke();
                _endGameEventSended = true;

                _isBreaking = false;
                ApplyDrive(_hInput = 0);
                ApplyBreaking();
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out IWorldObject worldObject))
            {
                (worldObject as IActivatable)?.Activate();
                (worldObject as IPickupable)?.PickUp(this);
            }
        }

        private void OnCollisionEnter(Collision collision)
        {
            print(collision.gameObject.layer);
        }

        public void Refuel(float value)
        {
            currentFuel = Math.Min(currentFuel + value, fuelCapacity);
        }

        public bool TakeDamage(float damage)
        {
            if (immunable.IsImmunable)
                return false;

            immunable.MakeImmunable();

            currentStrength -= damage;
            OnDamageReceived?.Invoke();

            return true;
        }

#if UNITY_EDITOR
        void OnDrawGizmos()
        {
            for (Wheel.Part wheelid = 0; wheelid < Wheel.Part.Count; ++wheelid)
            {
                _wheels[wheelid].whellCollider.GetWorldPose(out var position, out var rotation);
                Gizmos.DrawWireSphere(position, 0.1f);
            }
        }
#endif
    }
}