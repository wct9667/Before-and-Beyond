using UnityEngine;
using System.Collections;
using Player;

namespace Ability
{
    [CreateAssetMenu(menuName = "Player/Abilities/GrappleHookAbility", fileName = "GrappleHookAbilitySO")]
    public class GrappleHookAbility : AbstractAbility 
    {
        [Header("Ability Attributes")]
        [SerializeField] private float maxDist = 45;
        [SerializeField]  private LayerMask grappleLayerMask;
        [SerializeField] private float nonRefundedCooldown = 2;

        private RaycastHit hit;

        private GameObject playerGameObject;
        private Transform camera, player;
        private Rigidbody playerRB;

        private float startTime;

        private PlayerController playerController;

        public override void ActivateAbility()
        {
            startTime = Time.time;

            Debug.Log("Grapple");
            Debug.Log($"Activating Ability {name}");

            GrappleStart();
        }

        public override void Setup()
        {
            camera = Camera.main.transform;
            playerGameObject = GameObject.FindGameObjectWithTag("Player");
            player = playerGameObject.transform;
            playerController = playerGameObject.GetComponent<PlayerController>();
        }

        private void GrappleStart()
        {
            if (Physics.Raycast(camera.transform.position, camera.TransformDirection(Vector3.forward), out hit, maxDist, grappleLayerMask))
            {
                this.AbilityCooldown = nonRefundedCooldown;
                Debug.DrawRay(camera.position, camera.TransformDirection(Vector3.forward) * hit.distance, Color.yellow);
                Debug.Log("-----------------------Did Hit: " + hit.point);

                DrawGrapple();
                playerController.Grapple(startTime, hit);
            }
            else
            {
                this.AbilityCooldown = 0;

                Debug.DrawRay(camera.position, camera.TransformDirection(Vector3.forward) * 1000, Color.white);
                Debug.Log("-----------------------Did not Hit");
            }

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