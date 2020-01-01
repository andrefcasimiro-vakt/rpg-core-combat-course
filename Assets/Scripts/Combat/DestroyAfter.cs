using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyAfter : MonoBehaviour
{
    [SerializeField] float lifespan = 3f;
    float elapsedTime = 0f;

    // Update is called once per frame
    void Update()
    {
        if (elapsedTime > lifespan)
        {
            Destroy(this.gameObject);
        } else
        {
            elapsedTime += Time.deltaTime;
        }
    }
}
