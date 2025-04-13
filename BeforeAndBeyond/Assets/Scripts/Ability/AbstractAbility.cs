using System;
using UnityEngine;

namespace Ability
{
    [CreateAssetMenu(menuName = "Player/Abilities")]
    public class AbstractAbility : ScriptableObject
    {
        [SerializeField] private Sprite image;
        public Sprite Image => image;

        [SerializeField] private float abilityCooldown;
        public float AbilityCooldown => abilityCooldown;
        
        /// <summary>
        /// Activates the ability
        /// </summary>
        public virtual void ActivateAbility()
        {
            Debug.Log($"Activating Ability {name}");
        }

        /// <summary>
        /// Setups up the SO
        /// </summary>
        protected virtual void Setup()
        {
            
        }

        public void OnEnable()
        {
          Setup();
        }
    }

}
