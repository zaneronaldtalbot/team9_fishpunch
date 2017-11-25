using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

//Written by Angus Secomb
//Last edited 22/11/17
[RequireComponent(typeof(KartController))]
public class PlayerActor : MonoBehaviour {

    //Lap info.
    //Private lap manager instance.
    private LapsManager lapManager;

    private NewPlacementController npcManager;

    public ParticleSystem deathParticles;

    public ParticleSystem respawnParticles;

    public Material transparent;
    private Material matCopy;

    public Renderer wheel1, wheel2, wheel3, wheel4;

    private Material wheelCopy;

    public float immuneTimer = 4.0f;

    private float flashingTimer = 0.1f;
    private float ftCopy;

    private bool flashClear = false;
    private bool flashFull = false;

    public Renderer kartMat;

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

    private float overTurnCounter = 3;


    private List<bool> checks = new List<bool>();
    
    [HideInInspector]
    public float raceCountdownTimer = 3.0f;

    private Image greyBackDrop, pauseUI, pauseContinue, pauseMenu;

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

    float deadZone = 0.9f;

    float coolDown = 0.3f;
    float cdCopy = 0.3f;


    [HideInInspector]
    public KartController kart;

    public int playerNumber = 1;
    public int numberOfPlayers = 4;
    public float breakPower = 20.0f;
    [Tooltip("The traction the car has while drifting.")]
    public float driftTraction = 0f;
    [Tooltip("How fast the car decelerates while drifting.")]
    public float driftDeceleration = 0.05f;
    [Tooltip("How long the car spins out for when it hits an oil slick.")]
    public float slickSpinOutTime = 3.0f;

    public float boostPower = 150.0f;
    public float boostTime = 3.0f;

    [HideInInspector]
    public xbox_gamepad gamepad;

    private Vector3 groundedPosition;

    private GameObject checkPointPosition;

    private float disabledTimer = 0.0f;

    [HideInInspector]
    public bool immuneToDamage = false;

    [Tooltip("How long the karts steering locks for when it hits an oil slick")]
    public float lockSteerTime = 2.0f;
    [Tooltip("How much the oil slick slows the car down when it hits one.")]
    public float slickSpeedPenalty = 0.5f;
    private bool lockSteer = false;
    private float steerAngleCopy;

    private Image play, playerSelect, menu;

    private float steerSpeedCopy;

    [HideInInspector]
    public bool hitSlick = false;

    private bool isPaused = false;

    private int buttonIndex = 1;

    //Layer mask used to stop raycast interacting and hitting kart.
    int layerMask;

    // Use this for initialization
    void Start () {
        kart = GetComponent<KartController>();
        steerSpeedCopy = kart.steerSpeed;
        ftCopy = flashingTimer;
        lapManager = GameObject.Find("Manager").GetComponent<LapsManager>();
        npcManager = GameObject.Find("Manager").GetComponent<NewPlacementController>();
    //    checkPointPosition = GameObject.Find("RespawnPoint");
        lapNumber = 1;
        steerAngleCopy = kart.maxSteerAngle;
      //  lockSteerTime = slickSpinOutTime;
        //Layer mask to ignore the kart.
        layerMask = 1 << LayerMask.NameToLayer("Vehicle");
        layerMask = ~layerMask;
        matCopy = kartMat.material;
        wheelCopy = wheel1.material;
        //Acquire gamepad based on player number
        flashFull = true;

        greyBackDrop = GameObject.Find("GreyBackDrop").GetComponent<Image>();
        pauseUI = GameObject.Find("PauseUIPaused").GetComponent<Image>();
        pauseContinue = GameObject.Find("PauseUIContinue").GetComponent<Image>();
        pauseMenu = GameObject.Find("PauseUIMainMenu").GetComponent<Image>();

        for (int i = 1; i <= GamePadManager.Instance.ConnectedTotal(); ++ i)
        {
            xbox_gamepad temppad = GamePadManager.Instance.GetGamePad(i);
            switch (playerNumber)
            {
                case 1:
                    if (temppad.newControllerIndex == 1)
                    {
                        gamepad = temppad;
                    }
                  
                    break;
                case 2:
                    if (temppad.newControllerIndex == 2)
                    {
                        gamepad = temppad;
                    }
                    
                    break;
                case 3:
                    if(temppad.newControllerIndex == 3)
                    {
                        gamepad = temppad;
                    }
                    
                    break;
                case 4:
                    if(temppad.newControllerIndex == 4)
                    {
                        gamepad = temppad;
                    }
                    break;
                default:
                    break;

            }
        }


    }

    void FixedUpdate()
    {

     

    }

    // Update is called once per frame
    void Update() {

        Debug.Log(kart.physicsBody.velocity.sqrMagnitude);
        Debug.Log("Kart " + playerNumber + ": " + checkPointCounter);

        if(gamepad.GetButtonDown("Y"))
        {
            lapNumber++;
        }

        if (lapManager == null)
        {
            lapManager = GameObject.Find("Manager").GetComponent<LapsManager>();
        }

        kartBody = kart.physicsBody;
        //If counter = 0 finish line = last checkpoint and first checkpoint = next.
        //if (checkPointCounter == 0)
        //{
        if (lapManager != null)
        {
            if (checkPointCounter == 0)
            {
                if (lapManager.checkPoints != null)
                {
                    nextCheckPoint = lapManager.checkPoints[0];
                }
                if (lapManager.FinishLine != null)
                {
                    lastCheckPoint = lapManager.FinishLine;
                }
            }
        }


        if (raceCountdownTimer < 0)
        {
            if (gamepad.GetButtonDown("Start") && !isPaused && !npcManager.isPaused)
            {
                npcManager.isPaused = true;
                isPaused = true;
                Time.timeScale = 0;
            }
            else if (gamepad.GetButtonDown("Start") && isPaused)
            {
                isPaused = false;
                npcManager.isPaused = false;
                Time.timeScale = 1;
            }
        }
        if (isPaused)
        {
            coolDown -= Time.unscaledDeltaTime;
            if (buttonIndex == 1)
            {
                if (gamepad.GetStick_L().Y < -deadZone && coolDown < 0)
                {
                    coolDown = cdCopy;
                    buttonIndex = 2;
                }
                if(gamepad.GetButtonDown("A"))
                {
                    isPaused = false;
                    npcManager.isPaused = false;
                    Time.timeScale = 1;
                    npcManager.enabled = false;
                }
                if (gamepad.GetButtonUp("A"))
                {
                    npcManager.enabled = false;
                }
            }
            if(buttonIndex == 2)
            {
                if (gamepad.GetStick_L().Y > deadZone && coolDown < 0)
                {
                    coolDown = cdCopy;
                    buttonIndex = 1;
                }
                if (gamepad.GetButtonDown("A"))
                {
                    Time.timeScale = 1;
                    Destroy(GameObject.Find("Manager"));
                    SceneManager.LoadScene(0);
                }
            }
            greyBackDrop.enabled = true;
            pauseContinue.enabled = true;
            pauseMenu.enabled = true;
            pauseUI.enabled = true;

        }
        else if (!isPaused)
        {
            greyBackDrop.enabled = false;
            pauseContinue.enabled = false;
            pauseMenu.enabled = false;
            pauseUI.enabled = false;
        }

        if(buttonIndex == 1)
        {
            pauseContinue.color = Color.yellow;
            pauseMenu.color = Color.grey;
        }
        if (buttonIndex == 2)
        {
            pauseContinue.color = Color.grey;
            pauseMenu.color = Color.yellow;
        }

        //if (lapManager.endTime < 0 && lapNumber < 3)
        //{
        //    finalPosition = kartPosition;
        //}
        //If kart hits slick spin kart for set time.

        if (boostPlayer)
        {
            kart.SpeedBoost(boostPower, boostTime, 3.0f, 1.0f);
            boostPlayer = false;
        }


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
            Vector3 tempVector;
            deathParticles.Play();
            tempVector = deathParticles.transform.position;
            //Freeze rigidbody, disable mesh, reset velocity, move kart slightly back and up.
           

            disabledTimer += Time.deltaTime;
            kartBody.velocity = new Vector3();
            kartMat.material = transparent;
            kartBody.constraints = RigidbodyConstraints.FreezeAll;
            mesh.SetActive(false);

            if(setOnce)
            {
                this.gameObject.transform.position = gameObject.transform.position + (transform.forward * 12);
                gameObject.transform.position = gameObject.transform.position + (transform.up * 2);
                setOnce = false;
            }

            deathParticles.transform.position = tempVector;

            //If disable timer is greater than 0 take constraints off car
            //turn mesh on and turn off/on relevant bools.
            if(disabledTimer > 2.0f)
            {
                immuneToDamage = true;
                deathParticles.transform.position = kart.transform.position;
                deathParticles.Stop();
                respawnParticles.Play();
                kartBody.constraints = RigidbodyConstraints.None;
                disabledTimer = 0.0f;
                playerDisabled = false;
                
                mesh.SetActive(true);
                setOnce = true;
            }

           

        }

        if(immuneToDamage)
        {
            immuneTimer -= Time.deltaTime;
            flashingTimer -= Time.deltaTime;


            if(flashingTimer < 0)
            {
                flashingTimer = ftCopy;

                if(flashFull)
                {

                    kartMat.material = matCopy;
                    wheel1.material = wheelCopy;
                    wheel2.material = wheelCopy;
                    wheel3.material = wheelCopy;
                    wheel4.material = wheelCopy;
                    flashClear = true;
                    flashFull = false;
                }
                else if(flashClear)
                {
                    kartMat.material = transparent;
                    kartMat.material.color = Color.cyan;
                    wheel1.material = transparent;
                    wheel1.material.color = Color.cyan;
                    wheel2.material = transparent;
                    wheel2.material.color = Color.cyan;
                    wheel3.material = transparent;
                    wheel3.material.color = Color.cyan;
                    wheel4.material = transparent;
                    wheel4.material.color = Color.cyan;
                    flashClear = false;
                    flashFull = true;
                }
                
            }
          

            if (immuneTimer < 0)
            {
                respawnParticles.Stop();
                immuneToDamage = false;
                immuneTimer = 5.0f;
                kartMat.material = matCopy;
                wheel1.material = wheelCopy;
                wheel2.material = wheelCopy;
                wheel3.material = wheelCopy;
                wheel4.material = wheelCopy;
            }

        }
      


        //If trigger down go forward else if left trigger is down
        //bring thrust back to 0.
        if (gamepad.GetTriggerDown_R() && !lockSteer)
        {
            kart.Thrust = gamepad.GetTrigger_R();
        }
        else if(!gamepad.GetTriggerDown_L())
        {
            kart.Thrust = 0;
        }

        //If trigger down go backward else if right trigger isnt down
        //bring thrust back to 0.
        if (gamepad.GetTriggerDown_L() && !lockSteer)
        {
            kart.Thrust = -gamepad.GetTrigger_L();
        }
        else if(!gamepad.GetTriggerDown_R())
        {
            kart.Thrust = 0.0f;
        }

        //Steering left stick X axis input.
        kart.Steering = gamepad.GetStick_L().X;


        if(kart.IsOverturned)
        {
            overTurnCounter -= Time.deltaTime;

            if (overTurnCounter < 0)
            {
                this.transform.forward = lastCheckPoint.transform.forward;
                this.transform.position = lastCheckPoint.transform.position;
                playerDisabled = true;
                overTurnCounter = 3;
            }
        }

        //If b button traction, deceleration and breakpower
        // = set values else reset to defaults.
        if (gamepad.GetButton("B"))
        {
            if (kart.physicsBody.velocity.sqrMagnitude > 50.0f && kart.IsGrounded)
            {
                kart.steerSpeed = 0.4f;

                kart.decelerationSpeed = driftDeceleration;
                kart.breakPower = breakPower;
                if (kart.physicsBody.velocity.sqrMagnitude < 200)
                {
                    kart.traction = 0.4f;
                }
                else if (kart.physicsBody.velocity.sqrMagnitude > 200)
                {
                    kart.traction = driftTraction;
                }
            }
            
        }
        else
        {
            kart.decelerationSpeed = kart.decelerationSpeedCopy;
            kart.breakPower = 0.0f;
            kart.traction = 0.4f;
            kart.steerSpeed = steerSpeedCopy;
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
        //If they fall off the map and hit the killbox reset their position to the last checkpoint.
        if (coll.gameObject.tag == "Respawn")
        {
            this.transform.forward = lastCheckPoint.transform.forward;
            this.transform.position = lastCheckPoint.transform.position;
            playerDisabled = true;

        }
        //Checks for collisions with checkpoints sets a bool to true for each checkpoint passed
        //if they havent passed a certain checkpoint they will not be able to go to the next lap.
        if (coll.gameObject.tag == "Checkpoint")
        {
            if (coll.gameObject.name == "CheckPoint1")
            {
                if (!check1 && !check2 && !check3 && !check4 && !check5 && !check6
                    && !check7 && !check8 & !check9 && !check10 && !check11 && !check12)
                {
                    if (!check1)
                    {
                        check1 = true;
                        lastCheckPoint = lapManager.checkPoints[0];
                        nextCheckPoint = lapManager.checkPoints[1];
                        checkPointCounter += 1;
                    }
                }
            }

            if (coll.gameObject.name == "CheckPoint2")
            {
                if (check1 && !check2 && !check3 && !check4 && !check5 && !check6
                    && !check7 && !check8 && !check9 && !check10 && !check11 && !check12)
                {
                    if (!check2)
                    {
                        check2 = true;
                        lastCheckPoint = lapManager.checkPoints[1];
                        nextCheckPoint = lapManager.checkPoints[2];
                        checkPointCounter += 1;
                    }
                }
            }

            if (coll.gameObject.name == "CheckPoint3")
            {
                if (check1 && check2 && !check3 && !check4 && !check5 && !check6
                    && !check7 && !check8 && !check9 && !check10 && !check11 && !check12)
                {
                    if (!check3)
                    {
                        check3 = true;
                        lastCheckPoint = lapManager.checkPoints[2];
                        nextCheckPoint = lapManager.checkPoints[3];
                        checkPointCounter += 1;
                    }
                }
            }

            if (coll.gameObject.name == "CheckPoint4")
            {
                if (check1 && check2 && check3 && !check4 && !check5 && !check6 
                    && !check7 && !check8 && !check9 && !check10 && !check11 && !check12)
                {
                    if (!check4)
                    {
                        check4 = true;
                        lastCheckPoint = lapManager.checkPoints[3];
                        nextCheckPoint = lapManager.checkPoints[4];
                        checkPointCounter += 1;
                    }
                }
            }

            if(coll.gameObject.name == "CheckPoint5")
            {
                if(check1 && check2 && check3 && check4 && !check5 && !check6 &&
                   !check7 && !check8 && !check9 && !check10 && !check11 && !check12)
                {
                    if (!check5)
                    {
                        check5 = true;
                        lastCheckPoint = lapManager.checkPoints[4];
                        nextCheckPoint = lapManager.checkPoints[5];
                        checkPointCounter += 1;
                    }
                }
            }

            if (coll.gameObject.name == "CheckPoint6")
            {
                if (check1 && check2 && check3 && check4 && check5 && !check6 &&
                   !check7 && !check8 && !check9 && !check10 && !check11 && !check12)
                {
                    if (!check6)
                    {
                        check6 = true;
                        lastCheckPoint = lapManager.checkPoints[5];
                        nextCheckPoint = lapManager.checkPoints[6];
                        checkPointCounter += 1;
                    }
                }
            }

            if (coll.gameObject.name == "CheckPoint7")
            {
                if (check1 && check2 && check3 && check4 && check5 && check6 &&
                   !check7 && !check8 && !check9 && !check10 && !check11 && !check12)
                {
                    if (!check7)
                    {
                        check7 = true;
                        lastCheckPoint = lapManager.checkPoints[6];
                        nextCheckPoint = lapManager.checkPoints[7];
                        checkPointCounter += 1;
                    }
                }
            }

            if (coll.gameObject.name == "CheckPoint8")
            {
                if (check1 && check2 && check3 && check4 && check5 && check6 &&
                   check7 && !check8 && !check9 && !check10 && !check11 && !check12)
                {
                    if (!check8)
                    {
                        check8 = true;
                        lastCheckPoint = lapManager.checkPoints[7];
                        nextCheckPoint = lapManager.checkPoints[8];
                        checkPointCounter += 1;
                    }
                }
            }

            if (coll.gameObject.name == "CheckPoint9")
            {
                if (check1 && check2 && check3 && check4 && check5 && check6 &&
                   check7 && check8 && !check9 && !check10 && !check11 && !check12)
                {
                    if (!check9)
                    {
                        check9 = true;
                        lastCheckPoint = lapManager.checkPoints[8];
                        nextCheckPoint = lapManager.checkPoints[9];
                        checkPointCounter += 1;
                    }
                }
            }

            if (coll.gameObject.name == "CheckPoint10")
            {
                if (check1 && check2 && check3 && check4 && check5 && check6 &&
                   check7 && check8 && check9 && !check10 && !check11 && !check12)
                {
                    if (!check10)
                    {
                        check10 = true;
                        lastCheckPoint = lapManager.checkPoints[9];
                        nextCheckPoint = lapManager.checkPoints[10];
                        checkPointCounter += 1;
                    }

                }
            }

            if (coll.gameObject.name == "CheckPoint11")
            {
                if (check1 && check2 && check3 && check4 && check5 && check6 &&
                   check7 && check8 && check9 && check10 && !check11 && !check12)
                {
                    if (!check11)
                    {
                        check11 = true;
                        lastCheckPoint = lapManager.checkPoints[10];
                        nextCheckPoint = lapManager.checkPoints[11];
                        checkPointCounter += 1;
                    }
                }
            }

            if (coll.gameObject.name == "CheckPoint12")
            {
                if (check1 && check2 && check3 && check4 && check5 && check6 &&
                   check7 && check8 && check9 && check10 && check11 && !check12)
                {
                    if (!check12)
                    {
                        check12 = true;
                        lastCheckPoint = lapManager.checkPoints[11];
                        nextCheckPoint = lapManager.FinishLine;
                        checkPointCounter += 1;
                    }
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

                if (lapNumber == 4)
                {
                    finalPosition = kartPosition;
                }

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
           //     checkPointCounter = 0;
            }
            //if (check1 && check2 && check3 && check4 && (lapNumber >= 3))
            //{
            //    finalPosition = kartPosition;
            //}

        }

        //If item rpg is hit destroy the powerup and if they do not have a mine or boost
        //set itemRPG to true.

        
    }

    void OnCollisionEnter(Collision coll)
    {
        //If an rpg hits them and they are not immune to damage
        //Destroy the rpg and disable the player.
        if (coll.gameObject.tag == "RPG")
        {
            GameObject.Destroy(coll.gameObject.transform.parent.gameObject);
            if (!immuneToDamage)
            {
                
                playerDisabled = true;
            }
        }

        if (coll.gameObject.tag == "Respawn")
        {
            this.transform.forward = lastCheckPoint.transform.forward;
            this.transform.position = lastCheckPoint.transform.position;
            playerDisabled = true;

        }

    }


    void OnCollisionStay(Collision coll)
    {
        if (coll.gameObject.tag == "Respawn")
        {
            this.transform.forward = lastCheckPoint.transform.forward;
            this.transform.position = lastCheckPoint.transform.position;
            playerDisabled = true;

        }
    }
}
