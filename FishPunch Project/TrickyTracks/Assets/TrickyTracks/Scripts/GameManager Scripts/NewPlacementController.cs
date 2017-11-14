using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NewPlacementController : MonoBehaviour
{
    private GameObject[] items;
    private GameObject[] mines;
    private GameObject[] boosts;
    private GameObject[] slicks;

    public Color disabledColor;

    public AudioSource scrollItems1, scrollItems2, placeItem;

    public float exclusionDistance = 10.0f;

    //Manager variables to access other scripts on the manager.
    private GameObject manager;
    private GamePadManager gpManager;
    private PlayerSelectActor psActor;

    private LapsManager lapManager;

    //Copy of item lists.
    private GameObject itemListcopy1, itemListCopy2, itemListCopy3, itemListCopy4;

    public float prefabRotationSpeed = 2.0f;

    //Player Gamepad Instances
    private xbox_gamepad gamepad1, gamepad2, gamepad3, gamepad4;

    //Images for item UI that displays current item, next item and previous item.
    private Image currentItem1, currentItemBack1;
    private Image currentItem2, currentItemBack2;
    private Image currentItem3, currentItemBack3;
    private Image currentItem4, currentItemBack4;
    private Image powerup1, powerup2, powerup3, powerup4, powerup1_2P, powerup2_2P;
    private Image powerupIcon1, powerupIcon2, powerupIcon3, powerupIcon4, powerupIcon1_2P, powerupIcon2_2P;

    private Image itemCount1Num1, itemCount1Num2, itemCount2Num1, itemCount2Num2, itemCount3Num1, itemCount3Num2,
                  itemCount4Num1, itemCount4Num2;

    public Sprite[] nums;
    
    //Traps gameobject prefabs
    [Header("Traps")]
    public GameObject buzzsaw;
    public GameObject ramp;
    public GameObject oilslick;
    public GameObject boostsaw;

    //Item prefabs
    [Header("Items")]
    public GameObject boost;
    public GameObject rpg;
    public GameObject mine;

    //Sprites for all the items.
    [Header("Item Icons")]
    public Sprite buzzsawIcon;
    public Sprite rampIcon;
    public Sprite oilslickIcon;
    public Sprite boostIcon;
    public Sprite mineIcon;
    public Sprite rpgIcon;
    public Sprite blankIcon;
    public Sprite blankPowerupIcon;

    [Header("Player Icons")]
    public Sprite playerSprite1;
    public Sprite playerSprite2;
    public Sprite playerSprite3;
    public Sprite playerSprite4;

    public Material redItem, transparentItem;

    //Object that the raycast shoots down from.
    private GameObject raycastObject1, raycastObject2, raycastObject3, raycastObject4;
    
    //The object currently about to be placed
    private GameObject currentPlaceableObject1, currentPlaceableObject2,
                       currentPlaceableObject3, currentPlaceableObject4;

    //The object that is placed when the prefab is released with releaseprefab().
    private GameObject placeableObject1, placeableObject2,
                       placeableObject3, placeableObject4;

    //Index for each player item list.
    private int prefabIndex1 = 0, prefabIndex2 = 0, prefabIndex3 = 0, prefabIndex4 = 0;

    private bool cannotPlace1 = false, cannotPlace2 = false, cannotPlace3 = false, cannotPlace4 = false;

    //kart gameobjects.
    private GameObject kart1, kart2, kart3, kart4;

    //List with all trap and all item prefabs respectively.
    private List<GameObject> trapPrefabs = new List<GameObject>();
    private List<GameObject> itemPrefabs = new List<GameObject>();

    //list that stores random numbers for each player so they receive different items every lap.
    private List<int> randNumP1 = new List<int>(), randNumP2 = new List<int>(),
                      randNumP3 = new List<int>(), randNumP4 = new List<int>();

    //The list where player items are stored.
    [HideInInspector]
    public List<GameObject> itemsP1 = new List<GameObject>(), itemsP2 = new List<GameObject>(),
                             itemsP3 = new List<GameObject>(), itemsP4 = new List<GameObject>();

    //temporary number that random function assigns a number to before pushing to a random number list.
    private int randTempNum1;

    //Layer mask so the raycast object ignores the trap/item its placing.
    int layerMask;




    // Use this for initialization
    void Start()
    {
        //Grab manager instances from manager gameobject.
        manager = this.gameObject;
        gpManager = GetComponent<GamePadManager>();
        psActor = manager.GetComponent<PlayerSelectActor>();
        lapManager = manager.GetComponent<LapsManager>();
        

        //add traps to trap list.
        trapPrefabs.Add(buzzsaw);
        trapPrefabs.Add(oilslick);
        trapPrefabs.Add(ramp);

        //add items to item list
        itemPrefabs.Add(boost);
        itemPrefabs.Add(rpg);
        itemPrefabs.Add(mine);

        //I know how bad gameobject.Find is dont worry just used it as i had to write 99%
        //of the game myself so had no time to plan out a proper system :)


        itemCount1Num1 = GameObject.Find("ItemCount1Num1").GetComponent<Image>();
        itemCount1Num2 = GameObject.Find("ItemCount1Num2").GetComponent<Image>();
        itemCount2Num1 = GameObject.Find("ItemCount2Num1").GetComponent<Image>();
        itemCount2Num2 = GameObject.Find("ItemCount2Num2").GetComponent<Image>();
        itemCount3Num1 = GameObject.Find("ItemCount3Num1").GetComponent<Image>();
        itemCount3Num2 = GameObject.Find("ItemCount3Num2").GetComponent<Image>();
        itemCount4Num1 = GameObject.Find("ItemCount4Num1").GetComponent<Image>();
        itemCount4Num2 = GameObject.Find("ItemCount4Num2").GetComponent<Image>();

        
        currentItemBack1 = GameObject.Find("CurrentItemBack1").GetComponent<Image>();
        currentItemBack2 = GameObject.Find("CurrentItemBack2").GetComponent<Image>();
        currentItemBack3 = GameObject.Find("CurrentItemBack3").GetComponent<Image>();
        currentItemBack4 = GameObject.Find("CurrentItemBack4").GetComponent<Image>();

        currentItem1 = GameObject.Find("CurrentItem1").GetComponent<Image>();
        currentItem2 = GameObject.Find("CurrentItem2").GetComponent<Image>();
        currentItem3 = GameObject.Find("CurrentItem3").GetComponent<Image>();
        currentItem4 = GameObject.Find("CurrentItem4").GetComponent<Image>();

        powerup1 = GameObject.Find("Powerup1").GetComponent<Image>();
        powerup2 = GameObject.Find("Powerup2").GetComponent<Image>();
        powerup3 = GameObject.Find("Powerup3").GetComponent<Image>();
        powerup4 = GameObject.Find("Powerup4").GetComponent<Image>();

        powerup1_2P = GameObject.Find("Powerup1_2P").GetComponent<Image>();
        powerup2_2P = GameObject.Find("Powerup2_2P").GetComponent<Image>();

        powerupIcon1 = GameObject.Find("PowerupIcon1").GetComponent<Image>();
        powerupIcon2 = GameObject.Find("PowerupIcon2").GetComponent<Image>();
        powerupIcon3 = GameObject.Find("PowerupIcon3").GetComponent<Image>();
        powerupIcon4 = GameObject.Find("PowerupIcon4").GetComponent<Image>();

        powerupIcon1_2P = GameObject.Find("PowerupIcon1_2P").GetComponent<Image>();
        powerupIcon2_2P = GameObject.Find("PowerupIcon2_2P").GetComponent<Image>();

        //depending on playercount.
        switch (psActor.playerCount)
        {
            case 1:
                //Randomise items and assign gamepad.
                randomiseItems(randTempNum1, randNumP1);
                allocateRandItems(randNumP1, itemPrefabs, trapPrefabs, itemsP1);
                placeableObject1 = itemsP1[0];
                gamepad1 = GamePadManager.Instance.GetGamePad(1);

                //Find raycast object and kart
                raycastObject1 = GameObject.Find("RayCast1");
                kart1 = GameObject.Find("PlayerCharacter_001");

                //All UI except first player disabled.
                currentItem2.enabled = false;
                currentItem3.enabled = false;
                currentItem4.enabled = false;

                break;
            case 2:
                //Randomise items.
                randomiseItems(randTempNum1, randNumP1);
                allocateRandItems(randNumP1, itemPrefabs, trapPrefabs, itemsP1);
                randomiseItems(randTempNum1, randNumP2);
                allocateRandItems(randNumP2, itemPrefabs, trapPrefabs, itemsP2);
                placeableObject1 = itemsP1[0];
                placeableObject2 = itemsP2[0];

                //Assign gamepads.
                gamepad1 = GamePadManager.Instance.GetGamePad(1);
                gamepad2 = GamePadManager.Instance.GetGamePad(2);

                //Assign raycast object and karts
                raycastObject1 = GameObject.Find("RayCast1");
                raycastObject2 = GameObject.Find("RayCast2");
                kart1 = GameObject.Find("PlayerCharacter_001");
                kart2 = GameObject.Find("PlayerCharacter_002");

                //disabled UI sprites.
                currentItem2.enabled = false;
                currentItem4.enabled = false;
                currentItemBack2.enabled = false;
                currentItemBack4.enabled = false;
                currentItemBack3.sprite = playerSprite2;

                powerup1.enabled = false;
                powerup2.enabled = false;
                powerup3.enabled = false;
                powerup4.enabled = false;
                powerupIcon1.enabled = false;
                powerupIcon2.enabled = false;
                powerupIcon3.enabled = false;
                powerupIcon4.enabled = false;

                GameObject.Find("ItemCount3").SetActive(false);
                itemCount3Num1.enabled = false;
                itemCount3Num2.enabled = false;

                GameObject.Find("ItemCount4").SetActive(false);
                itemCount4Num1.enabled = false;
                itemCount4Num2.enabled = false;

                break;
            case 3:

                powerup1_2P.enabled = false;
                powerup2_2P.enabled = false;
                powerupIcon1_2P.enabled = false;
                powerupIcon2_2P.enabled = false;
                powerup4.enabled = false;
                powerupIcon4.enabled = false;
                //Randomise items.
                randomiseItems(randTempNum1, randNumP1);
                allocateRandItems(randNumP1, itemPrefabs, trapPrefabs, itemsP1);
                randomiseItems(randTempNum1, randNumP2);
                allocateRandItems(randNumP2, itemPrefabs, trapPrefabs, itemsP2);
                randomiseItems(randTempNum1, randNumP3);
                allocateRandItems(randNumP3, itemPrefabs, trapPrefabs, itemsP3);
                placeableObject1 = itemsP1[0];
                placeableObject2 = itemsP2[0];
                placeableObject3 = itemsP3[0];

                //Assign gamepads.
                gamepad1 = GamePadManager.Instance.GetGamePad(1);
                gamepad2 = GamePadManager.Instance.GetGamePad(2);
                gamepad3 = GamePadManager.Instance.GetGamePad(3);
                raycastObject1 = GameObject.Find("RayCast1");
                raycastObject2 = GameObject.Find("RayCast2");
                raycastObject3 = GameObject.Find("RayCast3");

                kart1 = GameObject.Find("PlayerCharacter_001");
                kart2 = GameObject.Find("PlayerCharacter_002");
                kart3 = GameObject.Find("PlayerCharacter_003");

                currentItemBack4.enabled = false;
                currentItem4.enabled = false;

                GameObject.Find("ItemCount4").SetActive(false);
                itemCount4Num1.enabled = false;
                itemCount4Num2.enabled = false;

              
                break;
            case 4:

                powerup1_2P.enabled = false;
                powerup2_2P.enabled = false;
                powerupIcon1_2P.enabled = false;
                powerupIcon2_2P.enabled = false;

                randomiseItems(randTempNum1, randNumP1);
                allocateRandItems(randNumP1, itemPrefabs, trapPrefabs, itemsP1);
                randomiseItems(randTempNum1, randNumP2);
                allocateRandItems(randNumP2, itemPrefabs, trapPrefabs, itemsP2);
                randomiseItems(randTempNum1, randNumP3);
                allocateRandItems(randNumP3, itemPrefabs, trapPrefabs, itemsP3);
                randomiseItems(randTempNum1, randNumP4);
                allocateRandItems(randNumP4, itemPrefabs, trapPrefabs, itemsP4);

                placeableObject1 = itemsP1[0];
                placeableObject2 = itemsP2[0];
                placeableObject3 = itemsP3[0];
                placeableObject4 = itemsP4[0];

                gamepad1 = GamePadManager.Instance.GetGamePad(1);
                gamepad2 = GamePadManager.Instance.GetGamePad(2);
                gamepad3 = GamePadManager.Instance.GetGamePad(3);
                gamepad4 = GamePadManager.Instance.GetGamePad(4);

                raycastObject1 = GameObject.Find("RayCast1");
                raycastObject2 = GameObject.Find("RayCast2");
                raycastObject3 = GameObject.Find("RayCast3");
                raycastObject4 = GameObject.Find("RayCast4");

                kart1 = GameObject.Find("PlayerCharacter_001");
                kart2 = GameObject.Find("PlayerCharacter_002");
                kart3 = GameObject.Find("PlayerCharacter_003");
                kart4 = GameObject.Find("PlayerCharacter_004");

                powerup1_2P.enabled = false;
                powerup2_2P.enabled = false;

                break;
            default:
                break;
        }


        layerMask = 1 << LayerMask.NameToLayer("Item");
        layerMask = ~layerMask;
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log("items count: " + itemsP1.Count);
        Debug.Log("index: " + prefabIndex1);
        items = GameObject.FindGameObjectsWithTag("Item");
        //    boosts = GameObject.FindGameObjectsWithTag("Boost");
        //      slicks = GameObject.FindGameObjectsWithTag("OilSlick");
        //      mines = GameObject.FindGameObjectsWithTag("ItemMine");

        foreach (GameObject obj in items)
        {
            if (currentPlaceableObject1 != null)
            { 
                if ((Vector3.Distance(obj.transform.position, currentPlaceableObject1.transform.position) < exclusionDistance))
                {
                    Renderer rend;
                    rend = currentPlaceableObject1.GetComponentInChildren<Renderer>();
                    rend.material.color = Color.red;

                    cannotPlace1 = true;
                }
                else
                {
                    cannotPlace1 = false;
                    Renderer rend;
                    rend = currentPlaceableObject1.GetComponentInChildren<Renderer>();
                    rend.material.color = Color.white;
                    rend.material.color = new Color(0,0,0,0);
                    
                }
            }

            if (currentPlaceableObject2 != null)
            {
                if ((Vector3.Distance(obj.transform.position, currentPlaceableObject2.transform.position) < exclusionDistance))
                {
                    Renderer rend;
                    rend = currentPlaceableObject2.GetComponentInChildren<Renderer>();
                    rend.material.color = Color.red;
                    cannotPlace2 = true;
                }
                else
                {
                    cannotPlace2 = false;
                    Renderer rend;
                    rend = currentPlaceableObject2.GetComponentInChildren<Renderer>();
                    rend.material.color = Color.white;
                    rend.material.color = new Color(0, 0, 0, 0);
                }
            }

            if (currentPlaceableObject3 != null)
            {
                if ((Vector3.Distance(obj.transform.position, currentPlaceableObject3.transform.position) < exclusionDistance))
                {
                    Renderer rend;
                    rend = currentPlaceableObject3.GetComponentInChildren<Renderer>();
                    rend.material.color = Color.red;
                    cannotPlace3 = true;
                }
                else
                {
                    cannotPlace3 = false;
                    Renderer rend;
                    rend = currentPlaceableObject3.GetComponentInChildren<Renderer>();
                    rend.material.color = Color.white;
                    rend.material.color = new Color(0, 0, 0, 0);
                }
            }

            if (currentPlaceableObject4 != null)
            {
                if ((Vector3.Distance(obj.transform.position, currentPlaceableObject4.transform.position) < exclusionDistance))
                {
                    Renderer rend;
                    rend = currentPlaceableObject4.GetComponentInChildren<Renderer>();
                    rend.material.color = Color.red;
                    cannotPlace4 = true;
                }
                else
                {
                    cannotPlace4 = false;
                    Renderer rend;
                    rend = currentPlaceableObject4.GetComponentInChildren<Renderer>();
                    rend.material.color = Color.white;
                    rend.material.color = new Color(0, 0, 0, 0);
                }
            }
        }
        if (lapManager.raceCountdownTimer < 0)
        {
            objectGeneration();
            releasePrefab();
         //   rotatePrefab();
            changePrefab();
        }

        switch (psActor.playerCount)
        {
            case 1:

                if (kart1.GetComponent<PlayerActor>().assignNewTraps)
                {
                    randNumP1.Clear();
                    randomiseItems(randTempNum1, randNumP1);
                    allocateRandItems(randNumP1, itemPrefabs, trapPrefabs, itemsP1);
                    kart1.GetComponent<PlayerActor>().assignNewTraps = false;
                    placeableObject1 = itemsP1[0];
                }

                if (currentPlaceableObject1 != null)
                {
                    fitPrefabToTrack(raycastObject1, currentPlaceableObject1, gamepad1);
                }

                switchItemIcons(prefabIndex1, currentItem1, itemsP1);

                break;
            case 2:
         
                switchItemIcons(prefabIndex1, currentItem1, itemsP1);
                switchItemIcons(prefabIndex2, currentItem3, itemsP2);

                if (kart1.GetComponent<PlayerActor>().assignNewTraps)
                {
                    randNumP1.Clear();
                    randomiseItems(randTempNum1, randNumP1);
                    allocateRandItems(randNumP1, itemPrefabs, trapPrefabs, itemsP1);
                    kart1.GetComponent<PlayerActor>().assignNewTraps = false;
                    placeableObject1 = itemsP1[0];
                }
                if (kart2.GetComponent<PlayerActor>().assignNewTraps)
                {
                    randNumP2.Clear();
                    randomiseItems(randTempNum1, randNumP2);
                    allocateRandItems(randNumP2, itemPrefabs, trapPrefabs, itemsP2);
                    kart2.GetComponent<PlayerActor>().assignNewTraps = false;
                    placeableObject2 = itemsP2[0];
                }


                       switchPowerupIcons(powerupIcon1_2P, kart1);
                switchPowerupIcons(powerupIcon2_2P, kart2);

                if (currentPlaceableObject1 != null)
                {
                    fitPrefabToTrack(raycastObject1, currentPlaceableObject1, gamepad1);
                }
                if (currentPlaceableObject2 != null)
                {
                    fitPrefabToTrack(raycastObject2, currentPlaceableObject2, gamepad2);
                }

                playerItemCountUi(itemsP1, itemCount1Num1, itemCount1Num2);
                playerItemCountUi(itemsP2, itemCount2Num1, itemCount2Num2);


                break;
            case 3:

                powerup1_2P.enabled = false;
                powerup2_2P.enabled = false;
                powerup4.enabled = false;

                switchItemIcons(prefabIndex1, currentItem1, itemsP1);
                switchItemIcons(prefabIndex2, currentItem2, itemsP2);
                switchItemIcons(prefabIndex3, currentItem3, itemsP3);

                switchPowerupIcons(powerupIcon1, kart1);
                switchPowerupIcons(powerupIcon2, kart2);
                switchPowerupIcons(powerupIcon3, kart3);

                if (kart1.GetComponent<PlayerActor>().assignNewTraps)
                {
                    randNumP1.Clear();
                    randomiseItems(randTempNum1, randNumP1);
                    allocateRandItems(randNumP1, itemPrefabs, trapPrefabs, itemsP1);
                    kart1.GetComponent<PlayerActor>().assignNewTraps = false;
                    placeableObject1 = itemsP1[0];
                }
                if (kart2.GetComponent<PlayerActor>().assignNewTraps)
                {
                    randNumP2.Clear();
                    randomiseItems(randTempNum1, randNumP2);
                    allocateRandItems(randNumP2, itemPrefabs, trapPrefabs, itemsP2);
                    kart2.GetComponent<PlayerActor>().assignNewTraps = false;
                    placeableObject2 = itemsP2[0];
                }
                if (kart3.GetComponent<PlayerActor>().assignNewTraps)
                {
                    randNumP3.Clear();
                    randomiseItems(randTempNum1, randNumP3);
                    allocateRandItems(randNumP3, itemPrefabs, trapPrefabs, itemsP3);
                    kart3.GetComponent<PlayerActor>().assignNewTraps = false;
                    placeableObject3 = itemsP3[0];
                }
                if (currentPlaceableObject1 != null)
                {
                    fitPrefabToTrack(raycastObject1, currentPlaceableObject1, gamepad1);
                }
                if (currentPlaceableObject2 != null)
                {
                    fitPrefabToTrack(raycastObject2, currentPlaceableObject2, gamepad2);
                }
                if (currentPlaceableObject3 != null)
                {
                    fitPrefabToTrack(raycastObject3, currentPlaceableObject3, gamepad3);
                }

                playerItemCountUi(itemsP1, itemCount1Num1, itemCount1Num2);
                playerItemCountUi(itemsP2, itemCount3Num1, itemCount3Num2);
                playerItemCountUi(itemsP3, itemCount2Num1, itemCount2Num2);

                break;
            case 4:
                if (kart1.GetComponent<PlayerActor>().assignNewTraps)
                {
                    randNumP1.Clear();
                    randomiseItems(randTempNum1, randNumP1);
                    allocateRandItems(randNumP1, itemPrefabs, trapPrefabs, itemsP1);
                    kart1.GetComponent<PlayerActor>().assignNewTraps = false;
                }
                if (kart2.GetComponent<PlayerActor>().assignNewTraps)
                {
                    randNumP2.Clear();
                    randomiseItems(randTempNum1, randNumP2);
                    allocateRandItems(randNumP2, itemPrefabs, trapPrefabs, itemsP2);
                    kart2.GetComponent<PlayerActor>().assignNewTraps = false;
                }
                if (kart3.GetComponent<PlayerActor>().assignNewTraps)
                {
                    randNumP3.Clear();
                    randomiseItems(randTempNum1, randNumP3);
                    allocateRandItems(randNumP3, itemPrefabs, trapPrefabs, itemsP3);
                    kart3.GetComponent<PlayerActor>().assignNewTraps = false;
                }
                if (kart4.GetComponent<PlayerActor>().assignNewTraps)
                {
                    randNumP4.Clear();
                    randomiseItems(randTempNum1, randNumP4);
                    allocateRandItems(randNumP4, itemPrefabs, trapPrefabs, itemsP4);
                    kart4.GetComponent<PlayerActor>().assignNewTraps = false;
                }
                if (currentPlaceableObject1 != null)
                {
                    fitPrefabToTrack(raycastObject1, currentPlaceableObject1, gamepad1);
                }
                if (currentPlaceableObject2 != null)
                {
                    fitPrefabToTrack(raycastObject2, currentPlaceableObject2, gamepad2);
                }
                if (currentPlaceableObject3 != null)
                {
                    fitPrefabToTrack(raycastObject3, currentPlaceableObject3, gamepad3);
                }
                if (currentPlaceableObject4 != null)
                {
                    fitPrefabToTrack(raycastObject4, currentPlaceableObject4, gamepad4);
                }
                placeableObject1 = itemsP1[0];
                placeableObject2 = itemsP2[0];
                placeableObject3 = itemsP3[0];
                placeableObject4 = itemsP4[0];


                switchItemIcons(prefabIndex1, currentItem1, itemsP1);
                switchItemIcons(prefabIndex2, currentItem2, itemsP2);
                switchItemIcons(prefabIndex3, currentItem3, itemsP3);
                switchItemIcons(prefabIndex4, currentItem4, itemsP4);


                switchPowerupIcons(powerupIcon1, kart1);
                switchPowerupIcons(powerupIcon2, kart2);
                switchPowerupIcons(powerupIcon3, kart3);
                switchPowerupIcons(powerupIcon4, kart4);


                playerItemCountUi(itemsP1, itemCount1Num1, itemCount1Num2);
                playerItemCountUi(itemsP2, itemCount3Num1, itemCount3Num2);
                playerItemCountUi(itemsP3, itemCount2Num1, itemCount2Num2);
                playerItemCountUi(itemsP4, itemCount4Num1, itemCount4Num2);

                break;
            default:
                break;
        }

    }

    //Generates new object when players A button is down and item count is above 0.
    void objectGeneration()
    {
        switch (psActor.playerCount)
        {
            case 1:
                if (gamepad1.GetButtonDown("A"))
                {
                    if (currentPlaceableObject1 == null)
                    {
                        if (itemsP1.Count > 0)
                        {
                            currentPlaceableObject1 = Instantiate(placeableObject1);
                            currentPlaceableObject1.tag = "PlaceableObject";
                        }
                    }
                }
                break;
            case 2:
                if (gamepad1.GetButtonDown("A"))
                {
                    if (currentPlaceableObject1 == null)
                    {
                        if (itemsP1.Count > 0)
                        {
                            currentPlaceableObject1 = Instantiate(placeableObject1);
                            currentPlaceableObject1.tag = "PlaceableObject";
                        }
                    }
                }
                if (gamepad2.GetButtonDown("A"))
                {
                    if (currentPlaceableObject2 == null)
                    {
                        if (itemsP2.Count > 0)
                        {
                            currentPlaceableObject2 = Instantiate(placeableObject2);
                            currentPlaceableObject2.tag = "PlaceableObject";
                        }
                    }
                }
                break;
            case 3:
                if (gamepad1.GetButtonDown("A"))
                {
                    if (currentPlaceableObject1 == null)
                    {
                        if (itemsP1.Count > 0)
                        {
                            currentPlaceableObject1 = Instantiate(placeableObject1);
                            currentPlaceableObject1.tag = "PlaceableObject";
                        }
                    }
                }
                if (gamepad2.GetButtonDown("A"))
                {
                    if (currentPlaceableObject2 == null)
                    {
                        if (itemsP2.Count > 0)
                        {
                            currentPlaceableObject2 = Instantiate(placeableObject2);
                            currentPlaceableObject2.tag = "PlaceableObject";
                        }
                    }
                }
                if (gamepad3.GetButtonDown("A"))
                {
                    if (currentPlaceableObject3 == null)
                    {
                        if (itemsP3.Count > 0)
                        {
                            currentPlaceableObject3 = Instantiate(placeableObject3);
                            currentPlaceableObject3.tag = "PlaceableObject";
                        }
                    }
                }
                break;
            case 4:
                if (gamepad1.GetButtonDown("A"))
                {
                    if (currentPlaceableObject1 == null)
                    {
                        if (itemsP1.Count > 0)
                        {
                            currentPlaceableObject1 = Instantiate(placeableObject1);
                            currentPlaceableObject1.tag = "PlaceableObject";
                        }
                    }
                }
                if (gamepad2.GetButtonDown("A"))
                {
                    if (currentPlaceableObject2 == null)
                    {
                        if (itemsP2.Count > 0)
                        {
                            currentPlaceableObject2 = Instantiate(placeableObject2);
                            currentPlaceableObject2.tag = "PlaceableObject";
                        }
                    }
                }
                if (gamepad3.GetButtonDown("A"))
                {
                    if (currentPlaceableObject3 == null)
                    {
                        if (itemsP3.Count > 0)
                        {
                            currentPlaceableObject3 = Instantiate(placeableObject3);
                            currentPlaceableObject3.tag = "PlaceableObject";
                        }
                    }
                }
                if (gamepad4.GetButtonDown("A"))
                {
                    if (currentPlaceableObject4 == null)
                    {
                        if (itemsP4.Count > 0)
                        {
                            currentPlaceableObject4 = Instantiate(placeableObject4);
                            currentPlaceableObject4.tag = "PlaceableObject";
                        }
                    }
                }
                break;
            default:
                break;
        }

    }

    //Raycast shoots down from raycast object and rotates item to fit track based on the hitinfo from raycast.
    void fitPrefabToTrack(GameObject raycastObject, GameObject currentPlaceableObject, xbox_gamepad gamepad)
    {

        RaycastHit hitInfo;
        if (Physics.Raycast(raycastObject.transform.position, -raycastObject.transform.up, out hitInfo, 1000, layerMask))
        {
            currentPlaceableObject.transform.position = hitInfo.point;

            currentPlaceableObject.transform.rotation = Quaternion.FromToRotation(Vector3.up, new Vector3(hitInfo.normal.x, hitInfo.normal.y, hitInfo.normal.z));
            currentPlaceableObject.transform.forward = raycastObject.transform.forward;
            currentPlaceableObject.transform.Rotate(0, gamepad.triggerRotation, 0);

        }
    }

    //Rotates prefab when a is held and they press RB or LB
    void rotatePrefab()
    {
        if (psActor.playerCount == 1)
        {
            if (gamepad1.GetButton("A"))
            {
                if (gamepad1.GetButton("RB"))
                {
                    gamepad1.triggerRotation += prefabRotationSpeed;
                }
                if (gamepad1.GetButton("LB"))
                {
                    gamepad1.triggerRotation -= prefabRotationSpeed;
                }
            }
        }

        if (psActor.playerCount == 2)
        {
            if (gamepad1.GetButton("A"))
            {
                if (gamepad1.GetButton("RB"))
                {
                    gamepad1.triggerRotation += prefabRotationSpeed;
                }
                if (gamepad1.GetButton("LB"))
                {
                    gamepad1.triggerRotation -= prefabRotationSpeed;
                }
            }
            if (gamepad2.GetButton("A"))
            {
                if (gamepad2.GetButton("RB"))
                {
                    gamepad2.triggerRotation += prefabRotationSpeed;
                }
                if (gamepad2.GetButton("LB"))
                {
                    gamepad2.triggerRotation -= prefabRotationSpeed;
                }
            }
        }

        if (psActor.playerCount == 3)
        {
            if (gamepad1.GetButton("A"))
            {
                if (gamepad1.GetButton("RB"))
                {
                    gamepad1.triggerRotation += prefabRotationSpeed;
                }
                if (gamepad1.GetButton("LB"))
                {
                    gamepad1.triggerRotation -= prefabRotationSpeed;
                }
            }
            if (gamepad2.GetButton("A"))
            {
                if (gamepad2.GetButton("RB"))
                {
                    gamepad2.triggerRotation += prefabRotationSpeed;
                }
                if (gamepad2.GetButton("LB"))
                {
                    gamepad2.triggerRotation -= prefabRotationSpeed;
                }
            }
            if (gamepad3.GetButton("A"))
            {
                if (gamepad3.GetButton("RB"))
                {
                    gamepad3.triggerRotation += prefabRotationSpeed;
                }
                if (gamepad3.GetButton("LB"))
                {
                    gamepad3.triggerRotation -= prefabRotationSpeed;
                }
            }

        }

        if (psActor.playerCount == 4)
        {
            if (gamepad1.GetButton("A"))
            {
                if (gamepad1.GetButton("RB"))
                {
                    gamepad1.triggerRotation += prefabRotationSpeed;
                }
                if (gamepad1.GetButton("LB"))
                {
                    gamepad1.triggerRotation -= prefabRotationSpeed;
                }
            }
            if (gamepad2.GetButton("A"))
            {
                if (gamepad2.GetButton("RB"))
                {
                    gamepad2.triggerRotation += prefabRotationSpeed;
                }
                if (gamepad2.GetButton("LB"))
                {
                    gamepad2.triggerRotation -= prefabRotationSpeed;
                }
            }
            if (gamepad3.GetButton("A"))
            {
                if (gamepad3.GetButton("RB"))
                {
                    gamepad3.triggerRotation += prefabRotationSpeed;
                }
                if (gamepad3.GetButton("LB"))
                {
                    gamepad3.triggerRotation -= prefabRotationSpeed;
                }
            }
            if (gamepad4.GetButton("A"))
            {
                if (gamepad4.GetButton("RB"))
                {
                    gamepad4.triggerRotation += prefabRotationSpeed;
                }
                if (gamepad4.GetButton("LB"))
                {
                    gamepad4.triggerRotation -= prefabRotationSpeed;
                }
            }

        }

    }

    //When A is not pressed down and there count is above 0
    //rb or lb switches between item prefabs
    void changePrefab()
    {
        switch (psActor.playerCount)
        {
            case 1:
                //if count above 0
                if (itemsP1.Count > 0)
                {
                    if (!gamepad1.GetButton("A"))
                    {
                        if (gamepad1.GetButtonDown("RB"))
                        {
                            //destroy object instance and add +1 to index
                            Destroy(currentPlaceableObject1);
                            if (prefabIndex1 < itemsP1.Count - 1)
                            {
                                prefabIndex1++;
                            }
                            else
                            {
                                //= 0 if index reaches end of the list.
                                prefabIndex1 = 0;
                            }
                            //Reassign object based on index
                            placeableObject1 = itemsP1[prefabIndex1];
                        }
                        if (gamepad1.GetButtonDown("LB"))
                        {
                            //destroy object instance
                            Destroy(currentPlaceableObject1);
                            //If index is above 0 -1 from index
                            if (prefabIndex1 > 0)
                            {
                                prefabIndex1--;
                            }
                            else
                            {
                                //if item count = 0
                                if(itemsP1.Count == 0)
                                {
                                    prefabIndex1 = itemsP1.Count;
                                }
                                //count is greater than 0
                                if (itemsP1.Count > 0)
                                {
                                    //Index = count - 1
                                    prefabIndex1 = itemsP1.Count - 1;
                                }

                            }
                            //reassign object based on index.
                            placeableObject1 = itemsP1[prefabIndex1];
                        }
                        
                    }
                }
                break;
            case 2:
                if (itemsP1.Count > 0)
                {
                    if (!gamepad1.GetButton("A"))
                    {
                        if (gamepad1.GetButtonDown("RB"))
                        {
                            scrollItems1.Play();
                            Destroy(currentPlaceableObject1);
                            if (prefabIndex1 < itemsP1.Count - 1)
                            {
                                prefabIndex1++;
                            }
                            else
                            {
                                prefabIndex1 = 0;
                            }
                            placeableObject1 = itemsP1[prefabIndex1];
                        }
                        if (gamepad1.GetButtonDown("LB"))
                        {
                            scrollItems2.Play();
                            Destroy(currentPlaceableObject1);
                            if (prefabIndex1 > 0)
                            {
                                prefabIndex1--;
                            }
                            else
                            {
                                if (itemsP1.Count == 0)
                                {
                                    prefabIndex1 = itemsP1.Count;
                                }
                                if (itemsP1.Count > 0)
                                {
                                    prefabIndex1 = itemsP1.Count - 1;
                                }

                            }
                            placeableObject1 = itemsP1[prefabIndex1];
                        }

                    }
                }
                if (itemsP2.Count > 0)
                {
                    if (!gamepad2.GetButton("A"))
                    {
                        if (gamepad2.GetButtonDown("RB"))
                        {
                            scrollItems1.Play();
                            Destroy(currentPlaceableObject2);
                            if (prefabIndex2 < itemsP2.Count - 1)
                            {
                                prefabIndex2++;
                            }
                            else
                            {
                                prefabIndex2 = 0;
                            }
                            placeableObject2 = itemsP2[prefabIndex2];
                        }
                        if (gamepad2.GetButtonDown("LB"))
                        {
                            scrollItems2.Play();
                            Destroy(currentPlaceableObject2);
                            if (prefabIndex2 > 0)
                            {
                                prefabIndex2--;
                            }
                            else
                            {
                                if (itemsP2.Count == 0)
                                {
                                    prefabIndex2 = itemsP2.Count;
                                }
                                if (itemsP2.Count > 0)
                                {
                                    prefabIndex2 = itemsP2.Count - 1;
                                }

                            }
                            placeableObject2 = itemsP2[prefabIndex2];
                        }

                    }
                }
                break;
            case 3:
                if (itemsP1.Count > 0)
                {
                    if (!gamepad1.GetButton("A"))
                    {
                        if (gamepad1.GetButtonDown("RB"))
                        {
                            Destroy(currentPlaceableObject1);
                            if (prefabIndex1 < itemsP1.Count - 1)
                            {
                                prefabIndex1++;
                            }
                            else
                            {
                                prefabIndex1 = 0;
                            }
                            placeableObject1 = itemsP1[prefabIndex1];
                        }
                        if (gamepad1.GetButtonDown("LB"))
                        {
                            Destroy(currentPlaceableObject1);
                            if (prefabIndex1 > 0)
                            {
                                prefabIndex1--;
                            }
                            else
                            {
                                if (itemsP1.Count == 0)
                                {
                                    prefabIndex1 = itemsP1.Count;
                                }
                                if (itemsP1.Count > 0)
                                {
                                    prefabIndex1 = itemsP1.Count - 1;
                                }

                            }
                            placeableObject1 = itemsP1[prefabIndex1];
                        }

                    }
                }
                if (itemsP2.Count > 0)
                {
                    if (!gamepad2.GetButton("A"))
                    {
                        if (gamepad2.GetButtonDown("RB"))
                        {
                            Destroy(currentPlaceableObject2);
                            if (prefabIndex2 < itemsP2.Count - 1)
                            {
                                prefabIndex2++;
                            }
                            else
                            {
                                prefabIndex2 = 0;
                            }
                            placeableObject2 = itemsP2[prefabIndex2];
                        }
                        if (gamepad2.GetButtonDown("LB"))
                        {
                            Destroy(currentPlaceableObject2);
                            if (prefabIndex2 > 0)
                            {
                                prefabIndex2--;
                            }
                            else
                            {
                                if (itemsP2.Count == 0)
                                {
                                    prefabIndex2 = itemsP2.Count;
                                }
                                if (itemsP2.Count > 0)
                                {
                                    prefabIndex2 = itemsP2.Count - 1;
                                }

                            }
                            placeableObject2 = itemsP2[prefabIndex2];
                        }

                    }
                }
                if (itemsP3.Count > 0)
                {
                    if (!gamepad3.GetButton("A"))
                    {
                        if (gamepad3.GetButtonDown("RB"))
                        {
                            Destroy(currentPlaceableObject3);
                            if (prefabIndex3 < itemsP3.Count - 1)
                            {
                                prefabIndex3++;
                            }
                            else
                            {
                                prefabIndex3 = 0;
                            }
                            placeableObject3 = itemsP3[prefabIndex3];
                        }
                        if (gamepad3.GetButtonDown("LB"))
                        {
                            Destroy(currentPlaceableObject3);
                            if (prefabIndex3 > 0)
                            {
                                prefabIndex3--;
                            }
                            else
                            {
                                if (itemsP3.Count == 0)
                                {
                                    prefabIndex3 = itemsP3.Count;
                                }
                                if (itemsP3.Count > 0)
                                {
                                    prefabIndex3 = itemsP3.Count - 1;
                                }

                            }
                            placeableObject3 = itemsP3[prefabIndex3];
                        }

                    }
                }
                break;
            case 4:
                if (itemsP1.Count > 0)
                {
                    if (!gamepad1.GetButton("A"))
                    {
                        if (gamepad1.GetButtonDown("RB"))
                        {
                            Destroy(currentPlaceableObject1);
                            if (prefabIndex1 < itemsP1.Count - 1)
                            {
                                prefabIndex1++;
                            }
                            else
                            {
                                prefabIndex1 = 0;
                            }
                            placeableObject1 = itemsP1[prefabIndex1];
                        }
                        if (gamepad1.GetButtonDown("LB"))
                        {
                            Destroy(currentPlaceableObject1);
                            if (prefabIndex1 > 0)
                            {
                                prefabIndex1--;
                            }
                            else
                            {
                                if (itemsP1.Count == 0)
                                {
                                    prefabIndex1 = itemsP1.Count;
                                }
                                if (itemsP1.Count > 0)
                                {
                                    prefabIndex1 = itemsP1.Count - 1;
                                }

                            }
                            placeableObject1 = itemsP1[prefabIndex1];
                        }

                    }
                }
                if (itemsP2.Count > 0)
                {
                    if (!gamepad2.GetButton("A"))
                    {
                        if (gamepad2.GetButtonDown("RB"))
                        {
                            Destroy(currentPlaceableObject2);
                            if (prefabIndex2 < itemsP2.Count - 1)
                            {
                                prefabIndex2++;
                            }
                            else
                            {
                                prefabIndex2 = 0;
                            }
                            placeableObject2 = itemsP2[prefabIndex2];
                        }
                        if (gamepad2.GetButtonDown("LB"))
                        {
                            Destroy(currentPlaceableObject2);
                            if (prefabIndex2 > 0)
                            {
                                prefabIndex2--;
                            }
                            else
                            {
                                if (itemsP2.Count == 0)
                                {
                                    prefabIndex2 = itemsP2.Count;
                                }
                                if (itemsP2.Count > 0)
                                {
                                    prefabIndex2 = itemsP2.Count - 1;
                                }

                            }
                            placeableObject2 = itemsP2[prefabIndex2];
                        }

                    }
                }
                if (itemsP3.Count > 0)
                {
                    if (!gamepad3.GetButton("A"))
                    {
                        if (gamepad3.GetButtonDown("RB"))
                        {
                            Destroy(currentPlaceableObject3);
                            if (prefabIndex3 < itemsP3.Count - 1)
                            {
                                prefabIndex3++;
                            }
                            else
                            {
                                prefabIndex3 = 0;
                            }
                            placeableObject3 = itemsP3[prefabIndex3];
                        }
                        if (gamepad3.GetButtonDown("LB"))
                        {
                            Destroy(currentPlaceableObject3);
                            if (prefabIndex3 > 0)
                            {
                                prefabIndex3--;
                            }
                            else
                            {
                                if (itemsP3.Count == 0)
                                {
                                    prefabIndex3 = itemsP3.Count;
                                }
                                if (itemsP3.Count > 0)
                                {
                                    prefabIndex3 = itemsP3.Count - 1;
                                }

                            }
                            placeableObject3 = itemsP3[prefabIndex3];
                        }

                    }
                }
                if (itemsP4.Count > 0)
                {
                    if (!gamepad4.GetButton("A"))
                    {
                        if (gamepad4.GetButtonDown("RB"))
                        {
                            Destroy(currentPlaceableObject4);
                            if (prefabIndex4 < itemsP4.Count - 1)
                            {
                                prefabIndex4++;
                            }
                            else
                            {
                                prefabIndex4 = 0;
                            }
                            placeableObject4 = itemsP4[prefabIndex4];
                        }
                        if (gamepad4.GetButtonDown("LB"))
                        {
                            Destroy(currentPlaceableObject4);
                            if (prefabIndex4 > 0)
                            {
                                prefabIndex4--;
                            }
                            else
                            {
                                if (itemsP4.Count == 0)
                                {
                                    prefabIndex4 = itemsP4.Count;
                                }
                                if (itemsP4.Count > 0)
                                {
                                    prefabIndex4 = itemsP4.Count - 1;
                                }

                            }
                            placeableObject4 = itemsP4[prefabIndex4];
                        }

                    }
                }
                break;
            default:
                break;
        }
    }

    //Release prefab when a button comes up from being pressed.
    void releasePrefab()
    {
        switch (psActor.playerCount)
        {
            case 1:
                //On button release
                if (gamepad1.GetButtonUp("A") && !cannotPlace1)
                {
                    if (itemsP1.Count > 0)
                    {
                        currentPlaceableObject1.tag = "Item";
                        //make current object copy null
                        //remove item at the index
                        currentPlaceableObject1 = null;
                        itemsP1.RemoveAt(prefabIndex1);
                        if (prefabIndex1 == (itemsP1.Count))
                        {
                            if (prefabIndex1 != 0)
                            {
                                prefabIndex1--;
                            }
                        }
                        if (itemsP1.Count > 1)
                        {

                            placeableObject1 = itemsP1[prefabIndex1];
                        }
                    }
                    if (itemsP1.Count == 0)
                    {
                        placeableObject1 = null;
                    }
                }
                else if (gamepad1.GetButtonUp("A") && cannotPlace1)
                {
                    Destroy(currentPlaceableObject1);
                }

                break;
            case 2:
                //if (gamepad1.GetButtonUp("A"))
                //{
                //    if (itemsP1.Count > 0)
                //    {
                //        currentPlaceableObject1 = null;
                //        itemsP1.RemoveAt(0);
                //        placeableObject1 = itemsP1[0];
                //    }
                //    if (itemsP1.Count == 0)
                //    {
                //        placeableObject1 = null;
                //    }
                //}
                if (gamepad1.GetButtonUp("A") && !cannotPlace1)
                {
                    if (itemsP1.Count > 0)
                    {
                        currentPlaceableObject1.tag = "Item";
                        //make current object copy null
                        //remove item at the index
                        currentPlaceableObject1 = null;
                        itemsP1.RemoveAt(prefabIndex1);
                        if (prefabIndex1 <= (itemsP1.Count))
                        {
                            if (prefabIndex1 != 0)
                            {
                                prefabIndex1--;
                            }
                        }
                        if (itemsP1.Count > 1)
                        {

                            placeableObject1 = itemsP1[prefabIndex1];

                        }
                    }
                    if (itemsP1.Count == 0)
                    {
                        placeableObject1 = null;

                    }
                }
                else if (gamepad1.GetButtonUp("A") && cannotPlace1)
                {
                    Destroy(currentPlaceableObject1);
                }

                if (gamepad2.GetButtonUp("A") && !cannotPlace2)
                {
                    if (itemsP2.Count > 0)
                    {
                        currentPlaceableObject2.tag = "Item";
                        //make current object copy null
                        //remove item at the index
                        currentPlaceableObject2 = null;
                        itemsP2.RemoveAt(prefabIndex2);
                        if (prefabIndex2 == (itemsP2.Count))
                        {
                            if (prefabIndex2 != 0)
                            {
                                prefabIndex2--;
                            }
                        }
                        if (itemsP2.Count >= 1)
                        {

                            placeableObject2 = itemsP2[prefabIndex2];
                        }
                    }
                    if (itemsP2.Count == 0)
                    {
                        placeableObject2 = null;
                    }
                }
                else if (gamepad2.GetButtonUp("A") && cannotPlace2)
                {
                    Destroy(currentPlaceableObject2);
                }
                break;
            case 3:
                if (gamepad1.GetButtonUp("A") && !cannotPlace1)
                {
                    if (itemsP1.Count > 0)
                    {
                        currentPlaceableObject1.tag = "Item";
                        //make current object copy null
                        //remove item at the index
                        currentPlaceableObject1 = null;
                        itemsP1.RemoveAt(prefabIndex1);
                        if (prefabIndex1 == (itemsP1.Count))
                        {
                            if (prefabIndex1 != 0)
                            {
                                prefabIndex1--;
                            }
                        }
                        if (itemsP1.Count >= 1)
                        {

                            placeableObject1 = itemsP1[prefabIndex1];
                        }
                    }
                    if (itemsP1.Count == 0)
                    {
                        placeableObject1 = null;
                    }
                }
                else if (gamepad1.GetButtonUp("A") && cannotPlace1)
                {
                    Destroy(currentPlaceableObject1);
                }
                if (gamepad2.GetButtonUp("A") && !cannotPlace2)
                {
                    if (itemsP2.Count > 0)
                    {
                        currentPlaceableObject2.tag = "Item";
                        //make current object copy null
                        //remove item at the index
                        currentPlaceableObject2 = null;
                        itemsP2.RemoveAt(prefabIndex2);
                        if (prefabIndex2 == (itemsP2.Count))
                        {
                            if (prefabIndex2 != 0)
                            {
                                prefabIndex2--;
                            }
                        }
                        if (itemsP2.Count >= 1)
                        {

                            placeableObject2 = itemsP2[prefabIndex2];
                        }
                    }
                    if (itemsP2.Count == 0)
                    {
                        placeableObject2 = null;
                    }
                }
                else if (gamepad2.GetButtonUp("A") && cannotPlace2)
                {
                    Destroy(currentPlaceableObject2);
                }
                if (gamepad3.GetButtonUp("A") && !cannotPlace3)
                {
                    if (itemsP3.Count > 0)
                    {
                        currentPlaceableObject3.tag = "Item";
                        //make current object copy null
                        //remove item at the index
                        currentPlaceableObject3 = null;
                        itemsP3.RemoveAt(prefabIndex3);
                        if (prefabIndex3 == (itemsP3.Count))
                        {
                            if (prefabIndex3 != 0)
                            {
                                prefabIndex3--;
                            }
                        }
                        if (itemsP3.Count >= 1)
                        {

                            placeableObject3 = itemsP3[prefabIndex3];
                        }
                    }
                    if (itemsP3.Count == 0)
                    {
                        placeableObject3 = null;
                    }
                }
                else if (gamepad3.GetButtonUp("A") && cannotPlace3)
                {
                    Destroy(currentPlaceableObject3);
                }
                break;
            case 4:
                if (gamepad1.GetButtonUp("A") && !cannotPlace1)
                {
                    if (itemsP1.Count > 0)
                    {
                        currentPlaceableObject1.tag = "Item";
                        //make current object copy null
                        //remove item at the index
                        currentPlaceableObject1 = null;
                        itemsP1.RemoveAt(prefabIndex1);
                        if (prefabIndex1 == (itemsP1.Count))
                        {
                            if (prefabIndex1 != 0)
                            {
                                prefabIndex1--;
                            }
                        }
                        if (itemsP1.Count >= 1)
                        {

                            placeableObject1 = itemsP1[prefabIndex1];
                        }
                    }
                    if (itemsP1.Count == 0)
                    {
                        placeableObject1 = null;
                    }
                }
                else if (gamepad1.GetButtonUp("A") && cannotPlace1)
                {
                    Destroy(currentPlaceableObject1);
                }
                if (gamepad2.GetButtonUp("A") && !cannotPlace1)
                {
                    if (itemsP2.Count > 0)
                    {
                        //make current object copy null
                        //remove item at the index
                        currentPlaceableObject2 = null;
                        itemsP2.RemoveAt(prefabIndex2);
                        if (prefabIndex2 == (itemsP2.Count))
                        {
                            if (prefabIndex2 != 0)
                            {
                                prefabIndex2--;
                            }
                        }
                        if (itemsP2.Count >= 1)
                        {

                            placeableObject2 = itemsP2[prefabIndex2];
                        }
                    }
                    if (itemsP2.Count == 0)
                    {
                        placeableObject2 = null;
                    }
                }
                else if (gamepad2.GetButtonUp("A") && cannotPlace1)
                {
                    Destroy(currentPlaceableObject2);
                }
                if (gamepad3.GetButtonUp("A") && !cannotPlace3)
                {
                    if (itemsP3.Count > 0)
                    {
                        currentPlaceableObject3.tag = "Item";
                        //make current object copy null
                        //remove item at the index
                        currentPlaceableObject3 = null;
                        itemsP3.RemoveAt(prefabIndex3);
                        if (prefabIndex3 == (itemsP3.Count))
                        {
                            if (prefabIndex3 != 0)
                            {
                                prefabIndex3--;
                            }
                        }
                        if (itemsP3.Count >= 1)
                        {

                            placeableObject3 = itemsP3[prefabIndex3];
                        }
                    }
                    if (itemsP3.Count == 0)
                    {
                        placeableObject3 = null;
                    }
                }
                else if (gamepad3.GetButtonUp("A") && cannotPlace3)
                {
                    Destroy(currentPlaceableObject3);
                }
                if (gamepad4.GetButtonUp("A") && !cannotPlace4)
                {
                    if (itemsP4.Count > 0)
                    {
                        currentPlaceableObject4.tag = "Item";
                        //make current object copy null
                        //remove item at the index
                        currentPlaceableObject4 = null;
                        itemsP4.RemoveAt(prefabIndex4);
                        if (prefabIndex4 == (itemsP4.Count))
                        {
                            if (prefabIndex4 != 0)
                            {
                                prefabIndex4--;
                            }
                        }
                        if (itemsP4.Count >= 1)
                        {

                            placeableObject4 = itemsP4[prefabIndex4];
                        }
                    }
                    if (itemsP4.Count == 0)
                    {
                        placeableObject4 = null;
                    }
                }
                else if(gamepad4.GetButtonUp("A") && cannotPlace4)
                {
                    Destroy(currentPlaceableObject4);
                }
                break;
            default:
                break;
        }

    }

    //Randomises items
    //Creates a list of random numbers between 0,3 (trap list size)
    void randomiseItems(int intToRand, List<int> playerList)
    {
        //First 5 are traps, second 5 are items.
        for (int i = 0; i < 6; ++i)
        {
            intToRand = Random.Range(0, 3);
            playerList.Add(intToRand);
        }

    }

    //Allocates random items to player list based on the random numbers generated.
    void allocateRandItems(List<int> numberList, List<GameObject> itemList, List<GameObject> trapList, List<GameObject> playerItemList)
    {
        //Trap Allocation.
        for (int i = 0; i < 3; ++i)
        {
            if (playerItemList.Count <= 12)
            {
                playerItemList.Add(trapList[numberList[i]]);

            }
         }

        //Item allocation.
        for (int i = 3; i < 6; ++i)
        {
            if (playerItemList.Count <= 12)
            {
                playerItemList.Add(itemList[numberList[i]]);
            }
        }
    }

    //Switches between item icons displaying current selected item, previous item and next item.
    //if itemlist index = certain trap or item assign icon.
    void switchItemIcons(int prefabIndex, Image currentItem, List<GameObject> playerItems)
    {
            if(playerItems.Count > 0)
            if (playerItems[prefabIndex] == buzzsaw)
            {
                currentItem.sprite = buzzsawIcon;
                currentItem.color = new Color(1, 1, 1, 1);
            }
        if (playerItems.Count > 0)
            if (playerItems[prefabIndex] == ramp)
            {
                currentItem.sprite = rampIcon;
                currentItem.color = new Color(1, 1, 1, 1);
            }
        if (playerItems.Count > 0)
            if (playerItems[prefabIndex] == oilslick)
            {
                currentItem.sprite = oilslickIcon;
                currentItem.color = new Color(1, 1, 1, 1);
            }
        if (playerItems.Count > 0)
            if (playerItems[prefabIndex] == rpg)
            {
                currentItem.sprite = rpgIcon;
                currentItem.color = new Color(1, 1, 1, 1);
            }
        if (playerItems.Count > 0)
            if (playerItems[prefabIndex] == mine)
            {
                currentItem.sprite = mineIcon;
                currentItem.color = new Color(1, 1, 1, 1);
            }
        if (playerItems.Count > 0)
            if (playerItems[prefabIndex] == boost)
            {
                currentItem.sprite = boostIcon;
                currentItem.color = new Color(1, 1, 1, 1);
            }
        if (playerItems.Count == 0)
        { 
                currentItem.sprite = blankIcon;
            currentItem.color = new Color(0,0,0,0);
        } 

    }

    private void switchPowerupIcons(Image powerupIcon, GameObject kart)
    {
        if(kart.GetComponent<PlayerActor>().itemRPG)
        {
            powerupIcon.sprite = rpgIcon;
        }

        if(kart.GetComponent<PlayerActor>().itemBoost)
        {
            powerupIcon.sprite = boostIcon;
        }

        if (kart.GetComponent<PlayerActor>().itemMine)
        {
            powerupIcon.sprite = mineIcon;
        }

        if (!kart.GetComponent<PlayerActor>().itemRPG && !kart.GetComponent<PlayerActor>().itemBoost && !kart.GetComponent<PlayerActor>().itemMine)
        {
            powerupIcon.sprite = blankPowerupIcon;
        }

    }

    void playerItemCountUi(List<GameObject> playerItems, Image playerSprites, Image playerSprites2)
    {
                switch(playerItems.Count)
                {
                    case 0:
                        playerSprites.sprite = nums[0];
                        playerSprites2.enabled = false;
                        break;
                    case 1:
                        playerSprites.sprite = nums[1];
                        playerSprites2.enabled = false;
                        break;
                    case 2:
                        playerSprites.sprite = nums[2];
                        playerSprites2.enabled = false;
                        break;
                    case 3:
                        playerSprites.sprite = nums[3];
                        playerSprites2.enabled = false;
                        break;
                    case 4:
                        playerSprites.sprite = nums[4];
                        playerSprites2.enabled = false;
                        break;
                    case 5:
                        playerSprites.sprite = nums[5];
                        playerSprites2.enabled = false;
                        break;
                    case 6:
                        playerSprites.sprite = nums[6];
                        playerSprites2.enabled = false;
                        break;
                    case 7:
                        playerSprites.sprite = nums[7];
                        playerSprites2.enabled = false;
                        break;
                    case 8:
                        playerSprites.sprite = nums[8];
                        playerSprites2.enabled = false;
                        break;
                    case 9:
                        playerSprites.sprite = nums[9];
                        playerSprites2.enabled = false;
                        break;
                    case 10:
                        playerSprites2.enabled = true;
                        playerSprites.sprite = nums[1];
                        playerSprites2.sprite = nums[0];
                        break;
                    case 11:
                        playerSprites2.enabled = true;
                        playerSprites.sprite = nums[1];
                        playerSprites2.sprite = nums[1];
                        break;
                    case 12:
                        playerSprites2.enabled = true;
                        playerSprites.sprite = nums[1];
                        playerSprites2.sprite = nums[2];
                        break;
                    default:
                        break;
                        
                }
    

        }   


}