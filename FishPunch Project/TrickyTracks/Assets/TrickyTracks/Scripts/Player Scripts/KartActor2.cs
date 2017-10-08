﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KartActor2 : MonoBehaviour {



    //Karts rigidbody
    Rigidbody kartBody;
    
    //Xbox controller.
    [HideInInspector]
    public xbox_gamepad gamepad;

    public ParticleSystem[] wheelTrails = new ParticleSystem[2];

   
    public GameObject mesh;

    //Makes controller less sensitive to input.
    float deadZone = 0.1f;
    
    //Variables that control the karts movement.
    [Header("Kart Variables: ")]
    public float groundedDrag = 3f;
    public float maxVelocity = 30f;
    public float hoverForce = 1000f;
    public float gravityForce = 1000f;
    public float hoverHeight = 1.5f;
    public float forwardAcceleration = 8000f;
    public float reverseAcceleration = 4000f;
    public float turnStrength = 1000f;
    public float breakSharpness = 10.0f;
    public float driftBreakSharpness = 10.0f;
    public float driftDrag = 1.4f;
    public float driftJumpValue = 1.0f;
    public float driftTurnTime = 0.5f;
    public float driftTurnValue = 1.2f;
    public float speedDropOff = 1f;

    //Airborne variables
    [Header("Airborne division variables: ")]
    public float airbornDrag = 0.1f;
    public float airbornGravityForce = 800.0f;
    public float airbornThrust = 10f;
    public float airbornTurnValue = 100f;

    //Public trap variables.
    [Header("Trap variables: ")]
    public float respawnTime = 3.0f;
    public float boostValue = 10.0f;
    public float boostLength = 3.0f;
    public float tempTurnStrength = 500.0f;

    float audioTimer = 0f;

    //Array to store the empty game objects the raycasts fire from.
    [Header("Raycast Wheels: ")]
    public GameObject[] wheelPoints;

    //[Header("Wheel Collider/Mesh objects: ")]
    //public MeshCollider[] wheelColliders;
    //public MeshRenderer[] wheelRenderers;
  
    //public variables that traps and obstacles will use.
    [HideInInspector]
    public float thrust = 0f;
    [HideInInspector]
    public float turnValue = 0f;

    //Layer mask used to stop raycast interacting and hitting kart.
    int layerMask;

    //Private timers for items.
    private float boostTime = 0.0f;
    private float mineTime = 0.0f;

    //Private acceleration.
    float input_triggerAcceleration = 0.0f;
    float input_negativeTriggerAcceleration = 0.0f;

    bool setOnce = true;

    //item booleans.
    [HideInInspector]
    public bool itemBoost, itemMine, itemRPG = false;

    //Trap booleans.
    [HideInInspector]
    public bool playerDisabled, boostPlayer, placeMine, fireRPG = false;

    //Private copys
    private float forwardAccelerationCopy;
    private float backwardAccelerationCopy;
    private float maxVelocityCopy;
    private float turnStrengthCopy;


    private float jumpCoolDown = 0.0f;
    private bool b_jumpCoolDown = false;
   
    // Use this for initialization
    void Start () {
        forwardAccelerationCopy = forwardAcceleration;
        backwardAccelerationCopy = reverseAcceleration;
        maxVelocityCopy = maxVelocity;
        turnStrengthCopy = turnStrength;

        
        //Gets karts rigid body and sets the centre of mass.
        kartBody = GetComponent<Rigidbody>();
        kartBody.centerOfMass = Vector3.down;

        //Layer mask to ignore the kart.
        layerMask = 1 << LayerMask.NameToLayer("Vehicle");
        layerMask = ~layerMask;
	}

    // Update is called once per frame
    void Update() {

        //for(int i = 0; i <= 1; i++)
        //{
        //    wheelRenderers[i].transform.Rotate(0, 0, kartBody.velocity.sqrMagnitude * Time.deltaTime);
        //    wheelColliders[i].transform.Rotate(0, 0, kartBody.velocity.sqrMagnitude * Time.deltaTime);
        //    wheelRenderers[i + 2].transform.Rotate(0, 0, -kartBody.velocity.sqrMagnitude * Time.deltaTime);
        //    wheelColliders[i + 2].transform.Rotate(0, 0, -kartBody.velocity.sqrMagnitude * Time.deltaTime);
        //}


        //Audio

        //Gamepad assignment based on kart prefab name.
        switch(this.gameObject.name)
        {
            case "PlayerCharacter_001":
                gamepad = GamePadManager.Instance.GetGamePad(1);
                break;
            case "PlayerCharacter_002":        
                gamepad = GamePadManager.Instance.GetGamePad(2);
                break;
            case "PlayerCharacter_003":
                gamepad = GamePadManager.Instance.GetGamePad(3);
                break;
            case "PlayerCharacter_004":
                gamepad = GamePadManager.Instance.GetGamePad(4);
                break;
            default:
                break;
            
        }

        //If the player hit the boost
        if(boostPlayer)
        {
            //Add delta time to boost time and increase values temporarily.
            boostTime += Time.deltaTime;
           thrust = thrust + boostValue;
           forwardAcceleration = forwardAcceleration + boostValue;
           maxVelocity = 200.0f;
           forwardAcceleration = 20000f;
           turnStrength = tempTurnStrength;
           
            Debug.Log("item used");
        }

        //If the player hit the boost and boost time is > then the boost length.
        if(boostPlayer && (boostTime > boostLength))
        {
            //Boost player bool off and reset values back to default.
            boostPlayer = false;
            maxVelocity = maxVelocityCopy;
            forwardAcceleration = forwardAccelerationCopy;
            turnStrength = turnStrengthCopy;
            boostTime = 0.0f;
        }

        //If the player is disabled by the mine.
        if (playerDisabled)
        {
            //Add delta time to minetime.
            mineTime += Time.deltaTime;
                              //0, 0, 0
            //Reset the karts velocity to 0.
            kartBody.velocity = new Vector3();
            
            //Freeze all constraints.
            kartBody.constraints = RigidbodyConstraints.FreezeAll;

          
            mesh.SetActive(false);

            if (setOnce)
            {
                this.gameObject.transform.position = gameObject.transform.position + (transform.forward * -5);
                gameObject.transform.position = gameObject.transform.position + (transform.up * 2);
                setOnce = false;
            }
        
        }
        
        //When time is > than the respawn Time.
        if (playerDisabled && mineTime > respawnTime)
        {
            //Reset default values.
            playerDisabled = false;
            itemMine = false;
            mineTime = 0;
            kartBody.constraints = RigidbodyConstraints.None;
            mesh.SetActive(true);
            setOnce = true;

        }

        //If the player is not disabled they can control the kart.
        if (!playerDisabled)
        {
            //If the gamepad is connected.
            if (gamepad.IsConnected)
            {
                

                //Set acceleration to gamepad trigger values.
                input_triggerAcceleration = gamepad.GetTrigger_R();
                input_negativeTriggerAcceleration = gamepad.GetTrigger_L();

                //If the left trigger is down and thrust is > 0 reduce thrust based on break sharpness.
               
                    if (thrust > 0 && gamepad.GetTriggerTap_L())
                    {
                        
                        thrust -= breakSharpness;
                      

                    }


                //If thrust reaches the max lower the drag.
                if (kartBody.velocity.sqrMagnitude > (kartBody.velocity.normalized * maxVelocity).sqrMagnitude && gamepad.GetButton("B") == false)
                {
                    groundedDrag = 2.2f;
                }
                else if(gamepad.GetButton("B") == false)
                {
                    groundedDrag = 3.0f;
                }
               
                Debug.Log(groundedDrag);
          

                //Thrust set to 0 to slow car down when there is no input.
                if (thrust > 0.05 && !gamepad.GetTriggerTap_R())
                {
                    thrust -= speedDropOff;
                }

                //If the input is greater than the dead zone accelerator forward
                if (input_triggerAcceleration > deadZone)
                {
                   
                    thrust = input_triggerAcceleration * forwardAcceleration;
                }

                //if the input is less then the negative deadzone than accelerate backwards.
                if (thrust <= 100.0f)
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
                if(gamepad.GetButtonDown("B") && !b_jumpCoolDown)
                {
                  transform.Translate(0, driftJumpValue * Time.deltaTime, 0);
                    //  transform.Rotate(0, 1000 * turnValue * Time.deltaTime, 0);
                    StartCoroutine(Drift());
                    b_jumpCoolDown = true;

                }
                if (b_jumpCoolDown)
                {
                    jumpCoolDown += Time.deltaTime;
                }
                if(jumpCoolDown > 1)
                {
                    b_jumpCoolDown = false;
                    jumpCoolDown = 0.0f;
                }

                if(gamepad.GetButton("B") && thrust > 1f)
                {
                    kartBody.AddForce(transform.right * turnValue * 5);
                    thrust -= driftBreakSharpness;   
                    turnValue /= driftTurnValue;
                    groundedDrag = driftDrag;
                    
                }
             
            }
            // If gamepad is not connected use keyboard controls.
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
       
    }

    void FixedUpdate()
    {
        RaycastHit hit;
        bool grounded = false;

        

        //if i is less than the wheel points length.
        for (int i = 0; i < wheelPoints.Length; ++i)
        {
            var wheelPoint = wheelPoints[i];
            
            //shoot a raycast and work out the force to apply to each wheel point.
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

        //If grounded = true drag = the grounded drag.
        if (grounded)
        {
            kartBody.drag = groundedDrag;
         
        }
        else if(!grounded)
        {
            //If its airborne edit values.
            gravityForce = airbornGravityForce;
            kartBody.drag = airbornDrag;
            thrust /= airbornThrust;
            turnValue /= airbornTurnValue;
        }

        //Handle accelerating forward and reversing forces.
        if (Mathf.Abs(thrust) > 0)
        {
            kartBody.AddForce(transform.forward * thrust);
        }

        //Only turn when your going forward.
        if (thrust > 50.0f)
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
        if(thrust < -150.0f)
        {
            //Turn right
            if (turnValue > 0)
            {
                kartBody.AddRelativeTorque(Vector3.up * -turnValue * turnStrength);
            }
            //turn left.
            else if (turnValue < 0)
            {
                kartBody.AddRelativeTorque(Vector3.up * -turnValue * turnStrength);
            }

        }

        

        //Limits the cars velocity based on max velocity.
        if(kartBody.velocity.sqrMagnitude > (kartBody.velocity.normalized * maxVelocity).sqrMagnitude)
        {
            kartBody.velocity = kartBody.velocity.normalized * maxVelocity;
        }
    }

    IEnumerator Drift()
    {
        var timer = 0f;
        var driftRotation = new Vector3(0, (turnValue * 30) * Time.deltaTime, 0);

        while (timer < driftTurnTime)
        {
            timer += Time.deltaTime;

            transform.Rotate(driftRotation);


            yield return null;

        }
    }

    void OnTriggerEnter(Collider coll)
    {


        if (coll.gameObject.tag == "ItemRPG")
        {
            GameObject.Destroy(coll.gameObject.transform.parent.gameObject);
            itemRPG = true;
        }
    }

    void OnCollisionEnter(Collision coll)
    {
        if(coll.gameObject.tag == "RPG")
        {
            GameObject.Destroy(coll.gameObject.transform.parent.gameObject);
            playerDisabled = true;
        }
        
    }

}
