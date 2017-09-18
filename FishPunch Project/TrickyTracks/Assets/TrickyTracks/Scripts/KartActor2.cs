using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KartActor2 : MonoBehaviour {

    //Karts rigidbody
    Rigidbody kartBody;
    
    //Xbox controller.
    xbox_gamepad gamepad;
    

    //Makes controller less sensitive to input.
    float deadZone = 0.1f;

    //Variables that control the karts movement.
    public float groundedDrag = 3f;
    public float maxVelocity = 50f;
    public float hoverForce = 1000f;
    public float gravityForce = 1000f;
    public float hoverHeight = 1.5f;
    public float forwardAcceleration = 8000f;
    public float reverseAcceleration = 4000f;
    public float turnStrength = 1000f;
    public float breakSharpness = 10.0f;

    //Array to store the empty game objects the raycasts fire from.
    public GameObject[] wheelPoints;
  
    //public variables that traps and obstacles will use.
    [HideInInspector]
    public float thrust = 0f;
    [HideInInspector]
    public float turnValue = 0f;

    //Layer mask used to stop raycast interacting and hitting kart.
    int layerMask;

    float input_triggerAcceleration = 0.0f;
    float input_negativeTriggerAcceleration = 0.0f;

    // Use this for initialization
    void Start () {

        //Gets karts rigid body and sets the centre of mass.
        kartBody = GetComponent<Rigidbody>();
        kartBody.centerOfMass = Vector3.down;

        layerMask = 1 << LayerMask.NameToLayer("Vehicle");
        layerMask = ~layerMask;
	}

    // Update is called once per frame
    void Update() {

        gamepad = GamePadManager.Instance.GetGamePad(1);
    
        if (gamepad.IsConnected)
        {
                    
           input_triggerAcceleration = gamepad.GetTrigger_R();                   
           input_negativeTriggerAcceleration = gamepad.GetTrigger_L();


            if (gamepad.GetTriggerTap_L())
            {
                if (thrust > 0)
                {
                    thrust -= breakSharpness;
                }
            }

            //Thrust set to 0 to slow car down when there is no input.
            thrust = 0.0f;

            //If the input is greater than the dead zone accelerator forward
            if (input_triggerAcceleration > deadZone)
            {
                thrust = input_triggerAcceleration * forwardAcceleration;
            }
            
            //if the input is less then the negative deadzone than accelerate backwards.
            if (thrust <= 0)
            {
                if (input_negativeTriggerAcceleration > deadZone)
                {
                    thrust = -input_negativeTriggerAcceleration * reverseAcceleration;
                }
            }
            //Turn value set to 0 to brng wheels back to centre when theres no input.
            turnValue = 0.0f;
            float stickInputTurn = gamepad.GetStick_L().X;
            //if turning input is greater than the deadzone turn the wheels.
            if (Mathf.Abs(stickInputTurn) > deadZone)
            {
                turnValue = stickInputTurn;
            }
        }
        else
        {
            thrust = 0.0f;
            float input_acceleration = Input.GetAxis("Vertical");

            //If the input is greater than the dead zone accelerator forward
            if (input_acceleration > deadZone)
            {
                thrust = input_acceleration * forwardAcceleration;
            }
            //if the input is less then the negative deadzone than accelerate backwards.
            else if (input_acceleration < -deadZone)
            {
                thrust = input_acceleration * reverseAcceleration;
            }

            //Turn value set to 0 to brng wheels back to centre when theres no input.
            turnValue = 0.0f;
            float input_turn = Input.GetAxis("Horizontal");
            //if turning input is greater than the deadzone turn the wheels.
            if (Mathf.Abs(input_turn) > deadZone)
            {
                turnValue = input_turn;
            }
        }
    }

    void FixedUpdate()
    {
        RaycastHit hit;
        bool grounded = false;
        for (int i = 0; i < wheelPoints.Length; ++i)
        {
            var wheelPoint = wheelPoints[i];

            if (Physics.Raycast(wheelPoint.transform.position, -Vector3.up, out hit, hoverHeight, layerMask))
            {
                kartBody.AddForceAtPosition(Vector3.up * hoverForce * (1.0f - (hit.distance / hoverHeight)), wheelPoint.transform.position);
                grounded = true;
            }
            else
            {
                if (transform.position.y > wheelPoint.transform.position.y)
                {
                    kartBody.AddForceAtPosition(wheelPoint.transform.up * gravityForce, wheelPoint.transform.position);
                }
                else
                {
                    kartBody.AddForceAtPosition(wheelPoint.transform.up * -gravityForce, wheelPoint.transform.position);
                }
            }
        }

        if (grounded)
        {
            kartBody.drag = groundedDrag;
        }
        else
        {
            kartBody.drag = 0.1f;
            thrust /= 100f;
            turnValue /= 100f;
        }

        //Handle accelerating forward and reversing forces.
        if (Mathf.Abs(thrust) > 0)
        {
            kartBody.AddForce(transform.forward * thrust);
        }

        if (thrust > 0 || thrust < 0)
        {
            //Turn right
            if (turnValue > 0)
            {
                kartBody.AddRelativeTorque(Vector3.up * turnValue * turnStrength);
            }
            //turn left.
            else if (turnValue < 0)
            {
                kartBody.AddRelativeTorque(Vector3.up * turnValue * turnStrength);
            }
        }

        //Limits the cars velocity based on max velocity.
        if(kartBody.velocity.sqrMagnitude > (kartBody.velocity.normalized * maxVelocity).sqrMagnitude)
        {
            kartBody.velocity = kartBody.velocity.normalized * maxVelocity;
        }
    }
}
