using Shark.Gameplay.WorldObjects;
using UnityEngine;

public class BreakTree : MonoBehaviour, IActivatable
{
    [SerializeField] private float fallAnimationSpeed = 0.3f;

    private Animator _animator;
    private bool _isTriggered;

    private void Awake()
    {
        _animator = GetComponent<Animator>();

        _animator.SetFloat("fallAnimationSpeed", fallAnimationSpeed);
    }
    public void Activate()
    {
        if (_isTriggered) return;

        _animator.SetTrigger("fall");
        _isTriggered = true;
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        var rotation = Quaternion.LookRotation(-transform.forward);
        UnityEditor.Handles.ArrowHandleCap(0, transform.position, rotation, 7, EventType.Repaint);
    }
#endif
}