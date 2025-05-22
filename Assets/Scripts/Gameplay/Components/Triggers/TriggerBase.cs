using Shark.Gameplay.WorldObjects;
using UnityEngine;

namespace Scripts.Gameplay.Components
{
    public abstract class TriggerBase : MonoBehaviour, IActivatable
    {
        public bool ShowVisual { get; set; } = true;

        [SerializeField] private GameObject[] triggered;

        private bool wasTriggerd;


        public void Activate()
        {
            if (wasTriggerd) return;

            wasTriggerd = true;

            foreach (var obj in triggered)
            {
                if (obj.TryGetComponent(out IActivatable activatable))
                {
                    activatable.Activate();

                    Debug.Log($"Gameobject {obj.name} был стриггерен объектом {gameObject.name}!", this);
                }
                else
                {
                    Debug.LogError($"Gameobject {obj.name} не имеет реакции на триггер!", this);
                }
            }
        }
        protected abstract void ForbitChangingColliderSizeAndCenter();

#if !UNITY_EDITOR
    private void Awake()
    {
        DestroyVisual(); 
    }

    private void DestroyVisual()
    {
        if (TryGetComponent(out MeshRenderer renderer))
        {
            Destroy(renderer);
        }
        if (TryGetComponent(out MeshFilter filter))
        {
            Destroy(filter);
        }
    }
#endif
#if UNITY_EDITOR
        protected virtual void DrawGizmos() { }
        private void OnDrawGizmos()
        {
            // вызывается в OnDrawGizmos, т.к. OnValidate не вызывается при изменении размера коллайдера
            // а нужно чтобы сразу значение не обновлялось
            ForbitChangingColliderSizeAndCenter();

            if (!ShowVisual)
                return;

            DrawGizmos();

            if (wasTriggerd || triggered == null || triggered.Length == 0)
            {
                return;
            }

            Gizmos.color = Color.red;

            foreach (var obj in triggered)
            {
                if (!obj)
                    continue;

                Gizmos.DrawLine(transform.position, obj.transform.position);
            }
        }
#endif
    }
}