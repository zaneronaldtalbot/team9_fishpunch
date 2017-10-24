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
    public float driftTraction = 0f;
    public float driftDeceleration = 0.05f;

    [HideInInspector]
    public xbox_gamepad gamepad;

    private Vector3 groundedPosition;

    private GameObject checkPointPosition;

    //Layer mask used to stop raycast interacting and hitting kart.
    int layerMask;

    // Use this for initialization
    void Start () {
        kart = GetComponent<KartController>();

       // lapManager = GameObject.Find("Manager").GetComponent<LapsManager>();
        checkPointPosition = GameObject.Find("RespawnPoint");

        //Layer mask to ignore the kart.
        layerMask = 1 << LayerMask.NameToLayer("Vehicle");
        layerMask = ~layerMask;
    }

    // Update is called once per frame
    void Update() {

        //if (lapManager.endTime < 0 && lapNumber < 3)
        //{
        //    finalPosition = kartPosition;
        //}

        //if (checkPointCounter == 0)
        //{
        //    nextCheckPoint = lapManager.checkPoints[0];
        //    lastCheckPoint = lapManager.FinishLine;
        //}

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
              Debug.Log(kart.physicsBody.velocity.sqrMagnitude);
        if (gamepad.GetTriggerDown_R())
        {
            kart.Thrust = gamepad.GetTrigger_R();
        }
        else if(!gamepad.GetTriggerDown_L())
        {
            kart.Thrust = 0;
        }


        if (gamepad.GetTriggerDown_L())
        {
            kart.Thrust = -gamepad.GetTrigger_L();
        }
        else if(!gamepad.GetTriggerDown_R())
        {
            kart.Thrust = 0.0f;
        }
        kart.Steering = gamepad.GetStick_L().X;

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
            
	}
    void OnTriggerEnter(Collider coll)
    {
        if (coll.gameObject.tag == "Checkpoint")
        {
            if (coll.gameObject.name == "CheckPoint1")
            {
                if (!check1 && !check2 && !check3 && !check4)
                {
                    check1 = true;
                    lastCheckPoint = lapManager.checkPoints[0];
                    nextCheckPoint = lapManager.checkPoints[1];
                    checkPointCounter += 1;
                }
            }

            if (coll.gameObject.name == "CheckPoint2")
            {
                if (check1 && !check2 && !check3 && !check4)
                {
                    check2 = true;
                    lastCheckPoint = lapManager.checkPoints[1];
                    nextCheckPoint = lapManager.checkPoints[2];
                    checkPointCounter += 1;
                }
            }

            if (coll.gameObject.name == "CheckPoint3")
            {
                if (check1 && check2 && !check3 && !check4)
                {
                    check3 = true;
                    lastCheckPoint = lapManager.checkPoints[2];
                    nextCheckPoint = lapManager.checkPoints[3];
                    checkPointCounter += 1;
                }
            }

            if (coll.gameObject.name == "CheckPoint4")
            {
                if (check1 && check2 && check3 && !check4)
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

        if (coll.gameObject.tag == "Respawn")
        {
            this.transform.forward = lastCheckPoint.transform.forward;
            this.transform.position = lastCheckPoint.transform.position;
            playerDisabled = true;

        }
    }

    void OnCollisionEnter(Collision coll)
    {
        if (coll.gameObject.tag == "RPG")
        {
            GameObject.Destroy(coll.gameObject.transform.parent.gameObject);
            playerDisabled = true;
        }

    }

}
