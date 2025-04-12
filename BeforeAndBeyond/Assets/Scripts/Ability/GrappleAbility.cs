using UnityEngine;

namespace Ability
{
    [CreateAssetMenu(menuName = "Player/Abilities/GrappleHookAbility", fileName = "GrappleHookAbilitySO")]
    public class GrappleHookAbility : AbstractAbility 
    {
        private Vector3 grapplePoint;
        private int maxDist = 200;
        private LayerMask layerMask;
        private SpringJoint spring;
        private Transform camera, player;

        public override void ActivateAbility()
        {
            Debug.Log("Grapple");
            Debug.Log($"Activating Ability {name}");

            GrappleMovement();
        }

        private void GrappleSetup()
        {
            camera = Camera.main.transform;
            player = GameObject.FindGameObjectWithTag("Player").transform;
            layerMask = LayerMask.GetMask("whatIsGround");
        }

        private void GrappleMovement()
        {
            GrappleSetup();

            RaycastHit hit;

            if (Physics.Raycast(camera.transform.position, camera.TransformDirection(Vector3.forward), out hit, Mathf.Infinity, layerMask))
            {
                Debug.DrawRay(camera.position, camera.TransformDirection(Vector3.forward) * hit.distance, Color.yellow);
                Debug.Log("-----------------------Did Hit: " + hit.point);
            }
            else
            {
                Debug.DrawRay(camera.position, camera.TransformDirection(Vector3.forward) * 1000, Color.white);
                Debug.Log("-----------------------Did not Hit");
            }

        }

        private void EndGrapple()
        {
        }
    }
}