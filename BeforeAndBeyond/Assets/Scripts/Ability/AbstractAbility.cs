using System;
using UnityEngine;
using UnityEngine.Events;

namespace Ability
{
    [CreateAssetMenu(menuName = "Player/Abilities")]
    public class AbstractAbility : ScriptableObject
    {
        
        [Header("Overall Attributes")]
        [SerializeField] protected Sprite image;
        [SerializeField] protected float abilityCooldown;

        [Header("Charges")]
        [SerializeField] protected float rechargeTimer = 0f;
        [SerializeField] protected int maxCharges;
        public int MaxCharges => maxCharges;
        [SerializeField] protected int charges;

        
        [SerializeField] protected bool usesCharges = false;
        public bool UsesCharges => usesCharges;
        public int Charges => charges;
       
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
        /// Decrements charge, returns true if failed check or true if its good
        /// </summary>
        /// <returns>True if not using charges and if enough charges, false otherwise</returns>
        public bool UpdateAndCheckCharges()
        {
            if (!usesCharges) return true;
            if (charges <= 0) return false;
            charges--;
            ChargeUpdated.Invoke();
            return true;
        }

        /// <summary>
        /// Setups up the SO
        /// </summary>
        public virtual void Setup()
        {
            
        }

        public UnityEvent ChargeUpdated;
        
        public void TickRecharge(float deltaTime)
        {
            if (!usesCharges) return;
            if (charges >= maxCharges) return;

            rechargeTimer += deltaTime;

            if (rechargeTimer >= abilityCooldown)
            {
                rechargeTimer = 0f;
                charges++;
                ChargeUpdated.Invoke();
                Debug.Log($"Recharge complete for {name}. Charges: {charges}");
            }
        }
        
        
        private void OnEnable()
        {
            // Reset runtime values when the game starts or asset is reloaded
            charges = maxCharges;
            rechargeTimer = 0f;
        }

    }

}
