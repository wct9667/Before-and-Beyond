using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enemy;

namespace Ability
{
    [CreateAssetMenu(menuName = "Player/Abilities/HammerAbility", fileName = "HammerAbilitySO")]
    public class HammerAbility : AbstractAbility
    {
        [Header("Ability Attributes")]
        [SerializeField] private float attackReach = 3.5f;
        [SerializeField] private float plasmaAttackReach = 6f;

        [SerializeField] private LayerMask enemyLayer;
        [SerializeField] private LayerMask plasmaLayer;
        [SerializeField] private LayerMask groundLayer;
        [SerializeField] private float baseballForce = 100f;

        private GameObject hammer;
        private Transform hammerTip;

        private Transform camera;
        private Animator anim;

        private GameObject sword;
        private Animator animSword;

        private MonoBehaviour monoBehavior;

        public override void ActivateAbility()
        {
            Debug.Log("Hammer");
            animSword.SetTrigger("PutAway");
            anim.SetTrigger("HammerSwing");
            monoBehavior = FindObjectOfType<MonoBehaviour>();


            monoBehavior.StartCoroutine(AttackDetect());


        }

        public override void Setup()
        {
            sword = GameObject.FindGameObjectWithTag("Sword");

            camera = Camera.main.transform;
            hammer = GameObject.FindGameObjectWithTag("Hammer");
            anim = hammer.GetComponent<Animator>();

            animSword = sword.GetComponent<Animator>();

            hammerTip = hammer.transform.Find("hammerTip");

        }

        private IEnumerator AttackDetect()
        {
            yield return new WaitForSeconds(0.8f);

            Collider[] enemyCollidersDetected = Physics.OverlapSphere(hammerTip.position, attackReach, enemyLayer);
            Collider[] plasmaCollidersDetected = Physics.OverlapSphere(hammerTip.position, plasmaAttackReach, plasmaLayer);
            foreach (var col in enemyCollidersDetected)
            {
                Debug.Log("Enemy in Range at: " + col.transform.position);

                Health health = col.transform.gameObject.GetComponent<Health>();
                health.SubtractHealth(75f);

                for (float i = 0; i < 25; i++)
                {
                    yield return new WaitForSeconds(.001f);
                    col.transform.position += camera.transform.forward;

                    if(Physics.CheckSphere(new Vector3(col.transform.position.x, col.transform.position.y + 1, col.transform.position.z), 1f, groundLayer))
                    {
                        health.SubtractHealth(25f);
                    }
                }
            }

            foreach (var col in plasmaCollidersDetected)
            {
                col.transform.gameObject.GetComponent<Rigidbody>().drag = 0;
                col.transform.gameObject.GetComponent<Rigidbody>().AddForce(camera.transform.forward * baseballForce, ForceMode.Impulse);
            }

            yield return new WaitForSeconds(.075f);
            animSword.SetTrigger("TakeOut");

            yield return new WaitForSeconds(.01f);
        }
    }
}
