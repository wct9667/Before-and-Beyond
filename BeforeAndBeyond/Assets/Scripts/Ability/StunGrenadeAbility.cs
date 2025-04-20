using UnityEngine;
using System.Collections;
using Player;

namespace Ability
{
    [CreateAssetMenu(menuName = "Player/Abilities/StunGrenadeAbility", fileName = "StunGrenadeAbilitySO")]
    public class StunGrenadeAbility : AbstractAbility
    {
        [Header("Ability Attributes")]
        [SerializeField] private float throwForce = 100.0f;
        [SerializeField] private GameObject stunGrenade;

        private GameObject playerGameObject;
        private Rigidbody playerRB;

        private Transform player, camera;


        public override void ActivateAbility()
        {
            Debug.Log("-----------------------Stun Grenade Activated");
            GrenadeThrow();
        }

        public override void Setup()
        {
            camera = Camera.main.transform;

            playerGameObject = GameObject.FindGameObjectWithTag("Player");
            playerRB = playerGameObject.GetComponent<Rigidbody>();
            player = playerGameObject.transform;
        }

        public void GrenadeThrow()
        {
            GameObject obj = Instantiate(stunGrenade, new Vector3(player.transform.position.x, player.transform.position.y, player.transform.position.z), Quaternion.Euler(0, 0, 0));
            Rigidbody rb = obj.GetComponent<Rigidbody>();


            rb.AddForce(camera.TransformDirection(Vector3.forward) * throwForce);
            rb.AddForce(player.TransformDirection(Vector3.up) * (throwForce * 0.5f));
        }
    }
}
