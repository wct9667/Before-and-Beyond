using UnityEngine;
using System.Collections;
namespace Ability
{
    [CreateAssetMenu(menuName = "Player/Abilities/GrappleHookAbility", fileName = "GrappleHookAbilitySO")]
    public class GrappleHookAbility : AbstractAbility 
    {
        private Vector3 grapplePoint;
        private int maxDist = 200;
        private LayerMask layerMask;

        private Transform camera, player;

        private float speed = 100f;
        private float startTime;
        private float overTime;

        public override void ActivateAbility()
        {
            startTime = Time.time;
            overTime = startTime + 2f;

            Debug.Log("Grapple");
            Debug.Log($"Activating Ability {name}");

            GrappleStart();
        }

        private void GrappleSetup()
        {
            camera = Camera.main.transform;
            player = GameObject.FindGameObjectWithTag("Player").transform;
            layerMask = LayerMask.GetMask("whatIsGround");

        }

        private void GrappleStart()
        {
            GrappleSetup();

            RaycastHit hit;

            if (Physics.Raycast(camera.transform.position, camera.TransformDirection(Vector3.forward), out hit, Mathf.Infinity, layerMask))
            {
                Debug.DrawRay(camera.position, camera.TransformDirection(Vector3.forward) * hit.distance, Color.yellow);
                Debug.Log("-----------------------Did Hit: " + hit.point);

                DrawGrapple(hit);

                GrappleMovement(hit);
            }
            else
            {
                Debug.DrawRay(camera.position, camera.TransformDirection(Vector3.forward) * 1000, Color.white);
                Debug.Log("-----------------------Did not Hit");
            }

        }
         
        private IEnumerator GrappleMovement(RaycastHit hit)
        {
            //player.transform.position = Vector3.Lerp(camera.position, hit.point, fractionOfJourney);

            while (startTime < overTime)
            {
                player.transform.position = Vector3.Lerp(player.transform.position, hit.point, (Time.time - startTime) * speed); 

                yield return null;
            }

            yield return new WaitForSeconds(1f);


            player.transform.position = hit.point;
        }



        private void DrawGrapple(RaycastHit hit)
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

            GameObject.Destroy(grappleLine, 1.5f);
        }

    }
}