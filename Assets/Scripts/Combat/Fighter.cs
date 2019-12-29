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
        float timeSinceLastAttack = Mathf.Infinity;

        Animator animator;
        Transform target;
        Mover mover;

        string ANIMATOR_PARAMETER_TRIGGER_ATTACK = "attack";
        string ANIMATOR_PARAMETER_TRIGGER_STOP_ATTACK = "stopAttack";

        void Start()
        {
            mover = GetComponent<Mover>();
            animator = GetComponent<Animator>();
        }

        void Update()
        {
            timeSinceLastAttack += Time.deltaTime;

            if (target == null) return;
            if (target.GetComponent<Health>().IsDead()) return;

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
            // Look at target
            transform.LookAt(target);

            if (timeSinceLastAttack > timeBetweenAttacks) {
                TriggerAttack();

                // Reset timer
                timeSinceLastAttack = 0f;
            }
        }

        private void TriggerAttack()
        {
            // This will trigger the Hit() event
            animator.ResetTrigger(ANIMATOR_PARAMETER_TRIGGER_STOP_ATTACK);
            animator.SetTrigger(ANIMATOR_PARAMETER_TRIGGER_ATTACK);
        }
        private void TriggerStopAttack()
        {
            animator.ResetTrigger(ANIMATOR_PARAMETER_TRIGGER_ATTACK);
            animator.SetTrigger(ANIMATOR_PARAMETER_TRIGGER_STOP_ATTACK);
        }

        // Animation Event
        void Hit()
        {
            if (target == null)
            {
                return;
            }

            Health targetHealth = target.gameObject.GetComponent<Health>();
            targetHealth.TakeDamage(weaponDamage);
        }

        private bool GetIsInRange()
        {
            return Vector3.Distance(transform.position, target.position) < weaponRange;
        }

        public bool CanAttack(GameObject combatTarget)
        {
            if (combatTarget == null)
            {
                return false;
            }

            Health targetHealth = combatTarget.GetComponent<Health>();

            return targetHealth != null && !targetHealth.IsDead();
        }

        public void Attack(GameObject combatTarget)
        {
            GetComponent<ActionScheduler>().StartAction(this);
            target = combatTarget.transform;
        }

        public void Cancel()
        {
            TriggerStopAttack();
            target = null;
        }
    }

}

