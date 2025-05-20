using Shark.Gameplay.Physics;
using Shark.Gameplay.WorldObjects;
using UnityEngine;

namespace Shark.Gameplay.Player
{
    public class CarController : MonoBehaviour, IPlayer
    {
        public CarInput CarInput { get; private set; }
        [field: SerializeField] public CarFuel CarFuel { get; private set; }
        [field: SerializeField] public CarStrength CarStrength { get; private set; }
        [field: SerializeField] public CarPhysics CarPhysics { get; private set; }
        [field: SerializeField] public CarSounds CarSounds { get; private set; }
        [field: SerializeField] public CarEffects CarEffects { get; private set; }

        [SerializeField, Min(0)] private float minAngle = 10f;
        [SerializeField, Min(0)] private float minSpeed = 3f;

        public bool IsBroken => CarStrength.IsBroken;
        public void Refuel(float value) => CarFuel.Refuel(value);
        public bool TakeDamage(float damage) => CarStrength.TakeDamage(damage);

        private TextureUnderWheelsCheker _textureChecker;
        private Rigidbody _rigidbody;


#if UNITY_EDITOR
        public CarPhysicsData carPhysics => CarPhysics.data;
        public WheelPhysicsData wheelPhysics => CarPhysics.wheels.wheelPhysics;
#endif


        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();

            CarInput = new();

            var skid = new Skid(_rigidbody, CarInput, minAngle, minSpeed);

            InitPhysics();
            InitStrength();
            InitFuel();
            InitSounds(skid);
            InitEffects(skid);
        }
        private void OnDestroy()
        {
            CarInput.Dispose();
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            if (!UnityEditor.EditorApplication.isPlaying)
                return;

            if (_rigidbody)
            {
                InitPhysics();
            }

            // вызов кидает ошибку т.к. OnValidate вызывается еще до Awake и ссылка не установлена
            // меня эта ошибка в консоли бесит), поэтому перехватываю 
            try
            {
                CarEffects?.UpdateValues();
            }
            catch (System.Exception)
            {
            }
        }
#endif
        private void InitPhysics()
        {
            CarPhysics.Init(
                _rigidbody,
                CarInput,
                _textureChecker = new TextureUnderWheelsCheker());
            CarPhysics.ApplyCarData();
        }
        private void InitStrength()
        {
            CarStrength.Init(this);
            CarStrength.SetStrengthMax();
            CarStrength.OnCarBroken += OnBroken;
        }
        private void InitFuel()
        {
            CarFuel.Init();
            CarFuel.SetFuelMax();
            CarFuel.OnCarFuelRanOut += OnOutOfFuel;
        }
        private void InitEffects(Skid skid)
        {
            CarEffects.Init(this,
                CarPhysics.speed,
                _textureChecker,
                () => skid.IsSkid());
        }
        private void InitSounds(Skid skid)
        {
            CarSounds.Init(this,
                GetComponent<AudioSource>(),
                CarFuel.CurrentFuel,
                () => skid.IsSkid());
        }

        void FixedUpdate()
        {
            CarFuel.Update(CarPhysics.Speed);
            CarPhysics.Update();
        }

        private void OnOutOfFuel()
        {
            CarInput.Enabled = false;
        }

        private void OnBroken()
        {
            CarInput.Enabled = false;
        }

        //
        // Debug
        //

        [ContextMenu("Death")]
        private void Death()
        {
            CarStrength.TakeDamage(float.MaxValue);
        }
        [ContextMenu("Fuel out")]
        private void FuelOut()
        {
            CarFuel.Update(float.MaxValue);
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

#if UNITY_EDITOR
        void OnDrawGizmos()
        {
            for (Wheels.Part wheelid = 0; wheelid < Wheels.Part.Count; ++wheelid)
            {
                //CarPhysics.wheels[wheelid].whellCollider.GetWorldPose(out var position, out var rotation);
                //Gizmos.DrawWireSphere(position, 0.1f);
            }
        }
#endif
    }
}