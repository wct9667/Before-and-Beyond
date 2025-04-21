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
        [SerializeField] private LayerMask wallLayerMask;
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
        private Transform joustTip;

        private float startTime;

        private MonoBehaviour monoBehavior;

        private PlayerController playerController;

        private bool endJoust;

        public override void ActivateAbility()
        {
            Debug.Log("-----------------------Joust Activated");
            JoustStart();

        }

        public override void Setup()
        {
            endJoust = false;
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
            endJoust = false;

            Joust(movementSpeedScaling);

        }

        private void Joust(float moveScale)
        {
            StartingAnimation();
            monoBehavior.StartCoroutine(JoustMovement(moveScale));
        }
        private IEnumerator JoustMovement(float moveScale)
        {
            for (float i = 0; i < 125; i = i + 0.05f)
            {
                if(endJoust == false)
                {
                    SkewerEnemy();
                    playerRB.AddForce(new Vector3(camera.transform.forward.x, 0, camera.transform.forward.z) * (i * moveScale), ForceMode.Force);
                    yield return new WaitForSeconds(.0025f);
                }
                
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
                if (Physics.SphereCast(camera.transform.position, skewerRange*0.5f, new Vector3(camera.TransformDirection(Vector3.forward).x, 0, camera.TransformDirection(Vector3.forward).z), out hit, skewerRange, wallLayerMask))
                {
                    Destroy(enemy.transform.gameObject);
                    monoBehavior.StartCoroutine(DrawEffect());

                    endJoust = true;
                }
                enemy.transform.position = joustTip.transform.position;
            }

            if (Physics.SphereCast(camera.transform.position, skewerRange * 0.5f, new Vector3(camera.TransformDirection(Vector3.forward).x, 0, camera.TransformDirection(Vector3.forward).z), out hit, skewerRange, wallLayerMask))
            {
                monoBehavior.StartCoroutine(DrawEffect());

                endJoust = true;
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

            drawLine.startColor = Color.yellow;
            drawLine.endColor = Color.yellow;

            drawLine.startWidth = .25f;
            drawLine.endWidth = .25f;

            for (float j = 0; j < 5; j = j + 0.1f)
            {
                for (int i = 0; i < (51); i++)
                {
                    if (drawLine != null)
                    {
                        x = Mathf.Sin(Mathf.Deg2Rad * angle) * j;
                        z = Mathf.Cos(Mathf.Deg2Rad * angle) * j;

                        drawLine.SetPosition(i, new Vector3(joustTip.transform.position.x + x, joustTip.transform.position.y, joustTip.transform.position.z + z));

                        angle += (390f / 51);
                    }
                }
                yield return new WaitForSeconds(.001f);
            }
            GameObject.Destroy(circle, .1f);

            yield return new WaitForSeconds(.1f);

        }

    }

}

