using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAgents;
using System;

public class CarAgent : Agent
{
    Rigidbody r;

    public List<WheelCollider> throttleWheels; 
    public List<WheelCollider> steeringWheels;
    public float strengthCoefficient;
    public float maxTurn;

    public override void AgentAction(float[] act)
    {

        int throtact =(int) act[0];
        int steeract =(int) act[1];
        int throtfactor=0;
        int steerfactor=0;

        if(throtact==1)
            throtfactor=    -1;
        else if(throtact==2)
            throtfactor=   1;
        if(steeract==1)
            steerfactor=    1;
        else if(steeract==2)
            steerfactor=   -1;

        float torq = strengthCoefficient * Time.deltaTime * throtfactor;
        float steerforce = maxTurn * steerfactor;
        Debug.Log(torq+" "+steerforce);

        foreach( WheelCollider wheel in throttleWheels){
            wheel.motorTorque = torq;
        }
        foreach( WheelCollider wheel in steeringWheels){
            wheel.steerAngle = steerforce;
        }
    }

    public override float[] Heuristic()
    {
        var action = new float[2];
        action[0] = Input.GetAxis("Vertical");
        action[1] = Input.GetAxis("Horizontal");
        if(action[0]==-1)
            action[0]=2;
        else if(action[1]==-1)
            action[1]=2;

        //Debug.Log(action[0]+" "+action[1]);
        return action;

    }


}
