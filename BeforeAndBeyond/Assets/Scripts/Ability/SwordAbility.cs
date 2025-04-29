using Enemy;
using UnityEngine;

namespace Ability
{
    [CreateAssetMenu(menuName = "Player/Abilities/SwordAbility", fileName = "SwordAbilitySO")]
    public class SwordAbility : AbstractAbility
    {
        [Header("Ability Attributes")]
        [SerializeField] private float attackReach = 2f;
        [SerializeField] private LayerMask enemyLayerMask;
        
        private GameObject sword;
        private Transform swordTip;

        private Transform camera;
        private Animator anim;

        public override void ActivateAbility()
        {
            SwordSlash();
        }

        public override void Setup()
        {
            camera = Camera.main.transform;
            sword = GameObject.FindGameObjectWithTag("Sword");
            anim = sword.GetComponent<Animator>();
            swordTip = sword.transform.Find("swordTip");

        }

        private void SwordSlash()
        {
            anim.SetTrigger("Attack");
            AttackDetect();
        }

        private void AttackDetect()
        {
            Collider[] enemiesDetected = Physics.OverlapSphere(swordTip.position, attackReach, enemyLayerMask);
            foreach (var enemy in enemiesDetected)
            {
                Debug.Log("Enemy in Range at: " + enemy.transform.position);

                Health health = enemy.transform.gameObject.GetComponent<Health>();
                health.SubtractHealth(50f);

                //Destroy(enemy.transform.gameObject, .25f);
            }
        }

        private void HitTarget(RaycastHit hit)
        {
            Debug.Log("Hit an Enemy!");

            Health health = hit.transform.gameObject.GetComponent<Health>();
            health.SubtractHealth(50);
            
            //replace with actually dealing damage
            //Destroy(hit.transform.gameObject, .25f);
        }
    }
}