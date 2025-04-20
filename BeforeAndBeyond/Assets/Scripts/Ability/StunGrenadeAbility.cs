using UnityEngine;
using System.Collections;
using Player;

namespace Ability
{
    [CreateAssetMenu(menuName = "Player/Abilities/StunGrenadeAbility", fileName = "StunGrenadeAbilitySO")]
    public class StunGrenadeAbility : AbstractAbility
    {
        public override void ActivateAbility()
        {
            Debug.Log("-----------------------Stun Grenade Activated");
        }
    }
}
