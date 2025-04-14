using System;
using UnityEngine;

namespace Ability
{
    [CreateAssetMenu(menuName = "Player/Abilities")]
    public class AbstractAbility : ScriptableObject
    {
        
        [Header("Overall Attributes")]
        [SerializeField] private Sprite image;
        [SerializeField] private float abilityCooldown;
       
        public Sprite Image => image;
        public float AbilityCooldown 
        {
            get { return abilityCooldown; }  
            set { abilityCooldown = value; }  
        }

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
        public virtual void Setup()
        {
            
        }

    }

}
