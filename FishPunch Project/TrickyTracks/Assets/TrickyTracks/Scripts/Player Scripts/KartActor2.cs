using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Written By Angus Secomb
//Last edited 20/10/17
public class KartActor2 : MonoBehaviour {


    //Lap info.
    //Private lap manager instance.
    private LapsManager lapManager;
    
    //Public lap related varaibles.
    [HideInInspector]
    public GameObject lastCheckPoint;
    [HideInInspector]
    public GameObject nextCheckPoint;
    [HideInInspector]
    public int checkPointCounter = 0;
    [HideInInspector]
    public int kartPosition = 0;
    [HideInInspector]
    public int finalPosition = 0;
    [HideInInspector]
    public int lapNumber = 0;

    //Checkpoint checks
    bool check1 = false;
    bool check2 = false;
    bool check3 = false;
    bool check4 = false;

    private List<bool> checks = new List<bool>();


    //Bool to assign new traps on lap pass.
    [HideInInspector]
    public bool assignNewTraps = false;

    //Karts rigidbody
    Rigidbody kartBody;
    
    //Xbox controller.
    [HideInInspector]
    public xbox_gamepad gamepad;
    
    //Object to apply mesh to turn on and off
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
    private Vector3 groundedPosition;

    private GameObject checkPointPosition;

    public int playerNumber = 1;
    public int numberOfPlayers = 4;

    public Camera playerCamera;
   void Awake()
    {
     

    }

    // Use this for initialization
    void Start () {
        //Create copies of the default values to be able to reset them.
        forwardAccelerationCopy = forwardAcceleration;
        backwardAccelerationCopy = reverseAcceleration;
        maxVelocityCopy = maxVelocity;
        turnStrengthCopy = turnStrength;

        lapManager = GameObject.Find("Manager").GetComponent<LapsManager>();
        
        

        checkPointPosition = GameObject.Find("RespawnPoint");
        
        //Gets karts rigid body and sets the centre of mass.
        kartBody = GetComponent<Rigidbody>();
        kartBody.centerOfMass = Vector3.down;

        

        //   SetUpCamera();

     

    }

    public void Instantiate(int playerNumber, int numberOfPlayers)
    {
       

    }

    //private void SetUpCamera()
    //{
    //    float cameraWidth = 1;
    //    float cameraHeight = 0.5f;

    //    float cameraXpos = 0;
    //    float cameraYpos = 0.5f;

    //    if (numberOfPlayers == 2)
    //    {
    //        cameraWidth = 1;
    //    }
    //    else 
    //    {
    //        cameraWidth = 0.5f;
    //    }

    //    if (playerNumber == 2){
    //        cameraYpos = 0;
    //        if(numberOfPlayers > 2)
    //        {
    //            cameraXpos = 0.5f;
    //        }
    //        else
    //        {
    //            cameraXpos = 0;
    //        }
    //    }
    //    else if(playerNumber == 3)
    //    {
    //        cameraXpos = 0.5f;
    //    }

    //    playerCamera.rect = new Rect(cameraXpos, cameraYpos, cameraWidth, cameraHeight);


    //}

    // Update is called once per frame
    void Update() {

        Debug.Log("Kart " + playerNumber + ": " + kartPosition);

        if(lapManager.endTime < 0 && lapNumber < 3)
        {
            finalPosition = kartPosition;
        }

        if(checkPointCounter == 0)
        {
            nextCheckPoint = lapManager.checkPoints[0];
            lastCheckPoint = lapManager.FinishLine;
        }

        //Audio

        //Gamepad assignment based on kart prefab NUMBAAA.
        switch (playerNumber)
        {
            case 1:
                gamepad = GamePadManager.Instance.GetGamePad(1);
                break;
            case 2:        
                gamepad = GamePadManager.Instance.GetGamePad(2);
                break;
            case 3:
                gamepad = GamePadManager.Instance.GetGamePad(3);
                break;
            case 4:
                gamepad = GamePadManager.Instance.GetGamePad(4);
                break;
            default:
                break;
            
        }

       
        //If the player hit the boost
        if (boostPlayer)
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

            //Turns the car mesh off while disabled.
            mesh.SetActive(false);

            //Reset kart position a little bit back.
            if (setOnce)
            {
                this.gameObject.transform.position = gameObject.transform.position + (transform.forward * -8);
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

                //If B is pressed start drift routine.
                if(gamepad.GetButtonDown("B") && !b_jumpCoolDown)
                {
                  transform.Translate(0, driftJumpValue * Time.deltaTime, 0);
                    //  transform.Rotate(0, 1000 * turnValue * Time.deltaTime, 0);
                    StartCoroutine(Drift());
                    b_jumpCoolDown = true;

                }

                // Jump cool down for drift.
                if (b_jumpCoolDown)
                {
                    jumpCoolDown += Time.deltaTime;
                }
                if(jumpCoolDown > 1)
                {
                    b_jumpCoolDown = false;
                    jumpCoolDown = 0.0f;
                }

                //2nd part of drift routine.
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
                    kartBody.AddRelativeTorque(gameObject.transform.up * 0.7f);
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

        //While the timer is less then the drift turn time rotate kart
        //based on their current turn value.
        while (timer < driftTurnTime)
        {
            timer += Time.deltaTime;

            transform.Rotate(driftRotation);


            yield return null;

        }
    }

    void OnTriggerEnter(Collider coll)
    {
        if (coll.gameObject.tag == "Checkpoint")
        {
            if(coll.gameObject.name == "CheckPoint1")
            {
                if(!check1 && !check2 && !check3 && !check4)
                {
                    check1 = true;
                    lastCheckPoint = lapManager.checkPoints[0];
                    nextCheckPoint = lapManager.checkPoints[1];
                    checkPointCounter += 1;
                }
            }

            if(coll.gameObject.name == "CheckPoint2")
            {
                if(check1 && !check2 && !check3 && !check4)
                {
                    check2 = true;
                    lastCheckPoint = lapManager.checkPoints[1];
                    nextCheckPoint = lapManager.checkPoints[2];
                    checkPointCounter += 1;
                }
            }

            if(coll.gameObject.name == "CheckPoint3")
            {
                if(check1 && check2 && !check3 && !check4)
                {
                    check3 = true;
                    lastCheckPoint = lapManager.checkPoints[2];
                    nextCheckPoint = lapManager.checkPoints[3];
                    checkPointCounter += 1;
                }
            }

            if(coll.gameObject.name == "CheckPoint4")
            {
                if(check1 && check2 && check3 && !check4)
                {
                    check4 = true;
                    lastCheckPoint = lapManager.checkPoints[3];
                    nextCheckPoint = lapManager.FinishLine;
                    checkPointCounter += 1;
                }
            }
        }

        if (coll.gameObject.tag == "StartLine")
        {
            if ((check1 && check2 && check3 && check4) && (lapNumber < 3))
            {
                lastCheckPoint = lapManager.FinishLine;
                lapNumber += 1;
                assignNewTraps = true;
                lapManager.lapNumber += 1;
                check1 = false;
                check2 = false;
                check3 = false;
                check4 = false;
                checkPointCounter = 0;
            }
            if (check1 && check2 && check3 && check4 && (lapNumber >= 3))
            {
                finalPosition = kartPosition;
            }

        }
        if (coll.gameObject.tag == "ItemRPG")
        {
            GameObject.Destroy(coll.gameObject.transform.parent.gameObject);

            if (!itemMine && !itemBoost)
            {
                itemRPG = true;
            }
        }

        if(coll.gameObject.tag == "Respawn")
        {
            this.transform.forward = lastCheckPoint.transform.forward;
            this.transform.position = lastCheckPoint.transform.position;
            playerDisabled = true;

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
