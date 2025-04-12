using UnityEngine;
using System.Collections;
namespace Ability
{
    [CreateAssetMenu(menuName = "Player/Abilities/GrappleHookAbility", fileName = "GrappleHookAbilitySO")]
    public class GrappleHookAbility : AbstractAbility 
    {
        private RaycastHit hit;
        private int maxDist = 200;
        private LayerMask layerMask;

        private GameObject playerGameObject;
        private Transform camera, player;
        private Rigidbody playerRB;

        private float startTime;

        private MonoBehaviour monoBehavior;

        public override void ActivateAbility()
        {
            startTime = Time.time;

            Debug.Log("Grapple");
            Debug.Log($"Activating Ability {name}");

            GrappleStart();
        }

        private void GrappleSetup()
        {
            monoBehavior = GameObject.FindObjectOfType<MonoBehaviour>();
            camera = Camera.main.transform;
            playerGameObject = GameObject.FindGameObjectWithTag("Player");
            playerRB = playerGameObject.GetComponent<Rigidbody>();
            player = playerGameObject.transform;
            layerMask = LayerMask.GetMask("whatIsGround");
        }

        private void GrappleStart()
        {
            GrappleSetup();

            if (Physics.Raycast(camera.transform.position, camera.TransformDirection(Vector3.forward), out hit, Mathf.Infinity, layerMask))
            {
                Debug.DrawRay(camera.position, camera.TransformDirection(Vector3.forward) * hit.distance, Color.yellow);
                Debug.Log("-----------------------Did Hit: " + hit.point);

                DrawGrapple();
                monoBehavior.StartCoroutine(GrappleMovement());
            }
            else
            {
                Debug.DrawRay(camera.position, camera.TransformDirection(Vector3.forward) * 1000, Color.white);
                Debug.Log("-----------------------Did not Hit");
            }

        }
         
        private IEnumerator GrappleMovement()
        {

            while (Vector3.Distance(player.transform.position, hit.point) > 0.5f)
            {
                player.transform.position = Vector3.Lerp(player.transform.position, hit.point, (Time.time - startTime));

                playerRB.AddForce(0, 0.2f, 0, ForceMode.Impulse);

                yield return null;

            }
            yield return new WaitForSeconds(2f);
        }

        private void DrawGrapple()
        {
            GameObject grappleLine = new GameObject();
            LineRenderer drawLine;

            drawLine = grappleLine.AddComponent<LineRenderer>();
            drawLine.material = new Material(Shader.Find("Sprites/Default"));

            drawLine.startColor = Color.blue;
            drawLine.endColor = Color.red;

            drawLine.startWidth = .01f;
            drawLine.endWidth = .1f;

            drawLine.SetPosition(0, player.transform.position);
            drawLine.SetPosition(1, hit.point);

            GameObject.Destroy(grappleLine, .25f);
        }

    }
}