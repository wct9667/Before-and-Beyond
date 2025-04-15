using UnityEngine;
using System.Collections;

namespace Ability
{
    [CreateAssetMenu(menuName = "Player/Abilities/SlamAbility", fileName = "SlamAbilitySO")]
    public class SlamAbility : AbstractAbility
    {
        [Header("Ability Attributes")]
        [SerializeField] private float slamRadius = 8.0f;
        [SerializeField] private LayerMask enemyLayerMask;
        [SerializeField] private LayerMask groundLayerMask;
        [SerializeField] private float nonRefundedCooldown = 2;

        private GameObject sword;
        private GameObject playerGameObject;
        private Rigidbody playerRB;

        private Transform camera, player;
        private Animator anim;
        private RaycastHit hit;

        private float startTime;

        private MonoBehaviour monoBehavior;

        public override void ActivateAbility()
        {
            Debug.Log("Slam");
            Debug.Log($"Activating Ability {name}");

            SlamStart();
        }

        public override void Setup()
        {
            camera = Camera.main.transform;

            playerGameObject = GameObject.FindGameObjectWithTag("Player");
            playerRB = playerGameObject.GetComponent<Rigidbody>();
            player = playerGameObject.transform;

            sword = GameObject.FindGameObjectWithTag("Sword");
            anim = sword.GetComponent<Animator>();
        }

        private void SlamStart()
        {
            monoBehavior = FindObjectOfType<MonoBehaviour>();

            if (Physics.Raycast(player.transform.position, player.TransformDirection(Vector3.down), out hit, Mathf.Infinity, groundLayerMask))
            {

                this.AbilityCooldown = nonRefundedCooldown;
                Debug.DrawRay(camera.position, camera.TransformDirection(Vector3.forward) * hit.distance, Color.yellow);
                Debug.Log("-----------------------Did Slam: " + hit.point);

                anim.SetTrigger("Slam");
                monoBehavior.StartCoroutine(SlamMovement());
            }
            else
            {
                this.AbilityCooldown = 0;

                Debug.DrawRay(camera.position, camera.TransformDirection(Vector3.forward) * 1000, Color.white);
                Debug.Log("-----------------------Did not Slam");
            }
        }

        private IEnumerator SlamMovement()
        {

            while (Vector3.Distance(player.transform.position, hit.point) > 0.2f)
            {
                player.transform.position = Vector3.Lerp(player.transform.position, hit.point, (Time.time - startTime));

                anim.SetTrigger("SlamEnd");
                HitTarget();

                yield return null;

            }
            yield return new WaitForSeconds(2f);
        }

        private void HitTarget()
        {
            Debug.Log("Enemy in Range");

            //replace with actually dealing damage
            //Destroy(hit.transform.gameObject, .25f);
        }
    }
}
