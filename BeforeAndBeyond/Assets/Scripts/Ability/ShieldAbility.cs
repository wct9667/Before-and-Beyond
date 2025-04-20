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
            GameObject obj = Instantiate(shieldSphere, player);


            Destroy(obj, 1f);

            yield return new WaitForSeconds(.1f);

        }

        public void ShieldHealthSap(float amtTaken)
        {
            //fill out once we have damaging enemies and player health
        }

    }

}