using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

namespace Enemy
{
    public class Health : MonoBehaviour
    {
        [Header("Health")]
        [SerializeField] private float startingHealth;
        [SerializeField] private bool destroyOnDeath;
        [SerializeField] private float timeToDeath;

        //events for if there are animations or such on other components to trigger
        public UnityAction DamageEvent;
        public UnityAction<Health> DeathEvent;

        private float health;

        private void Start()
        {
            health = startingHealth;
        }

        //Removes Health
        public void SubtractHealth(float healthChange)
        {
            health -= healthChange;
            if (health <= 0)
            {
                DeathEvent.Invoke(this);
                if(destroyOnDeath) Destroy(gameObject, timeToDeath);
                return;
            }
            DamageEvent.Invoke();
        }

        /// <summary>
        /// Adds health
        /// </summary>
        /// <param name="health"></param>
        public void AddHealth(float health)
        {
            this.health += health;
            if (health >= startingHealth) health = startingHealth;
        }


    }
}
