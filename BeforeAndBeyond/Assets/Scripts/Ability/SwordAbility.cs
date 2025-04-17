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
            Debug.Log("Sword");
            Debug.Log($"Activating Ability {name}");

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
                HitTarget();
            }
            else
            {
                Debug.Log("No Enemy!");
            }
        }

        private void HitTarget()
        {
            Debug.Log("Hit an Enemy!");

            //replace with actually dealing damage
            Destroy(hit.transform.gameObject, .25f);
        }
    }
}