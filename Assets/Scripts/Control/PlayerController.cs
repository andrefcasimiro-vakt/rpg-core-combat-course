using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Combat;
using RPG.Core;

namespace RPG.Control
{
    public class PlayerController : MonoBehaviour
    {
        Health health;

        // Start is called before the first frame update
        void Start()
        {
            health = GetComponent<Health>();
        }

        // Update is called once per frame
        void Update()
        {
            if (health.IsDead()) return;
            if (InteractWithCombat()) return;
            if (InteractWithMovement()) return;
        }

        bool InteractWithCombat()
        {
            RaycastHit[] hits = Physics.RaycastAll(GetMouseRay());

            foreach (RaycastHit hit in hits)
            {
                CombatTarget target = hit.transform.gameObject.GetComponent<CombatTarget>();
                if (target == null) continue;

                // Can we attack this target?
                if (!GetComponent<Fighter>().CanAttack(target.gameObject))
                {
                    continue;
                }

                if (Input.GetMouseButton(0))
                {
                    GetComponent<Fighter>().Attack(target.gameObject);
                }

                return true;
            }

            return false;
        }

        bool InteractWithMovement()
        {
            RaycastHit hit;

            bool hasHit = Physics.Raycast(GetMouseRay(), out hit);

            if (hasHit)
            {
                if (Input.GetMouseButton(0))
                {
                    GetComponent<RPG.Movement.Mover>().StartMoveAction(hit.point, 1f);
                }

                return true;
            }

            return false;
        }

        static Ray GetMouseRay()
        {
            return Camera.main.ScreenPointToRay(Input.mousePosition);
        }
    }
}
