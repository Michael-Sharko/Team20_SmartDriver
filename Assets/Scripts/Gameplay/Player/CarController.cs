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

        public bool IsBroken => CarStrength.IsBroken;
        public void Refuel(float value) => CarFuel.Refuel(value);
        public bool TakeDamage(float damage) => CarStrength.TakeDamage(damage);

        private TextureUnderWheelsCheker _textureChecker;


#if UNITY_EDITOR
        public CarPhysicsData carPhysics => CarPhysics.data;
        public WheelPhysicsData wheelPhysics => CarPhysics.wheels.wheelPhysics;
#endif


        private void Awake()
        {
            InitInput();
            InitPhysics();
            InitStrength();
            InitFuel();
            InitSounds();
            InitEffects();
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            if (!UnityEditor.EditorApplication.isPlaying)
                return;

            InitPhysics();

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
        private void InitInput()
        {
            CarInput = new();
        }
        private void InitPhysics()
        {
            CarPhysics.Init(
                GetComponent<Rigidbody>(),
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
        private void InitEffects()
        {
            CarEffects.Init(this, CarPhysics.speed, _textureChecker);
        }
        private void InitSounds()
        {
            CarSounds.Init(this,
                GetComponent<AudioSource>(),
                CarFuel.CurrentFuel);
        }

        void FixedUpdate()
        {
            CarInput.Update();
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