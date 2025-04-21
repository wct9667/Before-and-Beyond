using UnityEngine;
using System.Collections;
using Player;

namespace Ability
{
    [CreateAssetMenu(menuName = "Player/Abilities/JoustAbility", fileName = "JoustAbilitySO")]
    public class JoustAbility : AbstractAbility
    {
        [Header("Ability Attributes")]
        [SerializeField] private LayerMask enemyLayerMask;

        [Header("Range Per Unit Fell")]
        [SerializeField] private float movementSpeedScaling = .5f;

        private GameObject joustObject;
        private GameObject playerGameObject;
        private Rigidbody playerRB;

        private GameObject sword;
        private Animator animSword;

        private Transform camera, player;
        private Animator anim;
        private RaycastHit hit;
        private RaycastHit enemyHit;

        private float startTime;

        private MonoBehaviour monoBehavior;

        public override void ActivateAbility()
        {
            Debug.Log("-----------------------Joust Activated");
            StartingAnimation();

        }

        public override void Setup()
        {
            camera = Camera.main.transform;

            playerGameObject = GameObject.FindGameObjectWithTag("Player");
            playerRB = playerGameObject.GetComponent<Rigidbody>();
            player = playerGameObject.transform;

            joustObject = GameObject.FindGameObjectWithTag("Joust");
            anim = joustObject.GetComponent<Animator>();

            sword = GameObject.FindGameObjectWithTag("Sword");
            animSword = sword.GetComponent<Animator>();

        }

        private void StartingAnimation()
        {
            animSword.SetTrigger("PutAway");
            anim.SetTrigger("StartJoust");
        }
    }
}
