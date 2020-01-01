using System;
using UnityEngine;

namespace RPG.Combat
{
    [CreateAssetMenu(fileName = "Weapon", menuName = "Weapons/Make New Weapon", order = 0)]
    public class Weapon : ScriptableObject
    {
        [Header("Graphics")]
        [SerializeField] GameObject equippedPrefab = null;

        [Header("Animator Controller Override")]
        [SerializeField] AnimatorOverrideController animatorOverride = null;

        [Header("Stats")]
        [SerializeField] float damage = 5f;
        [SerializeField] float range = 2f;

        [SerializeField] bool isRightHanded = true;

        [SerializeField] Projectile projectile = null;

        const string weaponName = "Weapon";
        const string destroyingWeaponName = "Destroying";

        public void Spawn(Transform rightHandTransform, Transform leftHandTransform, Animator animator)
        {
            DestroyOldWeapon(rightHandTransform, leftHandTransform);

            if (equippedPrefab != null)
            {
                GameObject weapon = Instantiate(
                    equippedPrefab, 
                    isRightHanded
                        ? rightHandTransform
                        : leftHandTransform
                 );

                weapon.name = weaponName;
            }

            if (animatorOverride != null)
            {
                // Override character animator
                animator.runtimeAnimatorController = animatorOverride;
            }
        }

        private void DestroyOldWeapon(Transform rightHandTransform, Transform leftHandTransform)
        {
            Transform oldWeapon = rightHandTransform.Find(weaponName);
            if (oldWeapon == null)
            {
                oldWeapon = leftHandTransform.Find(weaponName);
            }

            if (oldWeapon == null) return;

            oldWeapon.name = destroyingWeaponName;
            Destroy(oldWeapon.gameObject);
        }

        public float GetDamage()
        {
            return damage;
        }

        public float GetRange()
        {
            return range;
        }

        public bool HasProjectile()
        {
            return projectile != null;
        }

        public void LaunchProjectile(Transform rightHandTransform, Transform leftHandTransform, Transform target)
        {
            Projectile projectileInstance = Instantiate(
                        projectile,
                        isRightHanded
                            ? rightHandTransform.position
                            : leftHandTransform.position,
                        Quaternion.identity
            );

            projectileInstance.SetTarget(target, damage);
        }
    }

}

