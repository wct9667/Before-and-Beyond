using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enemy;

namespace Ability
{
    [CreateAssetMenu(menuName = "Player/Abilities/HammerAbility", fileName = "HammerAbilitySO")]
    public class HammerAbility : AbstractAbility
    {
        [Header("Ability Attributes")]
        [SerializeField] private float attackReach = 2f;

        public override void ActivateAbility()
        {
            Debug.Log("Hammer");
        }

        public override void Setup()
        {
            
        }

    }
}
