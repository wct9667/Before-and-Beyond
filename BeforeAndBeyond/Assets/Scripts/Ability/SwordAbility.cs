using UnityEngine;

namespace Ability
{
    [CreateAssetMenu(menuName = "Player/Abilities/SwordAbility", fileName = "SwordAbilitySO")]
    public class SwordAbility : AbstractAbility
    {
        private GameObject sword;
        private Transform camera;
        private Animator anim;
        private float attackReach;
        private RaycastHit hit;
        private LayerMask layerMask;

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
            layerMask = LayerMask.GetMask("whatIsEnemy");
            attackReach = 3.0f;
        }

        private void SwordSlash()
        {
            anim.SetTrigger("Attack");
            AttackRay();
        }

        private void AttackRay()
        {
            if(Physics.Raycast(camera.transform.position, camera.TransformDirection(Vector3.forward), out hit, attackReach, layerMask))
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