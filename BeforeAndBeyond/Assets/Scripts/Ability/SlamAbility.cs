using UnityEngine;
using System.Collections;

namespace Ability
{
    [CreateAssetMenu(menuName = "Player/Abilities/SlamAbility", fileName = "SlamAbilitySO")]
    public class SlamAbility : AbstractAbility
    {
        [Header("Ability Attributes")]
        [SerializeField] private LayerMask enemyLayerMask;
        [SerializeField] private LayerMask groundLayerMask;
        [SerializeField] private float nonRefundedCooldown = 2;

        [Header("Range Per Unit Fell")]
        [SerializeField] private float scalePerUnit = .5f;

        private GameObject sword;
        private GameObject playerGameObject;
        private Rigidbody playerRB;

        private float raycastDist;

        private Transform camera, player;
        private Animator anim;
        private RaycastHit hit;
        private RaycastHit enemyHit;

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

            if(!Physics.Raycast(player.transform.position, player.TransformDirection(Vector3.down), 5f))
            {
                if (Physics.Raycast(player.transform.position, player.TransformDirection(Vector3.down), out hit, Mathf.Infinity, groundLayerMask))
                {

                    this.AbilityCooldown = nonRefundedCooldown;
                    Debug.Log("-----------------------Did Slam: " + hit.point);

                    raycastDist = hit.distance;
                    Debug.Log("-----------------------Value: " + scalePerUnit * raycastDist);

                    anim.SetTrigger("Slam");
                    monoBehavior.StartCoroutine(SlamMovement());
                }
                else
                {
                    this.AbilityCooldown = 0;

                    Debug.Log("-----------------------Did not Slam");
                }
            }
            else
            {
                this.AbilityCooldown = 0;

                Debug.Log("-----------------------Did not Slam");
            }


        }

        private IEnumerator SlamMovement()
        {
            playerRB.AddForce(0, 15f, 0, ForceMode.Impulse);

            yield return new WaitForSeconds(.25f);

            startTime = Time.time;

            while (Vector3.Distance(player.transform.position, hit.point) > 1.0f)
            {
                player.transform.position = Vector3.Lerp(player.transform.position, hit.point, (Time.time - startTime) * 0.85f);
                yield return null;

            }

            HitTarget();
            playerRB.velocity = new Vector3(playerRB.velocity.x, 0.0f, playerRB.velocity.z);

            monoBehavior.StartCoroutine(DrawEffect());
            yield return new WaitForSeconds(.25f);
            anim.SetTrigger("SlamEnd");

            yield return new WaitForSeconds(.1f);

        }

        private void HitTarget()
        {
            //replace with actually dealing damage


            Collider[] enemiesDetected = Physics.OverlapSphere(player.transform.position, scalePerUnit * raycastDist, enemyLayerMask);
            foreach (var enemy in enemiesDetected)
            {
                Debug.Log("Enemy in Range at: " + enemy.transform.position);
                Destroy(enemy.transform.gameObject, .25f);
            }
        }

        private IEnumerator DrawEffect()
        {
            GameObject circle = new GameObject();
            LineRenderer drawLine;

            drawLine = circle.AddComponent<LineRenderer>();
            drawLine.material = new Material(Shader.Find("Sprites/Default"));

            drawLine.positionCount = 51;
            drawLine.useWorldSpace = false;

            float x;
            float z;

            float angle = 20f;

            Color newColor = new Vector4(1f, 1 - raycastDist/33f, 0f, .5f);

            drawLine.startColor = newColor;
            drawLine.endColor = newColor;

            drawLine.startWidth = raycastDist/10;
            drawLine.endWidth = raycastDist/10;

            for (float j = 0; j < scalePerUnit * raycastDist; j = j + (scalePerUnit * raycastDist / 80f))
            {
                for (int i = 0; i < (51); i++)
                {
                    if(drawLine != null)
                    {
                        x = Mathf.Sin(Mathf.Deg2Rad * angle) * j;
                        z = Mathf.Cos(Mathf.Deg2Rad * angle) * j;

                        drawLine.SetPosition(i, new Vector3(player.transform.position.x + x, player.transform.position.y, player.transform.position.z + z));

                        angle += (390f / 51);
                    }
                }
                yield return new WaitForSeconds(.0015f);
                GameObject.Destroy(circle, .2f);

            }
            yield return new WaitForSeconds(.1f);

        }
    }
}
