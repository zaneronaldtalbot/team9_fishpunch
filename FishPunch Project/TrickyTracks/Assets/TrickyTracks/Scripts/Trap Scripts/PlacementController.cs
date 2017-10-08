using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlacementController : MonoBehaviour {

    private GameObject manager;
    private ItemManager itemManager;

    [SerializeField]
    private GameObject cursor, buzzSaw, oilSlick, boost, mine, rpg, ramp;
    public Camera camera;

    public float offSetFloat = 500.0f;

    private GameObject placeableObject;
    private List<GameObject> prefabs = new List<GameObject>();

    private GameObject currentPlaceableObject;

    private float yOffset = 0.5f;
    private float checkstuff;

    private int prefabIndex = 0;
    private RaycastHit raycopy;

    private float triggerRotationR;
    private float triggerRotationL;
    // Use this for initialization
    void Start () {
        manager = GameObject.Find("Manager");
        itemManager = manager.GetComponent<ItemManager>();
        prefabs.Add(buzzSaw);
        prefabs.Add(oilSlick);
        prefabs.Add(rpg);
        prefabs.Add(boost);
        prefabs.Add(mine);

        placeableObject = prefabs[0];
       
    }
	
	// Update is called once per frame
	void Update () {

      
            HandleNewObjectHotKey();
       
        Debug.DrawRay(camera.transform.position, camera.transform.forward * yOffset, Color.red);

        MoveCursor();
        if (currentPlaceableObject != null)
        {

            MoveCurrentObjectToStick();
            ChangePrefab();
            RotateWithTrigger();
            
            ReleaseIfPressed();
         
        }

        //if(itemManager.kart1.gamepad.GetButtonDown("RB"))
        //{
        //    ChangePrefab();
        //}

        //if (itemManager.kart1.gamepad.GetButtonDown("LB"))
        //{
        //    ChangePrefab();
        //}

    }

    private void HandleNewObjectHotKey()
    {

        if (itemManager.kart1.gamepad.GetButtonDown("Y"))
        {
            if (currentPlaceableObject != null)
            {
                Destroy(currentPlaceableObject);
            }
            else
            {
                currentPlaceableObject = Instantiate(placeableObject);

            }
        }


            

    }

    private void MoveCurrentObjectToStick()
    {
        Ray ray = camera.ScreenPointToRay(new Vector3(617, 313,0));
     
        
        RaycastHit hitInfo;
        if (Physics.Raycast(ray, out hitInfo))
        {
            raycopy = hitInfo;
            currentPlaceableObject.transform.position = hitInfo.point;
            currentPlaceableObject.transform.rotation = Quaternion.FromToRotation(Vector3.up, hitInfo.normal);
            
        }
    }

    private void RotateWithTrigger()
    {

        triggerRotationR = itemManager.kart1.gamepad.GetTrigger_R();
        triggerRotationL = itemManager.kart1.gamepad.GetTrigger_L();
        currentPlaceableObject.transform.Rotate(Vector3.up, triggerRotationR);
        currentPlaceableObject.transform.Rotate(Vector3.up, -triggerRotationL);
    }

    private void ReleaseIfPressed()
    {
        if(itemManager.kart1.gamepad.GetButtonDown("A"))
        {
            currentPlaceableObject = null;
        }
    }

    private void ChangePrefab()
    {
      
        
        if (itemManager.kart1.gamepad.GetButtonDown("RB"))
        {
            if (prefabIndex < 5)
            {
               
                prefabIndex++;
            }
            else
            {
                prefabIndex = 0;
            }
        }
        if (itemManager.kart1.gamepad.GetButtonDown("LB"))
        {
            if (prefabIndex > 0)
            {

                prefabIndex--;
            }
            else
            {
                prefabIndex = 5;
            }
        }
        placeableObject = prefabs[prefabIndex];
        
    }

    private void MoveCursor()
    {
        camera.transform.position += new Vector3(itemManager.kart1.gamepad.GetStick_L().X,0 , itemManager.kart1.gamepad.GetStick_L().Y);
        camera.transform.position += new Vector3(0, itemManager.kart1.gamepad.GetStick_R().Y, 0);
    
    }
}
