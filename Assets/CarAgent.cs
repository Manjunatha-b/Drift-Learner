using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using MLAgents;
using System;

public class CarAgent : Agent
{
    Rigidbody r;

    public List<WheelCollider> throttleWheels;
    public List<WheelCollider> steeringWheels;
    public float strengthCoefficient = 20000;
    public float maxTurn = 20f;
    private Vector3 initposcar;
    private Vector3 initpostarg;
    private Quaternion initrot;

    public DateTime startOfEp;
    private int iterator;

    public LineRenderer lineTarg;
    private Vector2 p1, p2, p3;

    private Vector3[] allpoints;
    public Vector2 offsetv2;

    public GameObject parent;

    GameObject currCollider;

    void Start()
    {
        r = GetComponent<Rigidbody>();
        initposcar = r.position;
        startOfEp = DateTime.UtcNow;
        allpoints = new Vector3[lineTarg.positionCount];
        lineTarg.GetPositions(allpoints);
        initrot = r.rotation;
        TargetCreator();
        offsetv2 = new Vector2(-134.2f, -160.7f);
    }


    public override void AgentReset()
    {
        /*
        this.r.angularVelocity = Vector3.zero;
        this.r.velocity = Vector3.zero;
        r.position = initposcar;
        r.rotation = initrot;
        iterator = 2;
        p1 = new Vector2(allpoints[0].x + offset.x, allpoints[0].y + offset.z);
        p2 = new Vector2(allpoints[1].x + offset.x, allpoints[1].y + offset.z);
        p3 = new Vector2(allpoints[2].x + offset.x, allpoints[2].y + offset.z);

        startOfEp = DateTime.UtcNow;
        TargetShifter();*/

        r.angularVelocity = Vector3.zero;
        r.velocity = Vector3.zero;
        r.rotation = Quaternion.identity;
        iterator = 
            UnityEngine.Random.Range(2, lineTarg.positionCount-1);
        p1 = new Vector2(allpoints[iterator-2].x , allpoints[iterator-2].y );
        p2 = new Vector2(allpoints[iterator-1].x , allpoints[iterator-1].y );
        p3 = new Vector2(allpoints[iterator].x , allpoints[iterator].y );
        startOfEp = DateTime.UtcNow;

        TargetShifter();
        r.transform.forward = -currCollider.transform.forward;
        Vector3 tempvec;
        if (iterator < 5)
            tempvec = new Vector3(0, 23, 0);
        else if (iterator < 10)
        {
            tempvec = new Vector3(0, 16, 0);
        }
        else if (iterator < 15)
            tempvec = new Vector3(0, 5, 0);
        else if (iterator < 20)
            tempvec = new Vector3(0, 2.4f, 0);
        else if (iterator < 30)
            tempvec = new Vector3(0, -9 - (30 - iterator) / 2.2f, 0);
        else if (iterator < 35)
        {
            tempvec = new Vector3(0, -19 - (35 - iterator) / 2.2f, 0);
            if (iterator == 34)
                tempvec = new Vector3(0, -30, 0);
            
        }
        else if (iterator < 39)
            tempvec = new Vector3(0, -32, 0);
        else
            tempvec = new Vector3(0, -35, 0);
        r.position = currCollider.transform.position + tempvec;
        //Debug.Log(iterator);
    }

    public void TargetCreator()
    {
        currCollider = GameObject.CreatePrimitive(PrimitiveType.Cube);
        BoxCollider gg = currCollider.GetComponent<BoxCollider>() as BoxCollider;
        MeshRenderer gg2 = currCollider.GetComponent<MeshRenderer>() as MeshRenderer;
        //Destroy(gg2);
        gg.isTrigger = true;
        currCollider.tag = "Goal";
        currCollider.name = "Current";
        currCollider.transform.localScale = new Vector3(31, 100, 6);
        TargetShifter();
    }

    public void TargetShifter()
    {
        currCollider.transform.forward = new Vector3((p1 - p2).x, 0, (p1 - p2).y);
        currCollider.transform.position = new Vector3(p1.x, parent.transform.position.y, p1.y) + new Vector3(-134.2f,69.8f,-160.7f);

    }

    public override void CollectObservations()
    {
        Vector2 cartemp = new Vector2(r.transform.position.x, r.transform.position.z);
        AddVectorObs(p1 - cartemp + offsetv2);
        AddVectorObs(p2 - cartemp + offsetv2);
        AddVectorObs(p3 - cartemp + offsetv2);

        Vector3 cardir = r.transform.forward;
        float ang = Vector2.Angle((p1 - p2), new Vector2(cardir.x, cardir.z));
        AddVectorObs(ang);

        // Agent velocity
        AddVectorObs(r.velocity.x);
        AddVectorObs(r.velocity.z);

        //Debug.Log(ang+" "+r.velocity.x % 1f+" "+r.velocity.y % 1f);
    }
    
    public override void AgentAction(float[] act)
    {

        int throtact = (int)act[0];
        int steeract = (int)act[1];
        int throtfactor = 0;
        int steerfactor = 0;

        if (throtact == 1)
        {
            throtfactor = -1;
             //Debug.Log("Backward");
            AddReward(-0.5f);
        }
        else if (throtact == 2)
        {
            throtfactor = 1;
        }
        if (steeract == 1)
            steerfactor = 1;
        else if (steeract == 2)
            steerfactor = -1;

        float torq = strengthCoefficient * Time.deltaTime * throtfactor;
        float steerforce = maxTurn * steerfactor;

        foreach (WheelCollider wheel in throttleWheels)
        {
            wheel.motorTorque = torq;
        }
        foreach (WheelCollider wheel in steeringWheels)
        {
            wheel.steerAngle = steerforce;
        }

        Vector3 cartemp = r.transform.position;
        float temp = Vector2.Distance(p1, new Vector2(cartemp.x, cartemp.z));
        float MSE_loss = -temp*temp/100000;


        Vector3 cardir = r.transform.forward;
        float ang = Vector2.Angle((p1 - p2), new Vector2(cardir.x, cardir.z));
        AddReward(MSE_loss+(180-ang)/360);

        /*

        //CODE FOR ANGULAR DIFF LOSS
        Vector3 cardir = this.transform.forward;
        Vector3 tempVec = r.position-targets[iterator].position;
        float sign = (r.position.z < targets[iterator].position.z)? -1.0f : 1.0f;
        float ang = Vector3.Angle(cardir , tempVec);
        float Ang_loss = -(180f-ang)/360;

        AddReward(Time_loss);
        AddReward(2.2f*MSE_loss);
        AddReward(Ang_loss);
        */

        TimeSpan difference = -startOfEp.Subtract(DateTime.UtcNow);

        if (difference.TotalSeconds > 16)
        {
            startOfEp = DateTime.UtcNow;
            Done();
        }
        
    }

    public override float[] Heuristic()
    {
        var action = new float[2];
        action[0] = Input.GetAxis("Vertical");
        action[1] = Input.GetAxis("Horizontal");
        if (action[0] == -1)
            action[0] = 1;
        else if (action[0] == 1)
            action[0] = 2;
        if (action[1] == -1)
            action[1] = 2;
        return action;

    }

    public void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name == "Grid")
        {
            AddReward(-500f);
            startOfEp = DateTime.UtcNow;
            Done();
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        startOfEp = DateTime.UtcNow;
        iterator++;
        AddReward(100f);
        if (iterator == lineTarg.positionCount)
        {
            Done();
        }
        //Debug.Log(iterator);
        p1 = p2;
        p2 = p3;
        p3 = new Vector2(allpoints[iterator].x , allpoints[iterator].y );
        TargetShifter();
    }

}
