using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AntiRoll : MonoBehaviour
{
    public WheelCollider WheelL;
    public WheelCollider WheelR;
    public float antiRoll = 5000f;
    public Rigidbody r;
    // Update is called once per frame
    void FixedUpdate()
    {
        WheelHit hit;
        float travelL = 1f;
        float travelR = 1f;
        bool groundedL;
        bool groundedR;

        groundedL = WheelL.GetGroundHit(out hit);
        if(groundedL)
            travelL = (-WheelL.transform.InverseTransformPoint(hit.point).y - WheelL.radius) / WheelL.suspensionDistance;

        groundedR = WheelR.GetGroundHit(out hit);
        if(groundedR)
            travelR = (-WheelR.transform.InverseTransformPoint(hit.point).y - WheelR.radius) / WheelR.suspensionDistance;


        float antiRollForce = antiRoll* (travelL - travelR);
        //if(!groundedL)
            r.AddForceAtPosition(WheelL.transform.up * -antiRollForce, WheelL.transform.position);

        //if(!groundedR)
            r.AddForceAtPosition(WheelR.transform.up * antiRollForce, WheelR.transform.position);

    }
}
