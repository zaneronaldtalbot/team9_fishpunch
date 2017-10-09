using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlacementController : MonoBehaviour {


    //Private manager variables to retrieve kart instances.
    private GameObject manager;
    private ItemManager itemManager;


    //Game object prefabs
    [SerializeField]
    private GameObject cursor, buzzSaw, oilSlick, boost, mine, rpg, ramp;

    //Top Down Camera
    public Camera camera;

    public float offSetFloat = 500.0f;

    //List of all the different prefabs.
    private List<GameObject> prefabs = new List<GameObject>();

    //Placeable object and currentplace object.
    private GameObject placeableObject;
    private GameObject currentPlaceableObject;

    private float yOffset = 0.5f;

    int layerMask;

    private Ray rayCopy;
    private RaycastHit hitCopy;

    //Index for the prefab list to switch between prefabs.
    private int prefabIndex = 0;

    //Floats that change rotation of kart.
    private float triggerRotationR;
    private float triggerRotationL;
    private float triggerRotation = 1;
    // Use this for initialization
    void Start () {
        manager = GameObject.Find("Manager");
        itemManager = manager.GetComponent<ItemManager>();
        prefabs.Add(buzzSaw);
        prefabs.Add(oilSlick);
        prefabs.Add(rpg);
        prefabs.Add(boost);
        prefabs.Add(mine);
        prefabs.Add(ramp);

        placeableObject = prefabs[0];

        layerMask = 1 << LayerMask.NameToLayer("Item");
        layerMask = ~layerMask;
       
    }
	
	// Update is called once per frame
	void Update () {

        Debug.Log("Trigger: " + triggerRotation);
      
            ObjectGeneration();
       
        Debug.DrawRay(rayCopy.origin, rayCopy.direction, Color.red);

       
        if (currentPlaceableObject != null)
        {
            RotateTrigger();
         //   RotateWithTrigger();
            MoveCurrentObjectToStick();
            ChangePrefab();
            
            
            ReleaseIfPressed();
         
        }
        MoveCursor();
        //if(itemManager.kart1.gamepad.GetButtonDown("RB"))
        //{
        //    ChangePrefab();
        //}

        //if (itemManager.kart1.gamepad.GetButtonDown("LB"))
        //{
        //    ChangePrefab();
        //}

    }

    private void ObjectGeneration()
    {

        //if (currentPlaceableObject != null)
        //{
        //    Destroy(currentPlaceableObject);
        //}
        //else
        //{
        //Create currentplaceable if its null.
        if (currentPlaceableObject == null)
        {
            currentPlaceableObject = Instantiate(placeableObject);
        }
               

    }

    private void MoveCurrentObjectToStick()
    {
        Ray ray = camera.ScreenPointToRay(new Vector3(600, 300,0));

        rayCopy = ray;
        RaycastHit hitInfo;
        if (Physics.Raycast(ray, out hitInfo, 1000, layerMask))
        {
            currentPlaceableObject.transform.position = hitInfo.point;
            currentPlaceableObject.transform.rotation = Quaternion.FromToRotation(Vector3.up, new Vector3(hitInfo.normal.x, hitInfo.normal.y, hitInfo.normal.z));
            currentPlaceableObject.transform.Rotate(new Vector3(0, triggerRotation, 0));
            hitCopy = hitInfo;
            
        }
    }

    private void RotateTrigger()
    {
       
            triggerRotation += itemManager.kart1.gamepad.GetTrigger_R();
        
        
            triggerRotation -= itemManager.kart1.gamepad.GetTrigger_L();
        
        //currentPlaceableObject.transform.rotation = Quaternion.FromToRotation(Vector3.up, new Vector3(hitCopy.normal.x, hitCopy.normal.y + triggerRotation, hitCopy.normal.z));
    }

    private void RotateWithTrigger()
    {
        //Rotate based on trigger input.
        triggerRotationR = itemManager.kart1.gamepad.GetTrigger_R();
        triggerRotationL = itemManager.kart1.gamepad.GetTrigger_L();
        currentPlaceableObject.transform.Rotate(Vector3.up, triggerRotationR);
        currentPlaceableObject.transform.Rotate(Vector3.up, -triggerRotationL);
    }

    private void ReleaseIfPressed()
    {
        //Releases object if A button is pressed.
        if(itemManager.kart1.gamepad.GetButtonDown("A"))
        {
            currentPlaceableObject = null;
        }
    }

    private void ChangePrefab()
    {
      
        //If RB or LB is pressed destroy the current object
        //and add or subtract from the prefab index
        if (itemManager.kart1.gamepad.GetButtonDown("RB"))
        {
            Destroy(currentPlaceableObject);
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
            Destroy(currentPlaceableObject);
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
        //Change cameras position based on gamepad input.
        camera.transform.position += new Vector3(itemManager.kart1.gamepad.GetStick_L().X,0 , itemManager.kart1.gamepad.GetStick_L().Y);
        camera.transform.position += new Vector3(0, itemManager.kart1.gamepad.GetStick_R().Y, 0);
    
    }
}
