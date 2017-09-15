using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KartMovementScript : MonoBehaviour {

    //Floats engine force controls speed.
    //steering force controls how sharp you turn.
    public float engineForce;
    public float steeringForce;

    //Wheel Colliders.
    public WheelCollider wheelFR, wheelFL, wheelBR, wheelBL;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
        float input_v = Input.GetAxis("Vertical") * engineForce;
        float input_h = Input.GetAxis("Horizontal") * steeringForce;

        wheelBL.motorTorque = input_v;
        wheelBR.motorTorque = input_v;

        wheelFR.steerAngle = input_h;
        wheelFL.steerAngle = input_h;
    }
}