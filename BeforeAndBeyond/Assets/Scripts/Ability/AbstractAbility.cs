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
        
        //TODO add needed overrides here so any needed data can get passed through
    }

}
