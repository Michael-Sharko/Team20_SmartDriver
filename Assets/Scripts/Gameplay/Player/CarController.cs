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

        public bool IsBroken => CarStrength.IsBroken;
        public void Refuel(float value) => CarFuel.Refuel(value);
        public bool TakeDamage(float damage) => CarStrength.TakeDamage(damage);


#if UNITY_EDITOR
        public CarPhysicsData carPhysics => CarPhysics.data;
        public WheelPhysicsData wheelPhysics => CarPhysics.wheels.wheelPhysics;
#endif


        private void Start()
        {
            InitInput();
            InitPhysics();
            InitStrength();
            InitFuel();
            InitSounds();
        }

        private void OnValidate()
        {
            InitPhysics();
        }
        private void InitInput()
        {
            CarInput = new();
        }
        private void InitPhysics()
        {
            CarPhysics.Init(GetComponent<Rigidbody>(), CarInput);
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
            CarFuel.SetFuelMax();
            CarFuel.OnCarFuelRanOut += OnOutOfFuel;
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
                CarPhysics.wheels[wheelid].whellCollider.GetWorldPose(out var position, out var rotation);
                Gizmos.DrawWireSphere(position, 0.1f);
            }
        }
#endif
    }
}