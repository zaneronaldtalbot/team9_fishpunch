using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlacementController : MonoBehaviour
{


    //Private manager variables to retrieve kart instances.
    private GameObject manager;
    private ItemManager itemManager;
    private PlayerSelectActor psActor;


    public float placingCountdown = 30.0f;

    //Game object prefabs
    [SerializeField]
    private GameObject cursor, buzzSaw, oilSlick, boost, mine, rpg, ramp;

    //Top Down Camera
    private Camera camera1, camera2, camera3, camera4;

    private xbox_gamepad gamepad1, gamepad2, gamepad3, gamepad4;
    private GameObject TrapCameraP1, TrapCameraP2, TrapCameraP3, TrapCameraP4;
    private GameObject currentPlaceableObjectP1, currentPlaceableObjectP2, currentPlaceableObjectP3, currentPlaceableObjectP4;
    private GameObject placeableObjectP1, placeableObjectP2, placeableObjectP3, placeableObjectP4;

    private int prefabIndex1 = 0, prefabIndex2 = 0, prefabIndex3 = 0, prefabIndex4 = 0;

    private List<GameObject> itemsP1 = new List<GameObject>(), itemsP2 = new List<GameObject>(),
                             itemsP3 = new List<GameObject>(), itemsP4 = new List<GameObject>();

    private List<int> randNumP1 = new List<int>(), randNumP2 = new List<int>(), 
                      randNumP3 = new List<int>(), randNumP4 = new List<int>();
   
    //List of all the different prefabs.
    private List<GameObject> prefabs = new List<GameObject>();

    private List<GameObject> itemPrefabs = new List<GameObject>();
    private List<GameObject> trapPrefabs = new List<GameObject>();

    private Text timerText;

    //Placeable object and currentplace object.

    private float yOffset = 0.5f;

    int layerMask;
    private int tempRandNum;

    //Floats that change rotation of kart.
    private float triggerRotationR;
    private float triggerRotationL;
    //  private float triggerRotation = 1;
    // Use this for initialization
    void Start()
    {
        manager = this.gameObject;
        itemManager = manager.GetComponent<ItemManager>();
        itemPrefabs.Add(rpg);
        itemPrefabs.Add(boost);
        itemPrefabs.Add(mine);

        trapPrefabs.Add(buzzSaw);
        trapPrefabs.Add(oilSlick);
        trapPrefabs.Add(ramp);


        prefabs.Add(buzzSaw);
        prefabs.Add(oilSlick);
        prefabs.Add(rpg);
        prefabs.Add(boost);
        prefabs.Add(mine);
        prefabs.Add(ramp);

   //     timerText = GameObject.Find("TimerText").GetComponent<Text>();

        TrapCameraP1 = GameObject.Find("TrapCamSetP1");
        TrapCameraP2 = GameObject.Find("TrapCamSetP2");
        TrapCameraP3 = GameObject.Find("TrapCamSetP3");
        TrapCameraP4 = GameObject.Find("TrapCamSetP4");

        if (TrapCameraP1 != null)
        {
            camera1 = GameObject.Find("TrapCamP1").GetComponent<Camera>();
            randomiseItems(tempRandNum, randNumP1);
            allocateRandItems(randNumP1, itemPrefabs, trapPrefabs, itemsP1);
            placeableObjectP1 = itemsP1[0];
            gamepad1 = GamePadManager.Instance.GetGamePad(1);
        }
        if (TrapCameraP2 != null)
        {
            camera1 = GameObject.Find("TrapCamP1").GetComponent<Camera>();
            camera2 = GameObject.Find("TrapCamP2").GetComponent<Camera>();
            randomiseItems(tempRandNum, randNumP1);
            allocateRandItems(randNumP1, itemPrefabs, trapPrefabs, itemsP1);
            randomiseItems(tempRandNum, randNumP2);
            allocateRandItems(randNumP2, itemPrefabs, trapPrefabs, itemsP2);
            placeableObjectP1 = itemsP1[0];
            placeableObjectP2 = itemsP2[0];
            gamepad1 = GamePadManager.Instance.GetGamePad(1);
            gamepad2 = GamePadManager.Instance.GetGamePad(2);
        }
        if (TrapCameraP3 != null)
        {
            camera1 = GameObject.Find("TrapCamP1").GetComponent<Camera>();
            camera2 = GameObject.Find("TrapCamP2").GetComponent<Camera>();
            camera3 = GameObject.Find("TrapCamP3").GetComponent<Camera>();
            randomiseItems(tempRandNum, randNumP1);
            allocateRandItems(randNumP1, itemPrefabs, trapPrefabs, itemsP1);
            randomiseItems(tempRandNum, randNumP2);
            allocateRandItems(randNumP2, itemPrefabs, trapPrefabs, itemsP2);
            randomiseItems(tempRandNum, randNumP3);
            allocateRandItems(randNumP3, itemPrefabs, trapPrefabs, itemsP3);
            placeableObjectP1 = itemsP1[0];
            placeableObjectP2 = itemsP2[0];
            placeableObjectP3 = itemsP3[0];
            gamepad1 = GamePadManager.Instance.GetGamePad(1);
            gamepad2 = GamePadManager.Instance.GetGamePad(2);
            gamepad3 = GamePadManager.Instance.GetGamePad(3);
        }
        if (TrapCameraP4 != null)
        {
            camera1 = GameObject.Find("TrapCamP1").GetComponent<Camera>();
            camera2 = GameObject.Find("TrapCamP2").GetComponent<Camera>();
            camera3 = GameObject.Find("TrapCamP3").GetComponent<Camera>();
            camera4 = GameObject.Find("TrapCamP4").GetComponent<Camera>();
            randomiseItems(tempRandNum, randNumP1);
            allocateRandItems(randNumP1, itemPrefabs, trapPrefabs, itemsP1);
            randomiseItems(tempRandNum, randNumP2);
            allocateRandItems(randNumP2, itemPrefabs, trapPrefabs, itemsP2);
            randomiseItems(tempRandNum, randNumP3);
            allocateRandItems(randNumP3, itemPrefabs, trapPrefabs, itemsP3);
            randomiseItems(tempRandNum, randNumP4);
            allocateRandItems(randNumP4, itemPrefabs, trapPrefabs, itemsP4);
            placeableObjectP1 = itemsP1[0];
            placeableObjectP2 = itemsP2[0];
            placeableObjectP3 = itemsP3[0];
            placeableObjectP4 = itemsP4[0];
            gamepad1 = GamePadManager.Instance.GetGamePad(1);
            gamepad2 = GamePadManager.Instance.GetGamePad(2);
            gamepad3 = GamePadManager.Instance.GetGamePad(3);
            gamepad4 = GamePadManager.Instance.GetGamePad(4);
        }



        layerMask = 1 << LayerMask.NameToLayer("Item");
        layerMask = ~layerMask;

    }

    // Update is called once per frame
    void Update()
    {

        placingCountdown -= Time.deltaTime;

        int intCountDown = (int)placingCountdown;
      //  timerText.text = "Time Left: " + intCountDown.ToString();

        if (placingCountdown < 0)
        {
        //    timerText.enabled = false;

            if (TrapCameraP1 != null)
                TrapCameraP1.SetActive(false);

            if (TrapCameraP2 != null)
                TrapCameraP2.SetActive(false);

            if (TrapCameraP3 != null)
                TrapCameraP3.SetActive(false);

            if (TrapCameraP4 != null)
                TrapCameraP4.SetActive(false);

        }

        handlePlayerMode();

    }

    private void ObjectGeneration()
    {

        //Create currentplaceable if its null.
        if (TrapCameraP1 != null)
        {
            if (currentPlaceableObjectP1 == null)
            {
                if (itemsP1.Count > 0)
                    currentPlaceableObjectP1 = Instantiate(placeableObjectP1);
            }
        }

        if (TrapCameraP2 != null)
        {
            if (currentPlaceableObjectP1 == null)
            {
                if (itemsP1.Count > 0)
                    currentPlaceableObjectP1 = Instantiate(placeableObjectP1);
            }

            if (currentPlaceableObjectP2 == null)
            {
                if (itemsP2.Count > 0)
                {
                    currentPlaceableObjectP2 = Instantiate(placeableObjectP2);
                }
            }
        }
        if (TrapCameraP3 != null)
        {
            if (currentPlaceableObjectP1 == null)
            {
                if (itemsP1.Count > 0)
                    currentPlaceableObjectP1 = Instantiate(placeableObjectP1);
            }

            if (currentPlaceableObjectP2 == null)
            {
                if (itemsP2.Count > 0)
                {
                    currentPlaceableObjectP2 = Instantiate(placeableObjectP2);
                }
            }
            if (currentPlaceableObjectP3 == null)
            {
                if (itemsP3.Count > 0)
                    currentPlaceableObjectP3 = Instantiate(placeableObjectP3);
            }
        }
        if (TrapCameraP4 != null)
        {
            if (currentPlaceableObjectP1 == null)
            {
                if (itemsP1.Count > 0)
                    currentPlaceableObjectP1 = Instantiate(placeableObjectP1);
            }

            if (currentPlaceableObjectP2 == null)
            {
                if (itemsP2.Count > 0)
                {
                    currentPlaceableObjectP2 = Instantiate(placeableObjectP2);
                }
            }
            if (currentPlaceableObjectP3 == null)
            {
                if (itemsP3.Count > 0)
                    currentPlaceableObjectP3 = Instantiate(placeableObjectP3);
            }
            if (currentPlaceableObjectP4 == null)
            {
                if(itemsP4.Count > 0)
                currentPlaceableObjectP4 = Instantiate(placeableObjectP4);
            }
        }


    }

    private void MoveCurrentObjectToStick(Camera camera, xbox_gamepad gamepad, GameObject currentPlaceableObject, float divideHeight, float divideWidth)
    {
        Ray ray = camera.ScreenPointToRay(new Vector3(Screen.width / divideWidth, Screen.height / divideHeight, 0));

        RaycastHit hitInfo;
        if (Physics.Raycast(ray, out hitInfo, 1000, layerMask))
        {
            currentPlaceableObject.transform.position = hitInfo.point;
            currentPlaceableObject.transform.rotation = Quaternion.FromToRotation(Vector3.up, new Vector3(hitInfo.normal.x, hitInfo.normal.y, hitInfo.normal.z));
            currentPlaceableObject.transform.Rotate(new Vector3(0, gamepad.triggerRotation, 0));

        }
    }

    private void RotateTrigger()
    {

        if (TrapCameraP1 != null)
        {
            gamepad1.triggerRotation += gamepad1.GetTrigger_R();
            gamepad1.triggerRotation -= gamepad1.GetTrigger_L();
        }

        if (TrapCameraP2 != null)
        {
            gamepad1.triggerRotation += gamepad1.GetTrigger_R();
            gamepad1.triggerRotation -= gamepad1.GetTrigger_L();
            gamepad2.triggerRotation += gamepad2.GetTrigger_R();
            gamepad2.triggerRotation -= gamepad2.GetTrigger_L();
        }

        if (TrapCameraP3 != null)
        {
            gamepad1.triggerRotation += gamepad1.GetTrigger_R();
            gamepad1.triggerRotation -= gamepad1.GetTrigger_L();

            gamepad2.triggerRotation += gamepad2.GetTrigger_R();
            gamepad2.triggerRotation -= gamepad3.GetTrigger_L();

            gamepad3.triggerRotation += gamepad3.GetTrigger_R();
            gamepad3.triggerRotation -= gamepad3.GetTrigger_L();
        }

        if (TrapCameraP4 != null)
        {
            gamepad1.triggerRotation += gamepad1.GetTrigger_R();
            gamepad1.triggerRotation -= gamepad1.GetTrigger_L();

            gamepad2.triggerRotation += gamepad2.GetTrigger_R();
            gamepad2.triggerRotation -= gamepad3.GetTrigger_L();

            gamepad3.triggerRotation += gamepad3.GetTrigger_R();
            gamepad3.triggerRotation -= gamepad3.GetTrigger_L();

            gamepad4.triggerRotation += gamepad4.GetTrigger_R();
            gamepad4.triggerRotation -= gamepad4.GetTrigger_L();

        }


        //currentPlaceableObject.transform.rotation = Quaternion.FromToRotation(Vector3.up, new Vector3(hitCopy.normal.x, hitCopy.normal.y + triggerRotation, hitCopy.normal.z));
    }

    private void ReleasePrefab()
    {
        //Releases object if A button is pressed.
        if (TrapCameraP1 != null)
        {
            if (gamepad1.GetButtonDown("A"))
            {
                if (itemsP1.Count > 0)
                {
                    currentPlaceableObjectP1.GetComponent<PrefabDisabledActor>().prefabSet = true;
                    currentPlaceableObjectP1 = null;
                    itemsP1.RemoveAt(prefabIndex1);
                    placeableObjectP1 = itemsP1[prefabIndex1];
                }
                if (itemsP1.Count == 0)
                {
                    placeableObjectP1 = null;
                }
            }
        }
        if (TrapCameraP2 != null)
        {
            if (gamepad1.GetButtonDown("A"))
            {
                if (itemsP1.Count > 0)
                {
                    currentPlaceableObjectP1 = null;
                    itemsP1.RemoveAt(prefabIndex1);
                    placeableObjectP1 = itemsP1[prefabIndex1];
                }
                if (itemsP1.Count == 0)
                {
                    placeableObjectP1 = null;
                }
            }
            if (gamepad2.GetButtonDown("A"))
            {
                if (itemsP2.Count > 0)
                {
                    currentPlaceableObjectP2 = null;
                    itemsP2.RemoveAt(prefabIndex2);
                    if (prefabIndex2 > (itemsP2.Count - 1))
                    {
                        placeableObjectP2 = itemsP2[prefabIndex2 - 1];
                    }
                    else
                    {
                        placeableObjectP2 = itemsP2[prefabIndex2];
                    }
                }
                if (itemsP2.Count == 0)
                {
                    placeableObjectP2 = null;
                }
            }
        }
        if (TrapCameraP3 != null)
        {
            if (gamepad1.GetButtonDown("A"))
            {
                if (itemsP1.Count > 0)
                {
                    currentPlaceableObjectP1 = null;
                    itemsP1.RemoveAt(prefabIndex1);
                    placeableObjectP1 = itemsP1[prefabIndex1];
                }
                if (itemsP1.Count == 0)
                {
                    placeableObjectP1 = null;
                }
            }
            if (gamepad2.GetButtonDown("A"))
            {
                if (itemsP2.Count > 0)
                {
                    currentPlaceableObjectP2 = null;
                    itemsP2.RemoveAt(prefabIndex2);
                    placeableObjectP2 = itemsP2[prefabIndex2];
                }
                if (itemsP2.Count == 0)
                {
                    placeableObjectP2 = null;
                }
            }
            if (gamepad3.GetButtonDown("A"))
            {
                if (itemsP3.Count > 0)
                {
                    currentPlaceableObjectP3 = null;
                    itemsP3.RemoveAt(prefabIndex3);
                    placeableObjectP3 = itemsP3[prefabIndex3];
                }
                if (itemsP3.Count == 0)
                {
                    placeableObjectP3 = null;
                }
            }
        }
        if (TrapCameraP4 != null)
        {
            if (gamepad1.GetButtonDown("A"))
            {
                if (itemsP1.Count > 0)
                {
                    currentPlaceableObjectP1 = null;
                    itemsP1.RemoveAt(prefabIndex1);
                    placeableObjectP1 = itemsP1[prefabIndex1];
                }
                if (itemsP1.Count == 0)
                {
                    placeableObjectP1 = null;
                }
            }
            if (gamepad2.GetButtonDown("A"))
            {
                if (itemsP2.Count > 0)
                {
                    currentPlaceableObjectP2 = null;
                    itemsP2.RemoveAt(prefabIndex2);
                    placeableObjectP2 = itemsP2[prefabIndex2];
                }
                if (itemsP2.Count == 0)
                {
                    placeableObjectP2 = null;
                }
            }
            if (gamepad3.GetButtonDown("A"))
            {
                if (itemsP3.Count > 0)
                {
                    currentPlaceableObjectP3 = null;
                    itemsP3.RemoveAt(prefabIndex3);
                    placeableObjectP3 = itemsP3[prefabIndex3];
                }
                if (itemsP3.Count == 0)
                {
                    placeableObjectP3 = null;
                }
            }
            if (gamepad4.GetButtonDown("A"))
            {
                if (itemsP4.Count > 0)
                {
                    currentPlaceableObjectP4 = null;
                    itemsP4.RemoveAt(prefabIndex4);
                    placeableObjectP4 = itemsP3[prefabIndex4];
                }
                if (itemsP4.Count == 0)
                {
                    placeableObjectP4 = null;
                }
            }
        }
    }

    private void ChangePrefab()
    {
        //If RB or LB is pressed destroy the current object
        //and add or subtract from the prefab index
        if (TrapCameraP1 != null)
        {
            if (gamepad1.GetButtonDown("RB"))
            {
               
                Destroy(currentPlaceableObjectP1);
                if (prefabIndex1 < itemsP1.Count)
                {

                    prefabIndex1++;
                }
                else
                {
                    prefabIndex1 = 0;
                }
            }
            if (gamepad1.GetButtonDown("LB"))
            {
                Destroy(currentPlaceableObjectP1);
                if (prefabIndex1 > 0)
                {

                    prefabIndex1--;
                }
                else
                {
                    prefabIndex1 = itemsP1.Count;
                }
            }
            placeableObjectP1 = itemsP1[prefabIndex1];
        }

        if (TrapCameraP2 != null)
        {
            if (itemsP1.Count > 0)
            {
                if (gamepad1.GetButtonDown("RB"))
                {
                    Destroy(currentPlaceableObjectP1);
                    if (prefabIndex1 < (itemsP1.Count - 1))
                    {
                        prefabIndex1++;
                    }
                    else
                    {
                        prefabIndex1 = 0;
                    }
                }
                if (gamepad1.GetButtonDown("LB"))
                {
                    Destroy(currentPlaceableObjectP1);
                    if (prefabIndex1 > 0)
                    {
                        prefabIndex1--;
                    }
                    else
                    {
                        if (itemsP1.Count != 0)
                        {
                            prefabIndex1 = (itemsP1.Count - 1);
                        }
                        if (itemsP1.Count == 0)
                        {
                            prefabIndex1 = itemsP1.Count;
                        }
                    }
                }
                placeableObjectP1 = itemsP1[prefabIndex1];
            }
            if (itemsP2.Count > 0)
            {
                if (gamepad2.GetButtonDown("RB"))
                {
                    Destroy(currentPlaceableObjectP2);
                    if (prefabIndex2 < (itemsP2.Count - 1))
                    {

                        prefabIndex2++;
                    }
                    else
                    {
                        prefabIndex2 = 0;
                    }
                }
                if (gamepad2.GetButtonDown("LB"))
                {
                    Destroy(currentPlaceableObjectP2);
                    if (prefabIndex2 > 0)
                    {

                        prefabIndex2--;
                    }
                    else
                    {
                        if (itemsP2.Count != 0)
                        {
                            prefabIndex2 = (itemsP2.Count - 1);
                        }
                        if (itemsP2.Count == 0)
                        {
                            prefabIndex2 = itemsP2.Count;
                        }
                    }
                }
                placeableObjectP2 = itemsP2[prefabIndex2];
            }
        }

        if (TrapCameraP3 != null)
        {
            if (itemsP1.Count > 0)
            {
                if (gamepad1.GetButtonDown("RB"))
                {
                    Destroy(currentPlaceableObjectP1);
                    if (prefabIndex1 < (itemsP1.Count - 1))
                    {
                        prefabIndex1++;
                    }
                    else
                    {
                        prefabIndex1 = 0;
                    }
                }
                if (gamepad1.GetButtonDown("LB"))
                {
                    Destroy(currentPlaceableObjectP1);
                    if (prefabIndex1 > 0)
                    {
                        prefabIndex1--;
                    }
                    else
                    {
                        prefabIndex1 = itemsP1.Count;
                    }
                    placeableObjectP1 = itemsP1[prefabIndex1];
                }
            }
            if (itemsP2.Count > 0)
            {
                if (gamepad2.GetButtonDown("RB"))
                {
                    Destroy(currentPlaceableObjectP2);
                    if (prefabIndex2 < (itemsP2.Count - 1))
                    {

                        prefabIndex2++;
                    }
                    else
                    {
                        prefabIndex2 = 0;
                    }
                }
                if (gamepad2.GetButtonDown("LB"))
                {
                    Destroy(currentPlaceableObjectP2);
                    if (prefabIndex2 > 0)
                    {

                        prefabIndex2--;
                    }
                    else
                    {
                        if (itemsP2.Count != 0)
                        {
                            prefabIndex2 = (itemsP2.Count - 1);
                        }
                        if (itemsP2.Count == 0)
                        {
                            prefabIndex2 = itemsP2.Count;
                        }
                    }
                }
                placeableObjectP2 = itemsP2[prefabIndex2];
            }

            if (itemsP3.Count > 0)
            {
                if (gamepad3.GetButtonDown("RB"))
                {
                    Destroy(currentPlaceableObjectP3);
                    if (prefabIndex3 < (itemsP3.Count - 1))
                    {

                        prefabIndex3++;
                    }
                    else
                    {
                        prefabIndex3 = 0;
                    }
                }
                if (gamepad3.GetButtonDown("LB"))
                {
                    Destroy(currentPlaceableObjectP3);
                    if (prefabIndex3 > 0)
                    {

                        prefabIndex3--;
                    }
                    else
                    {
                        if (itemsP3.Count != 0)
                        {
                            prefabIndex2 = (itemsP3.Count - 1);
                        }
                        if (itemsP3.Count == 0)
                        {
                            prefabIndex3 = itemsP3.Count;
                        }
                    }
                }
                placeableObjectP3 = itemsP3[prefabIndex3];
            }
        }
        if (TrapCameraP4 != null)
        {
            if (itemsP1.Count > 0)
            {
                if (gamepad1.GetButtonDown("RB"))
                {
                    Destroy(currentPlaceableObjectP1);
                    if (prefabIndex1 < (itemsP1.Count - 1))
                    {
                        prefabIndex1++;
                    }
                    else
                    {
                        prefabIndex1 = 0;
                    }
                }
                if (gamepad1.GetButtonDown("LB"))
                {
                    Destroy(currentPlaceableObjectP1);
                    if (prefabIndex1 > 0)
                    {
                        prefabIndex1--;
                    }
                    else
                    {
                        prefabIndex1 = itemsP1.Count;
                    }
                    placeableObjectP1 = itemsP1[prefabIndex1];
                }
            }
            if (itemsP2.Count > 0)
            {
                if (gamepad2.GetButtonDown("RB"))
                {
                    Destroy(currentPlaceableObjectP2);
                    if (prefabIndex2 < (itemsP2.Count - 1))
                    {

                        prefabIndex2++;
                    }
                    else
                    {
                        prefabIndex2 = 0;
                    }
                }
                if (gamepad2.GetButtonDown("LB"))
                {
                    Destroy(currentPlaceableObjectP2);
                    if (prefabIndex2 > 0)
                    {

                        prefabIndex2--;
                    }
                    else
                    {
                        if (itemsP2.Count != 0)
                        {
                            prefabIndex2 = (itemsP2.Count - 1);
                        }
                        if (itemsP2.Count == 0)
                        {
                            prefabIndex2 = itemsP2.Count;
                        }
                    }
                }
                placeableObjectP2 = itemsP2[prefabIndex2];
            }

            if (itemsP3.Count > 0)
            {
                if (gamepad3.GetButtonDown("RB"))
                {
                    Destroy(currentPlaceableObjectP3);
                    if (prefabIndex3 < (itemsP3.Count - 1))
                    {

                        prefabIndex3++;
                    }
                    else
                    {
                        prefabIndex3 = 0;
                    }
                }
                if (gamepad3.GetButtonDown("LB"))
                {
                    Destroy(currentPlaceableObjectP3);
                    if (prefabIndex3 > 0)
                    {

                        prefabIndex3--;
                    }
                    else
                    {
                        if (itemsP3.Count != 0)
                        {
                            prefabIndex3 = (itemsP3.Count - 1);
                        }
                        if (itemsP3.Count == 0)
                        {
                            prefabIndex3 = itemsP3.Count;
                        }
                    }
                }
                placeableObjectP3 = itemsP3[prefabIndex3];
            }

            if (itemsP4.Count > 0)
            {
                if (gamepad4.GetButtonDown("RB"))
                {
                    Destroy(currentPlaceableObjectP4);
                    if (prefabIndex4 < (itemsP4.Count - 1))
                    {

                        prefabIndex4++;
                    }
                    else
                    {
                        prefabIndex4 = 0;
                    }
                }
                if (gamepad4.GetButtonDown("LB"))
                {
                    Destroy(currentPlaceableObjectP4);
                    if (prefabIndex4 > 0)
                    {

                        prefabIndex4--;
                    }
                    else
                    {
                        if (itemsP4.Count != 0)
                        {
                            prefabIndex4 = (itemsP4.Count - 1);
                        }
                        if (itemsP3.Count == 0)
                        {
                            prefabIndex4 = itemsP4.Count;
                        }
                    }
                }
                placeableObjectP4 = itemsP4[prefabIndex4];
            }
        }
    }

    private void MoveCursor(Camera camera, xbox_gamepad gamepad)
    {
        //Change cameras position based on gamepad input.
        camera.transform.position += new Vector3(gamepad.GetStick_L().X, 0, gamepad.GetStick_L().Y);
        camera.transform.position += new Vector3(0, gamepad.GetStick_R().Y, 0);

    }

    void handlePlayerMode()
    {

        switch (GamePadManager.Instance.ConnectedTotal())
        {
            case 1:

                ObjectGeneration();
                if (currentPlaceableObjectP1 != null)
                {
                    RotateTrigger();
                    MoveCurrentObjectToStick(camera1, gamepad1, currentPlaceableObjectP1, 2, 2);
                    ChangePrefab();
                    ReleasePrefab();
                }
                MoveCursor(camera1, gamepad1);
                break;
            case 2:
                ObjectGeneration();
                if (currentPlaceableObjectP1 != null && currentPlaceableObjectP2 != null)
                {
                    RotateTrigger();
                    MoveCurrentObjectToStick(camera1, gamepad1, currentPlaceableObjectP1, 1.3f, 2);
                    MoveCurrentObjectToStick(camera2, gamepad2, currentPlaceableObjectP2, 4, 2);
                    ChangePrefab();
                    ReleasePrefab();

                }
                MoveCursor(camera1, gamepad1);
                MoveCursor(camera2, gamepad2);
                break;
            case 3:
                ObjectGeneration();

                if (currentPlaceableObjectP1 != null && currentPlaceableObjectP2 != null &&
                   currentPlaceableObjectP3 != null)
                {
                    RotateTrigger();

                    MoveCurrentObjectToStick(camera1, gamepad1, currentPlaceableObjectP1, 1.3f, 4);
                    MoveCurrentObjectToStick(camera2, gamepad2, currentPlaceableObjectP2, 1.3f, 1.35f);
                    MoveCurrentObjectToStick(camera3, gamepad3, currentPlaceableObjectP3, 4, 4);
                    ChangePrefab();
                    ReleasePrefab();

                }
                MoveCursor(camera1, gamepad1);
                MoveCursor(camera2, gamepad2);
                MoveCursor(camera3, gamepad3);
                break;
            case 4:
                ObjectGeneration();
                if (currentPlaceableObjectP1 != null && currentPlaceableObjectP2 != null &&
                   currentPlaceableObjectP3 != null && currentPlaceableObjectP4 != null)
                {
                    RotateTrigger();
                    MoveCurrentObjectToStick(camera1, gamepad1, currentPlaceableObjectP1, 1.3f, 4);
                    MoveCurrentObjectToStick(camera2, gamepad2, currentPlaceableObjectP2, 1.3f, 1.35f);
                    MoveCurrentObjectToStick(camera3, gamepad3, currentPlaceableObjectP3, 4, 4);
                    MoveCurrentObjectToStick(camera4, gamepad4, currentPlaceableObjectP4, 4, 1.35f);
                    ChangePrefab();
                    ReleasePrefab();
                }
                MoveCursor(camera1, gamepad1);
                MoveCursor(camera2, gamepad2);
                MoveCursor(camera3, gamepad3);
                MoveCursor(camera4, gamepad4);
                break;
        }
    }

    void randomiseItems(int intToRand, List<int> playerList)
    {
        for (int i = 0; i < 10; ++i)
        {
            intToRand = Random.Range(0, 3);
            playerList.Add(intToRand);
        }
    }

    void allocateRandItems(List<int> numberList, List<GameObject> itemList, List<GameObject> trapList, List<GameObject> playerItemList)
    {
        //Trap Allocation.
        for (int i = 0; i < 5; ++i)
        {
            playerItemList.Add(trapList[numberList[i]]);
        }

        //Item allocation.
        for (int i = 5; i < 10; ++i)
        {
            playerItemList.Add(itemList[numberList[i]]);
        }

    }
}