using System;
using Scripts.Gameplay.Components;
using UnityEngine;

namespace Scripts.Gameplay.Enemys.Animations
{
    [Serializable]
    public class GoatAnimator
    {
        [SerializeField, Min(0)] private float idleAnimationSpeed = 1;
        [SerializeField, Min(0)] private float walkAnimationSpeed = 1;
        [SerializeField, Min(0)] private float attackAnimationSpeed = 1;

        private Animator animator;

        public void Init(Goat goat)
        {
            animator = goat.GetComponentInChildren<Animator>();

            PlayAnimationComponent playAnimation = new(animator);

            UpdateAnimationSpeed();

            goat.OnDamage += () => playAnimation.Play("attack");
            goat.OnIdle += () => playAnimation.Play("isWalking", false);
            goat.OnSeeking += () => playAnimation.Play("isWalking", true);
        }
#if UNITY_EDITOR
        public void UpdateValuesInEditor()
        {
            UpdateAnimationSpeed();
        }
#endif
        private void UpdateAnimationSpeed()
        {
            animator.SetFloat("idleAnimationSpeed", idleAnimationSpeed);
            animator.SetFloat("walkAnimationSpeed", walkAnimationSpeed);
            animator.SetFloat("attackAnimationSpeed", attackAnimationSpeed);
        }
    }
}