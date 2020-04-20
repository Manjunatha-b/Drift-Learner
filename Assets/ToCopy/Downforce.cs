using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Downforce : MonoBehaviour
{
    public Rigidbody body;
    public float liftCoefficient;

    // Update is called once per frame
    void FixedUpdate()
    {
        if (body != null)
        {
            float lift = liftCoefficient * body.velocity.sqrMagnitude;
            body.AddForceAtPosition(lift * transform.up, transform.position);
        }
    }
}
