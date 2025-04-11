using UnityEngine;

namespace Ability
{
    [CreateAssetMenu(menuName = "Player/Abilities/SwordAbility", fileName = "SwordAbilitySO")]
    public class SwordAbility : AbstractAbility
    {
        public override void ActivateAbility()
        {
            Debug.Log("Sword");
            Debug.Log($"Activating Ability {name}");
        }
    }
}