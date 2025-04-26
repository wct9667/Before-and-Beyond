using System;
using UnityEngine;

namespace Enemy
{
    [RequireComponent(typeof(Health))]
    [RequireComponent(typeof(Animator))]
    public class Animation : MonoBehaviour
    {
        private Health health;
        private Animator animator;

        private void Awake()
        {
            health = GetComponent<Health>();
            animator = GetComponent<Animator>();
        }

        private void OnEnable()
        {
            health.DamageEvent += TriggerDamageAnim;
            health.DeathEvent += TriggerDeathAnim;
        }
        
        private void OnDisable()
        {
            health.DamageEvent -= TriggerDamageAnim;
            health.DeathEvent -= TriggerDeathAnim;
        }

        private void TriggerDeathAnim(Health health)
        {
            animator.Play("DIE");
        }

        private void TriggerDamageAnim()
        {
            animator.Play("HURT");
        }
    }

}