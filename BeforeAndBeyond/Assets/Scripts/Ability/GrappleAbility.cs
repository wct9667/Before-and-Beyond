using UnityEngine;

namespace Ability
{
    [CreateAssetMenu(menuName = "Player/Abilities/GrappleHookAbility", fileName = "GrappleHookAbilitySO")]
    public class GrappleHookAbility : AbstractAbility 
    {
        private Vector3 grapplePoint;
        private float maxDist;
        private LayerMask whatIsGround;
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
        }

        private void GrappleMovement()
        {
            GrappleSetup();

            RaycastHit ray;
            if(Physics.Raycast(camera.position, camera.forward, out ray, maxDist, whatIsGround))
            {
                grapplePoint = ray.point;
                Debug.Log("Raycast Point: " + ray.point);

                spring = player.gameObject.AddComponent<SpringJoint>();
                spring.autoConfigureConnectedAnchor = false;
                spring.connectedAnchor = grapplePoint;

                float distanceFromPoint = Vector3.Distance(player.position, grapplePoint);

                spring.maxDistance = distanceFromPoint * 0.05f;
                spring.minDistance = distanceFromPoint * 0.33f;

                spring.spring = 25f;
                spring.damper = 5f;
                spring.massScale = 2.5f;

            }
        }

        private void EndGrapple()
        {
            Destroy(spring);
        }
    }
}