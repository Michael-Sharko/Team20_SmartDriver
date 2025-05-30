using Shark.Gameplay.WorldObjects;
using UnityEngine;

[SelectionBase]
public class BreakTree : MonoBehaviour, IActivatable
{
    [SerializeField] private float fallAnimationSpeed = 0.3f;
    [SerializeField] private ManualSound fallSound;

    private Animator _animator;

    private void Awake()
    {
        _animator = GetComponent<Animator>();

        fallSound.Init(GetComponent<AudioSource>());

        _animator.SetFloat("fallAnimationSpeed", fallAnimationSpeed);
    }
    public void Activate()
    {
        _animator.SetTrigger("fall");
        fallSound.Play();
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        var rotation = Quaternion.LookRotation(-transform.forward);
        UnityEditor.Handles.ArrowHandleCap(0, transform.position, rotation, 7, EventType.Repaint);
    }
#endif
}