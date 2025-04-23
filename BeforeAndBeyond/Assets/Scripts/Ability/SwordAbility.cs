using Enemy;
using UnityEngine;

namespace Ability
{
    [CreateAssetMenu(menuName = "Player/Abilities/SwordAbility", fileName = "SwordAbilitySO")]
    public class SwordAbility : AbstractAbility
    {
        [Header("Ability Attributes")]
        [SerializeField] private float attackReach = 3.5f;
        [SerializeField] private float attackWidthRadius = 2.0f;
        [SerializeField] private LayerMask enemyLayerMask;
        
        private GameObject sword;
        private Transform camera;
        private Animator anim;
        private RaycastHit hit;

        public override void ActivateAbility()
        {
            SwordSlash();
        }

        public override void Setup()
        {
            camera = Camera.main.transform;
            sword = GameObject.FindGameObjectWithTag("Sword");
            anim = sword.GetComponent<Animator>();
        }

        private void SwordSlash()
        {
            anim.SetTrigger("Attack");
            AttackRay();
        }

        private void AttackRay()
        {
            if(Physics.SphereCast(camera.transform.position, attackWidthRadius, camera.TransformDirection(Vector3.forward), out hit, attackReach, enemyLayerMask))
            {
                HitTarget(hit);
            }
            else
            {
                Debug.Log("No Enemy!");
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