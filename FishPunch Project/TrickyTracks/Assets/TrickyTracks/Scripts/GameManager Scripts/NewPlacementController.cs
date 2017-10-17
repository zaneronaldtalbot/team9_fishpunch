using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewPlacementController : MonoBehaviour {

    private GameObject manager;
    private GamePadManager gpManager;
    private PlayerSelectActor psActor;

    private GameObject itemListcopy1, itemListCopy2, itemListCopy3, itemListCopy4;

    public float prefabRotationSpeed = 2.0f;

    private xbox_gamepad gamepad1, gamepad2, gamepad3, gamepad4;

    [Header("Traps")]
    public GameObject buzzsaw;
    public GameObject ramp;
    public GameObject oilslick;
    public GameObject boostsaw;

    [Header("Items")]
    public GameObject boost;
    public GameObject rpg;
    public GameObject mine;

    private GameObject raycastObject1, raycastObject2, raycastObject3, raycastObject4;

    private GameObject currentPlaceableObject1, currentPlaceableObject2,
                       currentPlaceableObject3, currentPlaceableObject4;

    private GameObject placeableObject1, placeableObject2,
                       placeableObject3, placeableObject4;

    private GameObject kart1, kart2, kart3, kart4;

    private List<GameObject> trapPrefabs = new List<GameObject>();
    private List<GameObject> itemPrefabs = new List<GameObject>();

    private List<int> randNumP1 = new List<int>(), randNumP2 = new List<int>(),
                      randNumP3 = new List<int>(), randNumP4 = new List<int>();

    private List<GameObject> itemsP1 = new List<GameObject>(), itemsP2 = new List<GameObject>(), 
                             itemsP3 = new List<GameObject>(), itemsP4 = new List<GameObject>();

    private int randTempNum1;

    int layerMask;

    // Use this for initialization
    void Start() {
        manager = this.gameObject;
        gpManager = GetComponent<GamePadManager>();
        psActor = manager.GetComponent<PlayerSelectActor>();

        trapPrefabs.Add(buzzsaw);
        trapPrefabs.Add(oilslick);
        trapPrefabs.Add(ramp);

        itemPrefabs.Add(boost);
        itemPrefabs.Add(rpg);
        itemPrefabs.Add(mine);

        switch(psActor.playerCount)
        {
            case 1:
                randomiseItems(randTempNum1, randNumP1);
                allocateRandItems(randNumP1, itemPrefabs, trapPrefabs, itemsP1);
                placeableObject1 = itemsP1[0];
                gamepad1 = GamePadManager.Instance.GetGamePad(1);

                raycastObject1 = GameObject.Find("RayCast1");
                kart1 = GameObject.Find("PlayerCharacter_001");

                break;
            case 2:

                randomiseItems(randTempNum1, randNumP1);
                allocateRandItems(randNumP1, itemPrefabs, trapPrefabs, itemsP1);
                randomiseItems(randTempNum1, randNumP2);
                allocateRandItems(randNumP2, itemPrefabs, trapPrefabs, itemsP2);
                placeableObject1 = itemsP1[0];
                placeableObject2 = itemsP2[0];
                gamepad1 = GamePadManager.Instance.GetGamePad(1);
                gamepad2 = GamePadManager.Instance.GetGamePad(2);

                raycastObject1 = GameObject.Find("RayCast1");
                raycastObject2 = GameObject.Find("RayCast2");
                kart1 = GameObject.Find("PlayerCharacter_001");
                kart2 = GameObject.Find("PlayerCharacter_002");

                break;
            case 3:
                randomiseItems(randTempNum1, randNumP1);
                allocateRandItems(randNumP1, itemPrefabs, trapPrefabs, itemsP1);
                randomiseItems(randTempNum1, randNumP2);
                allocateRandItems(randNumP2, itemPrefabs, trapPrefabs, itemsP2);
                randomiseItems(randTempNum1, randNumP3);
                allocateRandItems(randNumP3, itemPrefabs, trapPrefabs, itemsP3);
                placeableObject1 = itemsP1[0];
                placeableObject2 = itemsP2[0];
                placeableObject3 = itemsP3[0];

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
    void Update() {

        objectGeneration();
        releasePrefab();
        rotatePrefab();

        switch(psActor.playerCount)
        {
            case 1:
                if(currentPlaceableObject1 != null)
                {
                    fitPrefabToTrack(raycastObject1, currentPlaceableObject1, gamepad1);
                }
                break;
            case 2:
                if (currentPlaceableObject1 != null)
                {
                    fitPrefabToTrack(raycastObject1, currentPlaceableObject1, gamepad1);
                }
                if (currentPlaceableObject2 != null)
                {
                    fitPrefabToTrack(raycastObject2, currentPlaceableObject2, gamepad2);
                }
                break;
            case 3:
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
                break;
            default:
                break;
        }
    }


    void objectGeneration()
    {
        switch(psActor.playerCount)
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

    void fitPrefabToTrack(GameObject raycastObject, GameObject currentPlaceableObject, xbox_gamepad gamepad)
    {

        RaycastHit hitInfo;
       if(Physics.Raycast(raycastObject.transform.position, -raycastObject.transform.up, out hitInfo, 1000, layerMask ))
       {
            currentPlaceableObject.transform.position = hitInfo.point;
        
            currentPlaceableObject.transform.rotation = Quaternion.FromToRotation(Vector3.up, new Vector3(hitInfo.normal.x, hitInfo.normal.y, hitInfo.normal.z));
            currentPlaceableObject.transform.forward = raycastObject.transform.forward;
            currentPlaceableObject.transform.Rotate(0, gamepad.triggerRotation, 0);

        }
    }

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
            if (gamepad1.GetButtonDown("A"))
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
            if (gamepad2.GetButtonDown("A"))
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
            if (gamepad1.GetButtonDown("A"))
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
            if (gamepad2.GetButtonDown("A"))
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
            if (gamepad3.GetButtonDown("A"))
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
            if (gamepad1.GetButtonDown("A"))
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
            if (gamepad2.GetButtonDown("A"))
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
            if (gamepad3.GetButtonDown("A"))
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
            if (gamepad4.GetButtonDown("A"))
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

    void changePrefab()
    {
        switch (psActor.playerCount)
        {
            case 1:
                if (itemsP1.Count > 0)
                {
                    if (gamepad1.GetButtonDown("RB"))
                    {
                        itemListcopy1 = itemsP1[0];
                        itemsP1[0] = itemsP1[1];

                        //itemsP1[2] 
                    }
                }
                break;
            case 2:
                break;
            case 3:
                break;
            case 4:
                break;
            default:
                break;
        }
    }

    void releasePrefab()
    {
        switch(psActor.playerCount)
        {
            case 1:
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

    void randomiseItems(int intToRand,List<int> playerList)
    {
        //First 5 are traps, second 5 are items.
        for(int i = 0; i < 6; ++i)
        {
            intToRand = Random.Range(0, 3);
            playerList.Add(intToRand);
        }

    }

    void allocateRandItems(List<int> numberList, List<GameObject> itemList, List<GameObject> trapList, List<GameObject> playerItemList)
    {
        //Trap Allocation.
        for (int i = 0; i < 3; ++i)
        {
            playerItemList.Add(trapList[numberList[i]]);
        }

        //Item allocation.
        for(int i = 3; i < 6; ++i)
        {
            playerItemList.Add(itemList[numberList[i]]);
        }

    }
}
