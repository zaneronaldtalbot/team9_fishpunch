using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GamePadManager : MonoBehaviour {
    
    //How many gamepads
    public int GamePadCount = 4;

    private List<xbox_gamepad> gamepads;
    private static GamePadManager manager;
    private LapsManager lpManager;

    private PlayerSelectActor psActor;
    
    //Public GameObjects.
    [Header("Kart Gamepad Prefabs")]
    private GameObject player1;
    private GameObject player2;
    private GameObject player3;
    private GameObject player4;

    private GameObject TrapCamP1, TrapCamP2, TrapCamP3, TrapCamP4;
    private PlacementController placementController;
    private ItemManager itemManager;
    private NewPlacementController newPlacementController;

    private Scene currentScene;

    private bool loadPlayerOnce = false;
    private bool loadTrapCamOnce = false;
    private bool findGameObjects = false;
	// initialize.
	void Awake () {
         
        //Create manager if one doesn't exist.
        if (manager != null && manager != this)
        {
            Destroy(this.gameObject);
            return;
        }
        else
        {
            //Create new gamepad manager instance
            manager = this;
            DontDestroyOnLoad(this.gameObject);

            itemManager = GetComponent<ItemManager>();
            lpManager = GetComponent<LapsManager>();
            newPlacementController = GetComponent<NewPlacementController>();
            placementController = GetComponent<PlacementController>();
            psActor = GetComponent<PlayerSelectActor>();

            //Lock gamepadcount based on range
            GamePadCount = Mathf.Clamp(GamePadCount, 1, 4);

            gamepads = new List<xbox_gamepad>();

            //Create gamepads based on gamepad count.
            for(int i =0; i < GamePadCount; ++i)
            {
                gamepads.Add(new xbox_gamepad(i + 1));
            }
           
        }

	}
	
	// Update is called once per frame
	void Update () {

        currentScene = SceneManager.GetActiveScene();
        if (currentScene.buildIndex == 2)
        {
            lpManager.enabled = true;
            itemManager.enabled = true;
      
            psActor.enabled = false;

            newPlacementController.enabled = true;  
            if (!findGameObjects)
            {
                player1 = GameObject.Find("1 Player");

                player2 = GameObject.Find("2 Player");

                player3 = GameObject.Find("3 Player");

                player4 = GameObject.Find("4 Player");

                TrapCamP1 = GameObject.Find("TrapCamSetP1");

                TrapCamP2 = GameObject.Find("TrapCamSetP2");

                TrapCamP3 = GameObject.Find("TrapCamSetP3");

                TrapCamP4 = GameObject.Find("TrapCamSetP4");

                findGameObjects = true;
            }

            if (!loadPlayerOnce)
            {
                activatePrefab();
                loadPlayerOnce = true;
            }

            if (!loadTrapCamOnce)
            {
           //     activateTraps();
                loadTrapCamOnce = true; 
            }

           
        }

        //Update gamepads.
		for(int i = 0; i < gamepads.Count; ++i)
        {
            gamepads[i].Update();
        }
        //Activate prefabs based on connected controllers.
      
    }

    public void Refresh()
    {
        for(int i = 0; i < gamepads.Count; ++i)
        {
            gamepads[i].Refresh();
        }
    }

    public static GamePadManager Instance
    {
        get
        {
            if(manager == null)
            {
                Debug.LogError("[GamePadManger]: Instance does not exist.");
                return null;
            }
            //Returns the instance of the gamepad manager if one exists.
            return manager;
        }
    }

    public xbox_gamepad GetGamePad(int index)
    {
        //Search gamepads for indexed gamepad.
        for(int i = 0; i < gamepads.Count;)
        {
            //indexes match, return this gamepad
            if(gamepads[i].Index == (index - 1))
            {
                return gamepads[i];
            }
            else
            {
                ++i;
            }
        }

        Debug.LogError("[GamepadManager]: " + index + "is not a valid gamepad.");

        return null;
    }

    public int ConnectedTotal()
    {
        int total = 0;

        //Adds 1 to the total for each gamepad that is connected.
        for(int i = 0; i < gamepads.Count; ++i)
        {
            if(gamepads[i].IsConnected)
            {
                total++;
            }
        }

        return total;
    }

    public bool GetButtonAny(string button)
    {
        for(int i = 0; i < gamepads.Count; ++i)
        {
            //gamepad meets both conditions
            if(gamepads[i].IsConnected && gamepads[i].GetButton(button))
            {
                return true;
            }
        }

        return false;
    }

    public bool GetButtonDownAny(string button)
    {
        for(int i = 0; i < gamepads.Count; ++i)
        {
            //gamepad meets both conditions
            if(gamepads[i].IsConnected && gamepads[i].GetButtonDown(button))
            {
                return true;
            }
        }

        return false;

    }

    public void activatePrefab()
    {
        //Activates the prefab depending on the connected total of controllers.
        if (ConnectedTotal() == 1 && player1 != null)
        {
            player1.SetActive(true);

            player2.SetActive(false);
        
            player3.SetActive(false);
        
            player4.SetActive(false);
            
        }
        else if (ConnectedTotal() == 2 && player2 != null)
        {
            player1.SetActive(false);
            player2.SetActive(true);
            player3.SetActive(false);
            player4.SetActive(false);
        }
        else if (ConnectedTotal() == 3 && player3 != null)
        {
            player3.SetActive(true);
            player1.SetActive(false);
            player2.SetActive(false);
            player4.SetActive(false);
        }
        else if (ConnectedTotal() == 4 && player4 != null)
        {
            player1.SetActive(false);
            player2.SetActive(false);
            player3.SetActive(false);
            player4.SetActive(true);
        }
    }

    //void activateTraps()
    //{

    //    switch(ConnectedTotal())
    //    {
    //        case 1:
    //            TrapCamP1.SetActive(true);
    //            TrapCamP2.SetActive(false);
    //            TrapCamP3.SetActive(false);
    //            TrapCamP4.SetActive(false);
    //            break;
    //        case 2:
    //            TrapCamP1.SetActive(false);
    //            TrapCamP2.SetActive(true);
    //            TrapCamP3.SetActive(false);
    //            TrapCamP4.SetActive(false);
    //            break;
    //        case 3:
    //            TrapCamP1.SetActive(false);
    //            TrapCamP2.SetActive(false);
    //            TrapCamP3.SetActive(true);
    //            TrapCamP4.SetActive(false);
    //            break;
    //        case 4:
    //            TrapCamP1.SetActive(false);
    //            TrapCamP2.SetActive(false);
    //            TrapCamP3.SetActive(false);
    //            TrapCamP4.SetActive(true);
    //            break;
    //        default:
    //            break;
    //    }

    //}

}
