using UnityEngine;
using RPG.Core;
using RPG.Movement;
using RPG.Saving;

namespace RPG.Combat {
    public class Fighter : MonoBehaviour, IAction, ISaveable
    {
        [SerializeField] float timeBetweenAttacks = 1f;
        float timeSinceLastAttack = Mathf.Infinity;

        [Header("Weapons")]
        [SerializeField] Transform leftHandTransform = null;
        [SerializeField] Transform rightHandTransform = null;
        Weapon currentWeapon = null;

        [Header("Weapons Resources Settings")]
        [SerializeField] string defaultWeaponName = "Unarmed";

        Animator animator;
        Transform target;
        Mover mover;

        string ANIMATOR_PARAMETER_TRIGGER_ATTACK = "attack";
        string ANIMATOR_PARAMETER_TRIGGER_STOP_ATTACK = "stopAttack";

        void Start()
        {
            mover = GetComponent<Mover>();
            animator = GetComponent<Animator>();

            if (currentWeapon == null) {
                LoadAndEquipWeapon(defaultWeaponName);
            }
        }

        void Update()
        {
            timeSinceLastAttack += Time.deltaTime;

            if (target == null) return;
            if (target.GetComponent<Health>().IsDead()) return;

            if (!GetIsInRange())
            {
                mover.MoveTo(target.position, 1f);
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

        private bool GetIsInRange()
        {
            return Vector3.Distance(transform.position, target.position) < currentWeapon.GetRange();
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
            mover.Cancel();
        }

        // Animation Event
        void Hit()
        {
            if (target == null)
            {
                return;
            }

            if (currentWeapon.HasProjectile())
            {
                currentWeapon.LaunchProjectile(leftHandTransform, rightHandTransform, target);
                return;
            }

            Health targetHealth = target.gameObject.GetComponent<Health>();
            targetHealth.TakeDamage(currentWeapon.GetDamage());
        }

        void Shoot()
        {
            Hit();
        }


        // EQUIPMENT
        public void EquipWeapon(Weapon weapon)
        {
            currentWeapon = weapon;
            weapon.Spawn(rightHandTransform, leftHandTransform, GetComponent<Animator>());
        }

        void LoadAndEquipWeapon(string weaponName)
        {
            Weapon weapon = Resources.Load<Weapon>(weaponName);

            // By default equip the default weapon
            EquipWeapon(weapon);
        }

        // SAVE METHODS
        public object CaptureState()
        {
            return currentWeapon.name;
        }

        public void RestoreState(object state)
        {
            string weaponName = (string)state;

            LoadAndEquipWeapon(weaponName);
        }
    }

}

