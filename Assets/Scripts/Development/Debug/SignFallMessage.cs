using Scripts.Extension;
using Scripts.Gameplay.Components;
using UnityEngine;

namespace Scripts.Development.Debug
{
    public class SignFallMessage : MonoBehaviour
    {
        private SignBreakEvent @event;
        private void Awake()
        {
            @event = GetComponent<SignBreakEvent>();
            @event.onJointBreak.AddListener(Log);
        }
        private void OnDestroy()
        {
            @event.onJointBreak.RemoveListener(Log);
        }
        private void Log()
        {
            UnityEngine.Debug.Log($"Знак {gameObject.name.Color(TextColor.red)} упал!", gameObject);
        }
    }
}