using UnityEngine;

namespace Ability
{
    [CreateAssetMenu(menuName = "Player/Abilities/GrappleHookAbility", fileName = "GrappleHookAbilitySO")]
    public class GrappleHookAbility : AbstractAbility 
    {
        public override void ActivateAbility()
        {
            Debug.Log("Grapple");
            Debug.Log($"Activating Ability {name}");
        }
    }
}