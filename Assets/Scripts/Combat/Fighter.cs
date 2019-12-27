using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Core;
using RPG.Movement;

namespace RPG.Combat {
    public class Fighter : MonoBehaviour, IAction
    {
        [SerializeField] float weaponRange = 2f;
        [SerializeField] float weaponDamage = 5f;

        [SerializeField] float timeBetweenAttacks = 1f;
        float timeSinceLastAttack = 0f;

        Animator animator;
        Transform target;
        Mover mover;

        string ANIMATOR_PARAMETER_TRIGGER_ATTACK = "attack";

        void Start()
        {
            mover = GetComponent<Mover>();
            animator = GetComponent<Animator>();
        }

        void Update()
        {
            timeSinceLastAttack += Time.deltaTime;

            if (target == null) return;

            if (!GetIsInRange())
            {
                mover.MoveTo(target.position);
            }
            else
            {
                mover.Cancel();
                AttackBehaviour();
            }
        }

        private void AttackBehaviour()
        {
            if (timeSinceLastAttack > timeBetweenAttacks) { 
                // This will trigger the Hit() event
                animator.SetTrigger(ANIMATOR_PARAMETER_TRIGGER_ATTACK);

                // Reset timer
                timeSinceLastAttack = 0f;
            }
        }

        // Animation Event
        void Hit()
        {
            Health targetHealth = target.gameObject.GetComponent<Health>();
            targetHealth.TakeDamage(weaponDamage);
        }

        private bool GetIsInRange()
        {
            return Vector3.Distance(transform.position, target.position) < weaponRange;
        }

        public void Attack(CombatTarget combatTarget)
        {
            GetComponent<ActionScheduler>().StartAction(this);
            target = combatTarget.transform;
        }

        public void Cancel()
        {
            target = null;
        }
    }

}

