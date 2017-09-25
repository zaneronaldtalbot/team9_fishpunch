using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KartActor : MonoBehaviour
{

    public float engineForce;
    public float steeringForce;

    //Private Wheel Colliders.
    private WheelCollider wheelFR;
    private WheelCollider wheelFL;
    private WheelCollider wheelBR;
    private WheelCollider wheelBL;

    //Private gameobjects
    private GameObject go_wheelBL;
    private GameObject go_wheelBR;
    private GameObject go_wheelFR;
    private GameObject go_wheelFL;

    // Use this for initialization
    void Start()
    {
        //Find car parts.

        //Back left wheel
        go_wheelBL = GameObject.Find("WheelCollBL");
        wheelBL = go_wheelBL.GetComponent<WheelCollider>();

        //Back Right Wheel
        go_wheelBR = GameObject.Find("WheelCollBR");
        wheelBR = go_wheelBR.GetComponent<WheelCollider>();

        //Front Left Wheel
        go_wheelFL = GameObject.Find("WheelCollFL");
        wheelFL = go_wheelFL.GetComponent<WheelCollider>();

        //Front Right Wheel
        go_wheelFR = GameObject.Find("WheelCollFR");
        wheelFR = go_wheelFR.GetComponent<WheelCollider>();
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
