using UnityEngine;
using System.Collections;
using Player;

namespace Ability
{
    [CreateAssetMenu(menuName = "Player/Abilities/ShieldAbility", fileName = "ShieldAbilitySO")]
    public class ShieldAbility : AbstractAbility
    {
        public override void ActivateAbility()
        {

            Debug.Log("Shield");
            Debug.Log($"Activating Ability {name}");

        }
    }

}