using UnityEngine;
using System.Collections;
using Player;

namespace Ability
{
    [CreateAssetMenu(menuName = "Player/Abilities/ShieldAbility", fileName = "ShieldAbilitySO")]
    public class ShieldAbility : AbstractAbility
    {
        [Header("Ability Attributes")]
        [SerializeField] private float shieldRadius = 10.0f;
        [SerializeField] private GameObject shieldSphere;
        [SerializeField] private float shieldTime = 1.5f;

        private GameObject playerGameObject;
        private Rigidbody playerRB;

        private Transform player;

        private MonoBehaviour monoBehavior;

        public override void ActivateAbility()
        {
            ShieldStart();
        }

        public override void Setup()
        {
            playerGameObject = GameObject.FindGameObjectWithTag("Player");
            playerRB = playerGameObject.GetComponent<Rigidbody>();
            player = playerGameObject.transform;
        }

        public void ShieldStart()
        {
            ShieldSphereSpawn();
        }

        public void  ShieldSphereSpawn()
        {
            GameObject obj = Instantiate(shieldSphere, player);


            Destroy(obj, shieldTime);
        }


    }

}