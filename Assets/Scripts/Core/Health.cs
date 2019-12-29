using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Core
{
    public class Health : MonoBehaviour
    {
        [SerializeField] float healthPoints = 100f;

        bool isDead = false;

        string ANIMATOR_PARAMETER_TRIGGER_DIE = "die";

        public void TakeDamage(float damage)
        {
            if (isDead) return;

            healthPoints = Mathf.Max(healthPoints - damage, 0);

            if (healthPoints == 0)
            {
                Die();
            }
        }

        void Die()
        {
            if (!isDead)
            {
                GetComponent<Animator>().SetTrigger(ANIMATOR_PARAMETER_TRIGGER_DIE);
                GetComponent<ActionScheduler>().CancelCurrentAction();

                isDead = true;
            }
        }

        public bool IsDead()
        {
            return isDead;
        }
    }

}
