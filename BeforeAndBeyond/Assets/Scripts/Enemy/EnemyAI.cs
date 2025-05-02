using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Enemy
{

    public class EnemyAI : MonoBehaviour
    {
        public NavMeshAgent agent;

        public Transform player;

        public LayerMask whatIsGround, whatIsPlayer;

        private Animator animator;

        // Patrol state
        public Vector3 walkPoint;
        bool walkPointSet;
        public float walkPointRange;

        // Attack state
        public float attackCD;
        bool attackOnCD;

        // States
        public float sightRange, attackRange;
        public bool playerInSightRange, playerInAttackRange, stunned;

        private void Awake()
        {
            player = GameObject.Find("Player").transform;
            agent = GetComponent<NavMeshAgent>();
            stunned = false;
            animator = GetComponent<Animator>();
        }

        private void Update()
        {
            // Check if player is in range
            playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
            playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);

            if (stunned) { Stunned(); return; }
            if (!playerInSightRange && !playerInAttackRange) Patroling();
            if (playerInSightRange && !playerInAttackRange) ChasePlayer();
            if (playerInAttackRange && playerInSightRange) Attack();
        }


        public void Stun(float time)
        {
            stunned = true;
            Invoke("UnStun", time);
        }

        private void UnStun()
        {
            stunned = false;
        }

        private void Stunned()
        {
            agent.SetDestination(this.gameObject.transform.position);
        }

        private void Patroling()
        {
            // Did not have time to fully implement patrolling for now
            return;

            if (!walkPointSet) SearchWalkPoint();

            if (walkPointSet)
            {
                agent.SetDestination(walkPoint);
            }

            if ((transform.position - walkPoint).magnitude < 1f)
            {
                walkPointSet = false;
            }
        }

        private void SearchWalkPoint()
        {

        }

        private void ChasePlayer()
        {
            agent.SetDestination(player.position);
        }

        private void Attack()
        {
            // So it doesnt box player in, may not be needed
            agent.SetDestination(transform.position);

            transform.LookAt(player);

            if (!attackOnCD)
            {

                EventBus<DecreasePlayerHealth>.Raise(new DecreasePlayerHealth()
                {
                    healthChange = 10
                });

                animator.Play("ATTACK");

                // Reset cd
                attackOnCD = true;
                Invoke(nameof(ResetAttack), attackCD);
            }
        }

        private void ResetAttack()
        {
            attackOnCD = false;
        }
    }
}
