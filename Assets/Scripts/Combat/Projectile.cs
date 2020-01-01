using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Core;

namespace RPG.Combat { 

    public class Projectile : MonoBehaviour
    {
        public Transform target = null;
        [SerializeField] float speed = 1f;

        [SerializeField] float durationLifetime = 1f;
        float durationTimer = 0f;

        [SerializeField] bool followsTargetEveryFrame = false;

        Health targetHealth;

        // Passed from parent weapon
        float damage = 0;

        private Vector3 targetPosition = Vector3.zero;

        private void Start()
        {
            targetHealth = target.GetComponent<Health>();

            // SAVE PLAYER POSITION AT THE MOMENT THE PROJECTILE IS LAUNCHED
            targetPosition = GetAimLocation();
            transform.LookAt(targetPosition);
        }

        // Update is called once per frame
        void Update()
        {
            if (durationTimer > durationLifetime)
            {
                Destroy(this.gameObject);
            } else
            {
                durationTimer += Time.deltaTime;
            }


            if (target == null)
            {
                return;
            }


            if (followsTargetEveryFrame && !targetHealth.IsDead())
            {
                transform.LookAt(GetAimLocation());
            }

            transform.Translate(Vector3.forward * speed * Time.deltaTime);
        }

        public void SetTarget(Transform target, float damage)
        {
            this.target = target;
            this.damage = damage;
        }

        Vector3 GetAimLocation()
        {
            CapsuleCollider targetCapsule = target.GetComponent<CapsuleCollider>();

            return targetCapsule.bounds.center;
        }

        private void OnTriggerEnter(Collider other)
        {

            if (other.gameObject != target.gameObject)
            {
                return;
            }

            if (targetHealth.IsDead())
            {
                return;
            }

            targetHealth.TakeDamage(damage);
            Destroy(this.gameObject);
        }
    }

}
