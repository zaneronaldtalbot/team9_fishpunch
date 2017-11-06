using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Actor uses altered kart controller
//by deercat from the asset store.

[RequireComponent(typeof(KartController))]
public class PlayerActor : MonoBehaviour {

    //Lap info.
    //Private lap manager instance.
    private LapsManager lapManager;

    private NewPlacementController npcManager;

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
    public int lapNumber = 1;

    //Checkpoint checks
    bool check1 = false, check2 = false, check3 = false, check4 = false, check5 = false, check6 = false,
         check7 = false, check8 = false, check9 = false, check10 = false, check11 = false, check12 = false;

    private List<bool> checks = new List<bool>();

    private float raceCountdownTimer = 3.0f;


    //Bool to assign new traps on lap pass.
    [HideInInspector]
    public bool assignNewTraps = false;

    //Karts rigidbody
    Rigidbody kartBody;

    //Object to apply mesh to turn on and off
    public GameObject mesh;

    //item booleans.
    [HideInInspector]
    public bool itemBoost, itemMine, itemRPG = false;

    //Trap booleans.
    [HideInInspector]
    public bool playerDisabled, boostPlayer, placeMine, fireRPG = false;

    bool setOnce = true;

    KartController kart;

    public int playerNumber = 1;
    public int numberOfPlayers = 4;
    public float breakPower = 20.0f;
    [Tooltip("The traction the car has while drifting.")]
    public float driftTraction = 0f;
    [Tooltip("How fast the car decelerates while drifting.")]
    public float driftDeceleration = 0.05f;
    [Tooltip("How long the car spins out for when it hits an oil slick.")]
    public float slickSpinOutTime = 3.0f;

    [HideInInspector]
    public xbox_gamepad gamepad;

    private Vector3 groundedPosition;

    private GameObject checkPointPosition;

    private float disabledTimer = 0.0f;

    private bool immuneToDamage = false;

    [Tooltip("How long the karts steering locks for when it hits an oil slick")]
    public float lockSteerTime = 2.0f;
    [Tooltip("How much the oil slick slows the car down when it hits one.")]
    public float slickSpeedPenalty = 0.5f;
    private bool lockSteer = false;
    private float steerAngleCopy;

    [HideInInspector]
    public bool hitSlick = false;

    //Layer mask used to stop raycast interacting and hitting kart.
    int layerMask;

    // Use this for initialization
    void Start () {
        kart = GetComponent<KartController>();
      
        lapManager = GameObject.Find("Manager").GetComponent<LapsManager>();
        npcManager = GameObject.Find("Manager").GetComponent<NewPlacementController>();
    //    checkPointPosition = GameObject.Find("RespawnPoint");
        lapNumber = 1;
        steerAngleCopy = kart.maxSteerAngle;
      //  lockSteerTime = slickSpinOutTime;
        //Layer mask to ignore the kart.
        layerMask = 1 << LayerMask.NameToLayer("Vehicle");
        layerMask = ~layerMask;


    }

    // Update is called once per frame
    void Update() {

        kartBody = kart.physicsBody;
        //If counter = 0 finish line = last checkpoint and first checkpoint = next.
        if (checkPointCounter == 0)
        {
            nextCheckPoint = lapManager.checkPoints[0];
            lastCheckPoint = lapManager.FinishLine;
        }

        //if (lapManager.endTime < 0 && lapNumber < 3)
        //{
        //    finalPosition = kartPosition;
        //}
        //If kart hits slick spin kart for set time.
        if (hitSlick)
           {

                kart.Spin(slickSpinOutTime);
                kart.SpeedPenalty(slickSpeedPenalty, lockSteerTime, 1.0f);
                lockSteer = true;
                hitSlick = false;
           }
           if(lockSteer)
            {
                lockSteerTime -= Time.deltaTime;
            kart.maxSteerAngle = 0.0f;
            if(lockSteerTime < 0)
            {
                kart.maxSteerAngle = steerAngleCopy;
                lockSteerTime = slickSpinOutTime;
                lockSteer = false;
            }
          
            if(boostPlayer)
            {
                kart.SpeedBoost(100.0f, 1.0f, 3.0f, 1.0f);
                boostPlayer = false;
            } 
        }
        raceCountdownTimer -= Time.deltaTime;
        if(raceCountdownTimer > 0)
        {
            kartBody.constraints = RigidbodyConstraints.FreezeAll;
        }
        else
        {
            kartBody.constraints = RigidbodyConstraints.None;
        }
           //If the player is disabled.
        if(playerDisabled)
        {
            //Freeze rigidbody, disable mesh, reset velocity, move kart slightly back and up.
            immuneToDamage = true;
            disabledTimer += Time.deltaTime;
            kartBody.velocity = new Vector3();

            kartBody.constraints = RigidbodyConstraints.FreezeAll;
            mesh.SetActive(false);

            if(setOnce)
            {
                this.gameObject.transform.position = gameObject.transform.position + (transform.forward * -10);
                gameObject.transform.position = gameObject.transform.position + (transform.up * 2);
                setOnce = false;
            }

            //If disable timer is greater than 0 take constraints off car
            //turn mesh on and turn off/on relevant bools.
            if(disabledTimer > 2.0f)
            {
                kartBody.constraints = RigidbodyConstraints.None;
                disabledTimer = 0.0f;
                playerDisabled = false;
                immuneToDamage = false;
                mesh.SetActive(true);
                setOnce = true;
            }

        }

      

        //Acquire gamepad based on player number
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

        //If trigger down go forward else if left trigger is down
        //bring thrust back to 0.
        if (gamepad.GetTriggerDown_R())
        {
            kart.Thrust = gamepad.GetTrigger_R();
        }
        else if(!gamepad.GetTriggerDown_L())
        {
            kart.Thrust = 0;
        }

        //If trigger down go backward else if right trigger isnt down
        //bring thrust back to 0.
        if (gamepad.GetTriggerDown_L())
        {
            kart.Thrust = -gamepad.GetTrigger_L();
        }
        else if(!gamepad.GetTriggerDown_R())
        {
            kart.Thrust = 0.0f;
        }

        //Steering left stick X axis input.
        kart.Steering = gamepad.GetStick_L().X;


        //If b button traction, deceleration and breakpower
        // = set values else reset to defaults.
        if (gamepad.GetButton("B"))
        {
            if (kart.Steering != 0 && kart.physicsBody.velocity.sqrMagnitude > 50.0f)
            {
                kart.traction = driftTraction;

                kart.decelerationSpeed = driftDeceleration;
                kart.breakPower = breakPower;
            }
            
        }
        else
        {
            kart.decelerationSpeed = kart.decelerationSpeedCopy;
            kart.breakPower = 0.0f;
            kart.traction = 0.4f;
        }

        if(gamepad.GetButton("Back"))
        {
            this.transform.forward = lastCheckPoint.transform.forward;
            this.transform.position = lastCheckPoint.transform.position; this.transform.forward = lastCheckPoint.transform.forward;
            this.transform.position = lastCheckPoint.transform.position;
        }

        Debug.Log(checkPointCounter);
	}


    void OnTriggerEnter(Collider coll)
    {
        //Checks for collisions with checkpoints sets a bool to true for each checkpoint passed
        //if they havent passed a certain checkpoint they will not be able to go to the next lap.
        if (coll.gameObject.tag == "Checkpoint")
        {
            if (coll.gameObject.name == "CheckPoint1")
            {
                if (!check1 && !check2 && !check3 && !check4 && !check5 && !check6
                    && !check7 && !check8 & !check9 && !check10 && !check11 && !check12)
                {
                    check1 = true;
                    lastCheckPoint = lapManager.checkPoints[0];
                    nextCheckPoint = lapManager.checkPoints[1];
                    checkPointCounter += 1;
                }
            }

            if (coll.gameObject.name == "CheckPoint2")
            {
                if (check1 && !check2 && !check3 && !check4 && !check5 && !check6
                    && !check7 && !check8 && !check9 && !check10 && !check11 && !check12)
                {
                    check2 = true;
                    lastCheckPoint = lapManager.checkPoints[1];
                    nextCheckPoint = lapManager.checkPoints[2];
                    checkPointCounter += 1;
                }
            }

            if (coll.gameObject.name == "CheckPoint3")
            {
                if (check1 && check2 && !check3 && !check4 && !check5 && !check6
                    && !check7 && !check8 && !check9 && !check10 && !check11 && !check12)
                {
                    check3 = true;
                    lastCheckPoint = lapManager.checkPoints[2];
                    nextCheckPoint = lapManager.checkPoints[3];
                    checkPointCounter += 1;
                }
            }

            if (coll.gameObject.name == "CheckPoint4")
            {
                if (check1 && check2 && check3 && !check4 && !check5 && !check6 
                    && !check7 && !check8 && !check9 && !check10 && !check11 && !check12)
                {
                    check4 = true;
                    lastCheckPoint = lapManager.checkPoints[3];
                    nextCheckPoint = lapManager.checkPoints[4];
                    checkPointCounter += 1;
                }
            }

            if(coll.gameObject.name == "CheckPoint5")
            {
                if(check1 && check2 && check3 && check4 && !check5 && !check6 &&
                   !check7 && !check8 && !check9 && !check10 && !check11 && !check12)
                {
                    check5 = true;
                    lastCheckPoint = lapManager.checkPoints[4];
                    nextCheckPoint = lapManager.checkPoints[5];
                    checkPointCounter += 1;
                }
            }

            if (coll.gameObject.name == "CheckPoint6")
            {
                if (check1 && check2 && check3 && check4 && check5 && !check6 &&
                   !check7 && !check8 && !check9 && !check10 && !check11 && !check12)
                {
                    check6 = true;
                    lastCheckPoint = lapManager.checkPoints[5];
                    nextCheckPoint = lapManager.checkPoints[6];
                    checkPointCounter += 1;
                }
            }

            if (coll.gameObject.name == "CheckPoint7")
            {
                if (check1 && check2 && check3 && check4 && check5 && check6 &&
                   !check7 && !check8 && !check9 && !check10 && !check11 && !check12)
                {
                    check7 = true;
                    lastCheckPoint = lapManager.checkPoints[6];
                    nextCheckPoint = lapManager.checkPoints[7];
                    checkPointCounter += 1;
                }
            }

            if (coll.gameObject.name == "CheckPoint8")
            {
                if (check1 && check2 && check3 && check4 && check5 && check6 &&
                   check7 && !check8 && !check9 && !check10 && !check11 && !check12)
                {
                    check8 = true;
                    lastCheckPoint = lapManager.checkPoints[7];
                    nextCheckPoint = lapManager.checkPoints[8];
                    checkPointCounter += 1;
                }
            }

            if (coll.gameObject.name == "CheckPoint9")
            {
                if (check1 && check2 && check3 && check4 && check5 && check6 &&
                   check7 && check8 && !check9 && !check10 && !check11 && !check12)
                {
                    check9 = true;
                    lastCheckPoint = lapManager.checkPoints[8];
                    nextCheckPoint = lapManager.checkPoints[9];
                    checkPointCounter += 1;
                }
            }

            if (coll.gameObject.name == "CheckPoint10")
            {
                if (check1 && check2 && check3 && check4 && check5 && check6 &&
                   check7 && check8 && check9 && !check10 && !check11 && !check12)
                {
                    check10 = true;
                    lastCheckPoint = lapManager.checkPoints[9];
                    nextCheckPoint = lapManager.checkPoints[10];
                    checkPointCounter += 1;
                }
            }

            if (coll.gameObject.name == "CheckPoint11")
            {
                if (check1 && check2 && check3 && check4 && check5 && check6 &&
                   check7 && check8 && check9 && check10 && !check11 && !check12)
                {
                    check11 = true;
                    lastCheckPoint = lapManager.checkPoints[10];
                    nextCheckPoint = lapManager.checkPoints[11];
                    checkPointCounter += 1;
                }
            }

            if (coll.gameObject.name == "CheckPoint12")
            {
                if (check1 && check2 && check3 && check4 && check5 && check6 &&
                   check7 && check8 && check9 && check10 && check11 && !check12)
                {
                    check12 = true;
                    lastCheckPoint = lapManager.checkPoints[11];
                    nextCheckPoint = lapManager.FinishLine;
                    checkPointCounter += 1;
                }
            }
        }
        //If all checks are true and the karts lapnumber is less than 3
        //add a lap to the lap counter reset all bools and assign new traps.
        if (coll.gameObject.tag == "StartLine")
        {
            if ((check1 && check2 && check3 && check4 && check5 && check6
                && check7 && check8 && check9 && check10 && check11 && check12) && (lapNumber < 4))
            {
                lastCheckPoint = lapManager.FinishLine;
                nextCheckPoint = lapManager.checkPoints[0];
                lapNumber++;

                switch(playerNumber)
                {
                    case 1:
                        if(npcManager.itemsP1.Count <= 12)
                        {
                            assignNewTraps = true;
                        }
                        break;
                    case 2:
                        if (npcManager.itemsP2.Count <= 12)
                        {
                            assignNewTraps = true;
                        }
                        break;
                    case 3:
                        if(npcManager.itemsP3.Count <= 12)
                        {
                            assignNewTraps = true;
                        }
                        break;
                    case 4:
                        if(npcManager.itemsP4.Count <= 12)
                        {
                            assignNewTraps = true;
                        }
                        break;

                }
               
                lapManager.lapNumber += 1;
                check1 = false;
                check2 = false;
                check3 = false;
                check4 = false;
                check5 = false;
                check6 = false;
                check7 = false;
                check8 = false;
                check9 = false;
                check10 = false;
                check11 = false;
                check12 = false;
                checkPointCounter = 0;
            }
            //if (check1 && check2 && check3 && check4 && (lapNumber >= 3))
            //{
            //    finalPosition = kartPosition;
            //}

        }

        //If item rpg is hit destroy the powerup and if they do not have a mine or boost
        //set itemRPG to true.
        if (coll.gameObject.tag == "ItemRPG")
        {
            GameObject.Destroy(coll.gameObject.transform.parent.gameObject);

            if (!itemMine && !itemBoost)
            {
                itemRPG = true;
            }
        }

        //If they fall off the map and hit the killbox reset their position to the last checkpoint.
        if (coll.gameObject.tag == "Respawn")
        {
            this.transform.forward = lastCheckPoint.transform.forward;
            this.transform.position = lastCheckPoint.transform.position;
            playerDisabled = true;

        }
    }

    void OnCollisionEnter(Collision coll)
    {
        //If an rpg hits them and they are not immune to damage
        //Destroy the rpg and disable the player.
        if (coll.gameObject.tag == "RPG")
        {
            if (!immuneToDamage)
            {
                GameObject.Destroy(coll.gameObject.transform.parent.gameObject);
                playerDisabled = true;
            }
        }

    }

}
