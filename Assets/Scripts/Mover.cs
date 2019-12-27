using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Mover : MonoBehaviour
{
    [SerializeField] bool debug = false;

    string ANIMATOR_PARAMETER_FORWARD_SPEED = "forwardSpeed";

    void Update() 
    {
        if (Input.GetMouseButton(0))
        {
            MoveToCursor();
        }

        UpdateAnimator();
    }

    void UpdateAnimator()
    {
        Vector3 velocity = GetComponent<NavMeshAgent>().velocity;
        Vector3 localVelocity = transform.InverseTransformDirection(velocity);

        float speed = localVelocity.z;
        GetComponent<Animator>().SetFloat(ANIMATOR_PARAMETER_FORWARD_SPEED, speed);    
    }

    void MoveToCursor() {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        bool hasHit = Physics.Raycast(ray, out hit);

        if (debug) {
            Debug.DrawRay(ray.origin, ray.direction * 100);
        }

        if (hasHit)
        {
            GetComponent<NavMeshAgent>().destination = hit.point;
        }
    }

}