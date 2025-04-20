using UnityEngine;
using System.Collections;
using Player;

namespace Ability
{
    [CreateAssetMenu(menuName = "Player/Abilities/JoustAbility", fileName = "JoustAbilitySO")]
    public class JoustAbility : AbstractAbility
    {
        public override void ActivateAbility()
        {
            Debug.Log("-----------------------Joust Activated");
        }
    }
}
