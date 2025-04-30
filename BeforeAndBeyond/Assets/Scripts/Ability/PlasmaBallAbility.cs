using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Player;

namespace Ability
{
    [CreateAssetMenu(menuName = "Player/Abilities/PlasmaBallAbility", fileName = "PlasmaBallAbilitySO")]
    public class PlasmaBallAbility : AbstractAbility
    {
        [Header("Ability Attributes")]
        [SerializeField] private float throwForce = 100.0f;
        [SerializeField] private GameObject plasmaBall;

        private GameObject playerGameObject;
        private Rigidbody playerRB;

        private Transform player, camera;


        public override void ActivateAbility()
        {
            Debug.Log("Plasma");
            BallThrow();
            
        }

        public override void Setup()
        {
            camera = Camera.main.transform;

            playerGameObject = GameObject.FindGameObjectWithTag("Player");
            playerRB = playerGameObject.GetComponent<Rigidbody>();
            player = playerGameObject.transform;
        }

        public void BallThrow()
        {
            GameObject obj = Instantiate(plasmaBall, player.transform.position + camera.transform.forward, Quaternion.Euler(0, 0, 0));
            Rigidbody rb = obj.GetComponent<Rigidbody>();


            rb.AddForce(player.TransformDirection(Vector3.up) * throwForce);
        }
    }
}
