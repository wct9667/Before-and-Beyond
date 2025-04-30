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
        [SerializeField] private LayerMask enemyLayerMask;

        private GameObject hammer;

        private Transform camera;
        private Animator anim;

        private GameObject sword;
        private Animator animSword;

        public override void ActivateAbility()
        {
            Debug.Log("Hammer");
            animSword.SetTrigger("PutAway");
            anim.SetTrigger("HammerSwing");
            animSword.SetTrigger("TakeOut");

        }

        public override void Setup()
        {
            camera = Camera.main.transform;
            hammer = GameObject.FindGameObjectWithTag("Hammer");
            anim = hammer.GetComponent<Animator>();

            sword = GameObject.FindGameObjectWithTag("Sword");
            animSword = sword.GetComponent<Animator>();
        }

    }
}
