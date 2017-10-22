using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NewPlacementController : MonoBehaviour
{

    //Manager variables to access other scripts on the manager.
    private GameObject manager;
    private GamePadManager gpManager;
    private PlayerSelectActor psActor;

    //Copy of item lists.
    private GameObject itemListcopy1, itemListCopy2, itemListCopy3, itemListCopy4;


    public float prefabRotationSpeed = 2.0f;

    //Player Gamepad Instances
    private xbox_gamepad gamepad1, gamepad2, gamepad3, gamepad4;

    //Images for item UI that displays current item, next item and previous item.
    private Image currentItem1, nextItem1, previousItem1;
    private Image currentItem2, nextItem2, previousItem2;
    private Image currentItem3, nextItem3, previousItem3;
    private Image currentItem4, nextItem4, previousItem4;

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

    //kart gameobjects.
    private GameObject kart1, kart2, kart3, kart4;

    //List with all trap and all item prefabs respectively.
    private List<GameObject> trapPrefabs = new List<GameObject>();
    private List<GameObject> itemPrefabs = new List<GameObject>();

    //list that stores random numbers for each player so they receive different items every lap.
    private List<int> randNumP1 = new List<int>(), randNumP2 = new List<int>(),
                      randNumP3 = new List<int>(), randNumP4 = new List<int>();

    //The list where player items are stored.
    private List<GameObject> itemsP1 = new List<GameObject>(), itemsP2 = new List<GameObject>(),
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

        //add traps to trap list.
        trapPrefabs.Add(buzzsaw);
        trapPrefabs.Add(oilslick);
        trapPrefabs.Add(ramp);

        //add items to item list
        itemPrefabs.Add(boost);
        itemPrefabs.Add(rpg);
        itemPrefabs.Add(mine);

        //Get image for each players item UI sprite.
        currentItem1 = GameObject.Find("CurrentItem1").GetComponent<Image>();
        nextItem1 = GameObject.Find("NextItem1").GetComponent<Image>();
        previousItem1 = GameObject.Find("PreviousItem1").GetComponent<Image>();
        currentItem2 = GameObject.Find("CurrentItem2").GetComponent<Image>();
        nextItem2 = GameObject.Find("NextItem2").GetComponent<Image>();
        previousItem2 = GameObject.Find("PreviousItem2").GetComponent<Image>();
        currentItem3 = GameObject.Find("CurrentItem3").GetComponent<Image>();
        nextItem3 = GameObject.Find("NextItem3").GetComponent<Image>();
        previousItem3 = GameObject.Find("PreviousItem3").GetComponent<Image>();
        currentItem4 = GameObject.Find("CurrentItem4").GetComponent<Image>();
        nextItem4 = GameObject.Find("NextItem4").GetComponent<Image>();
        previousItem4 = GameObject.Find("PreviousItem4").GetComponent<Image>();


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
                nextItem2.enabled = false;
                previousItem2.enabled = false;
                currentItem3.enabled = false;
                nextItem3.enabled = false;
                previousItem3.enabled = false;
                currentItem4.enabled = false;
                nextItem4.enabled = false;
                previousItem4.enabled = false;

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
                nextItem2.enabled = false;
                previousItem2.enabled = false;
                currentItem4.enabled = false;
                nextItem4.enabled = false;
                previousItem4.enabled = false;



                break;
            case 3:
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

                break;
            case 4:
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

        objectGeneration();
        releasePrefab();
        rotatePrefab();
        changePrefab();
       

        switch (psActor.playerCount)
        {
            case 1:

                if (kart1.GetComponent<KartActor2>().assignNewTraps)
                {
                    randNumP1.Clear();
                    randomiseItems(randTempNum1, randNumP1);
                    allocateRandItems(randNumP1, itemPrefabs, trapPrefabs, itemsP1);
                    kart1.GetComponent<KartActor2>().assignNewTraps = false;
                    placeableObject1 = itemsP1[0];
                }

                if (currentPlaceableObject1 != null)
                {
                    fitPrefabToTrack(raycastObject1, currentPlaceableObject1, gamepad1);
                }

                switchItemIcons(prefabIndex1, currentItem1, nextItem1, previousItem1, itemsP1);

                break;
            case 2:
                if (kart1.GetComponent<KartActor2>().assignNewTraps)
                {
                    randNumP1.Clear();
                    randomiseItems(randTempNum1, randNumP1);
                    allocateRandItems(randNumP1, itemPrefabs, trapPrefabs, itemsP1);
                    kart1.GetComponent<KartActor2>().assignNewTraps = false;
                    placeableObject1 = itemsP1[0];
                }
                if (kart2.GetComponent<KartActor2>().assignNewTraps)
                {
                    randNumP2.Clear();
                    randomiseItems(randTempNum1, randNumP2);
                    allocateRandItems(randNumP2, itemPrefabs, trapPrefabs, itemsP2);
                    kart2.GetComponent<KartActor2>().assignNewTraps = false;
                    placeableObject2 = itemsP2[0];
                }
                if (currentPlaceableObject1 != null)
                {
                    fitPrefabToTrack(raycastObject1, currentPlaceableObject1, gamepad1);
                }
                if (currentPlaceableObject2 != null)
                {
                    fitPrefabToTrack(raycastObject2, currentPlaceableObject2, gamepad2);
                }

                switchItemIcons(prefabIndex1, currentItem1, nextItem1, previousItem1, itemsP1);
                switchItemIcons(prefabIndex2, currentItem3, nextItem3, previousItem3, itemsP2);

                break;
            case 3:
                if (kart1.GetComponent<KartActor2>().assignNewTraps)
                {
                    randNumP1.Clear();
                    randomiseItems(randTempNum1, randNumP1);
                    allocateRandItems(randNumP1, itemPrefabs, trapPrefabs, itemsP1);
                    kart1.GetComponent<KartActor2>().assignNewTraps = false;
                    placeableObject1 = itemsP1[0];
                }
                if (kart2.GetComponent<KartActor2>().assignNewTraps)
                {
                    randNumP2.Clear();
                    randomiseItems(randTempNum1, randNumP2);
                    allocateRandItems(randNumP2, itemPrefabs, trapPrefabs, itemsP2);
                    kart2.GetComponent<KartActor2>().assignNewTraps = false;
                    placeableObject2 = itemsP2[0];
                }
                if (kart3.GetComponent<KartActor2>().assignNewTraps)
                {
                    randNumP3.Clear();
                    randomiseItems(randTempNum1, randNumP3);
                    allocateRandItems(randNumP3, itemPrefabs, trapPrefabs, itemsP3);
                    kart3.GetComponent<KartActor2>().assignNewTraps = false;
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



                break;
            case 4:
                if (kart1.GetComponent<KartActor2>().assignNewTraps)
                {
                    randNumP1.Clear();
                    randomiseItems(randTempNum1, randNumP1);
                    allocateRandItems(randNumP1, itemPrefabs, trapPrefabs, itemsP1);
                    kart1.GetComponent<KartActor2>().assignNewTraps = false;
                }
                if (kart2.GetComponent<KartActor2>().assignNewTraps)
                {
                    randNumP2.Clear();
                    randomiseItems(randTempNum1, randNumP2);
                    allocateRandItems(randNumP2, itemPrefabs, trapPrefabs, itemsP2);
                    kart2.GetComponent<KartActor2>().assignNewTraps = false;
                }
                if (kart3.GetComponent<KartActor2>().assignNewTraps)
                {
                    randNumP3.Clear();
                    randomiseItems(randTempNum1, randNumP3);
                    allocateRandItems(randNumP3, itemPrefabs, trapPrefabs, itemsP3);
                    kart3.GetComponent<KartActor2>().assignNewTraps = false;
                }
                if (kart4.GetComponent<KartActor2>().assignNewTraps)
                {
                    randNumP4.Clear();
                    randomiseItems(randTempNum1, randNumP4);
                    allocateRandItems(randNumP4, itemPrefabs, trapPrefabs, itemsP4);
                    kart4.GetComponent<KartActor2>().assignNewTraps = false;
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
                break;
            case 3:
                break;
            case 4:
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
                if(gamepad1.GetButtonUp("A"))
                {
                    if(itemsP1.Count > 0)
                    {
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
                    if(itemsP1.Count == 0)
                    {
                        placeableObject1 = null;
                    }
                }

                break;
            case 2:
                if (gamepad1.GetButtonUp("A"))
                {
                    if (itemsP1.Count > 0)
                    {
                        currentPlaceableObject1 = null;
                        itemsP1.RemoveAt(0);
                        placeableObject1 = itemsP1[0];
                    }
                    if (itemsP1.Count == 0)
                    {
                        placeableObject1 = null;
                    }
                }
                if (gamepad2.GetButtonUp("A"))
                {
                    if (itemsP2.Count > 0)
                    {
                        currentPlaceableObject2 = null;
                        itemsP2.RemoveAt(0);
                        placeableObject2 = itemsP2[0];
                    }
                    if (itemsP2.Count == 0)
                    {
                        placeableObject2 = null;
                    }
                }

                break;
            case 3:
                if (gamepad1.GetButtonUp("A"))
                {
                    if (itemsP1.Count > 0)
                    {
                        currentPlaceableObject1 = null;
                        itemsP1.RemoveAt(0);
                        placeableObject1 = itemsP1[0];
                    }
                    if (itemsP1.Count == 0)
                    {
                        placeableObject1 = null;
                    }
                }
                if (gamepad2.GetButtonUp("A"))
                {
                    if (itemsP2.Count > 0)
                    {
                        currentPlaceableObject2 = null;
                        itemsP2.RemoveAt(0);
                        placeableObject2 = itemsP2[0];
                    }
                    if (itemsP2.Count == 0)
                    {
                        placeableObject2 = null;
                    }
                }
                if (gamepad3.GetButtonUp("A"))
                {
                    if (itemsP3.Count > 0)
                    {
                        currentPlaceableObject3 = null;
                        itemsP3.RemoveAt(0);
                        placeableObject3 = itemsP3[0];
                    }
                    if (itemsP3.Count == 0)
                    {
                        placeableObject3 = null;
                    }
                }
                break;
            case 4:
                if (gamepad1.GetButtonUp("A"))
                {
                    if (itemsP1.Count > 0)
                    {
                        currentPlaceableObject1 = null;
                        itemsP1.RemoveAt(0);
                        placeableObject1 = itemsP1[0];
                    }
                    if (itemsP1.Count == 0)
                    {
                        placeableObject1 = null;
                    }
                }
                if (gamepad2.GetButtonUp("A"))
                {
                    if (itemsP2.Count > 0)
                    {
                        currentPlaceableObject2 = null;
                        itemsP2.RemoveAt(0);
                        placeableObject2 = itemsP2[0];
                    }
                    if (itemsP2.Count == 0)
                    {
                        placeableObject2 = null;
                    }
                }
                if (gamepad3.GetButtonUp("A"))
                {
                    if (itemsP3.Count > 0)
                    {
                        currentPlaceableObject3 = null;
                        itemsP3.RemoveAt(0);
                        placeableObject3 = itemsP3[0];
                    }
                    if (itemsP3.Count == 0)
                    {
                        placeableObject3 = null;
                    }
                }
                if (gamepad4.GetButtonUp("A"))
                {
                    if (itemsP4.Count > 0)
                    {
                        currentPlaceableObject4 = null;
                        itemsP4.RemoveAt(0);
                        placeableObject4 = itemsP4[0];
                    }
                    if (itemsP4.Count == 0)
                    {
                        placeableObject4 = null;
                    }
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
            playerItemList.Add(trapList[numberList[i]]);
        }

        //Item allocation.
        for (int i = 3; i < 6; ++i)
        {
            playerItemList.Add(itemList[numberList[i]]);
        }

    }

    //Switches between item icons displaying current selected item, previous item and next item.
    //if itemlist index = certain trap or item assign icon.
    void switchItemIcons(int prefabIndex, Image currentItem, Image nextItem, Image previousItem, List<GameObject> playerItems)
    {

        if (playerItems.Count > 0)
        {
            if (playerItems[prefabIndex] == buzzsaw)
            {
                currentItem.sprite = buzzsawIcon;
            }
            if (playerItems[prefabIndex] == ramp)
            {
                currentItem.sprite = rampIcon;
            }
            if (playerItems[prefabIndex] == oilslick)
            {
                currentItem.sprite = oilslickIcon;
            }
            if (playerItems[prefabIndex] == rpg)
            {
                currentItem.sprite = rpgIcon;
            }
            if (playerItems[prefabIndex] == mine)
            {
                currentItem.sprite = mineIcon;
            }
            if (playerItems[prefabIndex] == boost)
            {
                currentItem.sprite = boostIcon;
            }
            if(playerItems[prefabIndex] == null)
            {

            }

            if(prefabIndex == (playerItems.Count - 1))
            {
                if(playerItems[0] == boost)
                {
                    nextItem.sprite = boostIcon;
                }
                if(playerItems[0] == buzzsaw)
                {
                    nextItem.sprite = buzzsawIcon;
                }
                if(playerItems[0] == ramp)
                {
                    nextItem.sprite = rampIcon;
                }
                if(playerItems[0] == boost)
                {
                    nextItem.sprite = boostIcon;
                }
                if(playerItems[0] == rpg)
                {
                    nextItem.sprite = rpgIcon;
                }
                if(playerItems[0] == mine)
                {
                    nextItem.sprite = mineIcon;
                }
                if(playerItems[0] == null)
                {

                }
            }
            else if(playerItems.Count > 1)
            {
                if(playerItems[prefabIndex + 1] == boost)
                {
                    nextItem.sprite = boostIcon;
                }
                if (playerItems[prefabIndex + 1] == rpg)
                {
                    nextItem.sprite = rpgIcon;
                }
                if (playerItems[prefabIndex + 1] == mine)
                {
                    nextItem.sprite = mineIcon;
                }
                if (playerItems[prefabIndex + 1] == ramp)
                {
                    nextItem.sprite = rampIcon;
                }
                if (playerItems[prefabIndex + 1] == buzzsaw)
                {
                    nextItem.sprite = buzzsawIcon;
                }
                if (playerItems[prefabIndex + 1] == oilslick)
                {
                    nextItem.sprite = oilslickIcon;
                }
            }


            if (prefabIndex == 0)
            {
                if (playerItems[(playerItems.Count -1)] == buzzsaw)
                {
                    previousItem.sprite = buzzsawIcon;
                }
                if (playerItems[(playerItems.Count - 1)] == oilslick)
                {
                    previousItem.sprite = oilslickIcon;
                }
                if (playerItems[(playerItems.Count - 1)] == ramp)
                {
                    previousItem.sprite = rampIcon;
                }
                if (playerItems[(playerItems.Count - 1)] == mine)
                {
                    previousItem.sprite = mineIcon;
                }
                if (playerItems[(playerItems.Count - 1)] == boost)
                {
                    previousItem.sprite = boostIcon;
                }
                if(playerItems[(playerItems.Count - 1)] == rpg)
                {
                    previousItem.sprite = rpgIcon;
                }
            }
            else if(playerItems.Count > 1)
            {
                if(playerItems[prefabIndex - 1] == boost)
                {
                    previousItem.sprite = boostIcon;
                }
                if (playerItems[prefabIndex - 1] == mine)
                {
                    previousItem.sprite = mineIcon;
                }
                if (playerItems[prefabIndex - 1] == rpg)
                {
                    previousItem.sprite = rpgIcon;
                }
                if (playerItems[prefabIndex - 1] == ramp)
                {
                    previousItem.sprite = rampIcon;
                }
                if (playerItems[prefabIndex - 1] == oilslick)
                {
                    previousItem.sprite = oilslickIcon;
                }
                if (playerItems[prefabIndex - 1] == buzzsaw)
                {
                    previousItem.sprite = buzzsawIcon;
                }
                if(playerItems[prefabIndex - 1] == null)
                {

                }

            }
        }

    }
}