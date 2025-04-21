using UnityEngine;
using System.Collections;
using Player;

namespace Ability
{
    [CreateAssetMenu(menuName = "Player/Abilities/JoustAbility", fileName = "JoustAbilitySO")]
    public class JoustAbility : AbstractAbility
    {
        [Header("Ability Attributes")]
        [SerializeField] private LayerMask enemyLayerMask;
        [SerializeField] private float skewerRange = 1.5f;

        [Header("Speed Scaling")]
        [SerializeField] private float movementSpeedScaling = 1.5f;

        private GameObject joustObject;
        private GameObject playerGameObject;
        private Rigidbody playerRB;

        private GameObject sword;
        private Animator animSword;

        private Transform camera, player;
        private Animator anim;
        private RaycastHit hit;
        private RaycastHit enemyHit;
        private Transform joustTip;

        private float startTime;

        private MonoBehaviour monoBehavior;

        private PlayerController playerController;

        public override void ActivateAbility()
        {
            Debug.Log("-----------------------Joust Activated");
            JoustStart();

        }

        public override void Setup()
        {
            camera = Camera.main.transform;

            playerGameObject = GameObject.FindGameObjectWithTag("Player");
            playerRB = playerGameObject.GetComponent<Rigidbody>();
            player = playerGameObject.transform;
            playerController = playerGameObject.GetComponent<PlayerController>();

            joustObject = GameObject.FindGameObjectWithTag("Joust");
            anim = joustObject.GetComponent<Animator>();

            sword = GameObject.FindGameObjectWithTag("Sword");
            animSword = sword.GetComponent<Animator>();

            joustTip = joustObject.transform.Find("joustTip");


        }

        private void StartingAnimation()
        {
            animSword.SetTrigger("PutAway");
            anim.SetTrigger("StartJoust");
        }

        private void JoustStart()
        {
            monoBehavior = FindObjectOfType<MonoBehaviour>();

            Joust(movementSpeedScaling);

        }

        private void Joust(float moveScale)
        {
            StartingAnimation();
            monoBehavior.StartCoroutine(JoustMovement(moveScale));
        }
        private IEnumerator JoustMovement(float moveScale)
        {
            for (float i = 0; i < 150; i = i + 0.1f)
            {
                SkewerEnemy();
                playerRB.AddForce(new Vector3(camera.transform.forward.x, 0, camera.transform.forward.z) * (i * moveScale), ForceMode.Force);
                yield return new WaitForSeconds(.005f);
            }
            animSword.SetTrigger("TakeOut");
            anim.SetTrigger("EndJoust");

            yield return new WaitForSeconds(1f);
        }

        private void SkewerEnemy()
        {
            Collider[] enemiesDetected = Physics.OverlapSphere(joustTip.transform.position, skewerRange, enemyLayerMask);
            foreach (var enemy in enemiesDetected)
            {
                //enemy.transform.Rotate(0, 0, Random.Range(-180.0f, 180.0f));
                enemy.transform.position = joustTip.transform.position;
            }
        }
    }
}
