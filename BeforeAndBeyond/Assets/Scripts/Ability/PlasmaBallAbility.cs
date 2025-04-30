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
            
        }

        public override void Setup()
        {
            
        }

        public void BallThrow()
        {
            GameObject obj = Instantiate(plasmaBall, new Vector3(player.transform.position.x, player.transform.position.y, player.transform.position.z), Quaternion.Euler(0, 0, 0));
            Rigidbody rb = obj.GetComponent<Rigidbody>();


            rb.AddForce(camera.TransformDirection(Vector3.forward) * throwForce);
            rb.AddForce(player.TransformDirection(Vector3.up) * (throwForce * 0.5f));
        }
    }
}
