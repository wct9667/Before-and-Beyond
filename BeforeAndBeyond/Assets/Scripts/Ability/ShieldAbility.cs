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

        private GameObject playerGameObject;
        private Rigidbody playerRB;

        private Transform player;

        private MonoBehaviour monoBehavior;

        public override void ActivateAbility()
        {

            Debug.Log("Shield");
            Debug.Log($"Activating Ability {name}");

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
            monoBehavior = FindObjectOfType<MonoBehaviour>();
            monoBehavior.StartCoroutine(ShieldSphereSpawn());
        }

        public IEnumerator ShieldSphereSpawn()
        {
            Instantiate(shieldSphere, player);


            GameObject.Destroy(GameObject.FindGameObjectWithTag("Shield"), 1f);

            yield return new WaitForSeconds(.1f);

        }

        public void ShieldHealthSap()
        {
            //fill out once we have damaging enemies and player health
        }

    }

}