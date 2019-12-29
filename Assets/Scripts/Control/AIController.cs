using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Combat;
using RPG.Core;
using RPG.Movement;

namespace RPG.Control
{
    public class AIController : MonoBehaviour
    {
        [Header("Movement Settings")]
        [Range(0, 1)]
        [SerializeField] float patrolSpeedFraction = 0.2f;

        [Header("Chasing Settings")]
        [SerializeField] float chaseDistance = 5f;
        [SerializeField] float suspicionTime = 3f;


        [Header("Waypoint Settings")]
        [SerializeField] PatrolPath patrolPath = null;
        [SerializeField] float waypointTolerance = 1f;
        [SerializeField] float waypointDwellTime = 3f;

        string PLAYER = "Player";

        GameObject player;
        Fighter fighter;
        Health health;

        Vector3 guardPosition;
        int currentWaypointIndex = 0;

        // Timers
        float timeSinceLastSawPlayer = Mathf.Infinity;
        float timeSinceArrivedAtWaypoint = Mathf.Infinity;

        private void Start()
        {
            player = GameObject.FindWithTag(PLAYER);
            fighter = GetComponent<Fighter>();
            health = GetComponent<Health>();

            // Store the original position
            guardPosition = transform.position;
        }

        private void Update()
        {
            if (health.IsDead())
            {
                return;
            }

            if (InAttackRangeOfPlayer() && fighter.CanAttack(player))
            {
                AttackBehaviour();
            } 
            else if (timeSinceLastSawPlayer < suspicionTime)
            {
                SuspicionBehaviour();
            }
            else
            {
                PatrolBehaviour();
            }

            UpdateTimers();
        }

        void UpdateTimers()
        {
            timeSinceLastSawPlayer += Time.deltaTime;
            timeSinceArrivedAtWaypoint += Time.deltaTime;
        }

        void AttackBehaviour()
        {
            timeSinceLastSawPlayer = 0;
            fighter.Attack(player);
        }

        void SuspicionBehaviour()
        {
            GetComponent<ActionScheduler>().CancelCurrentAction();
        }

        void PatrolBehaviour()
        {
            Vector3 nextPosition = guardPosition;

            if (patrolPath != null)
            {
                if (AtWaypoint())
                {
                    timeSinceArrivedAtWaypoint = 0f;
                    CycleWaypoint();
                }
                nextPosition = GetCurrentWaypoint();
            }

            if (timeSinceArrivedAtWaypoint > waypointDwellTime) { 
                // Return to the original position
                GetComponent<Mover>().StartMoveAction(nextPosition, patrolSpeedFraction);
            }
        }

        bool AtWaypoint()
        {
            return Vector3.Distance(transform.position, GetCurrentWaypoint()) < waypointTolerance;
        }

        void CycleWaypoint()
        {
            currentWaypointIndex = patrolPath.GetNextIndex(currentWaypointIndex);
        }

        Vector3 GetCurrentWaypoint()
        {
            return patrolPath.GetWaypoint(currentWaypointIndex);
        }

        bool InAttackRangeOfPlayer()
        {
            return Vector3.Distance(transform.position, player.transform.position) < chaseDistance;
        }

        // Called by Unity
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireSphere(transform.position, chaseDistance);
        }
    }

}
