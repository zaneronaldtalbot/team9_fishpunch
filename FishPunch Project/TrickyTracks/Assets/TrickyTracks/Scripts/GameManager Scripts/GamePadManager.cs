using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//Written by Angus Secomb
//Last edited 14/11/17
public class GamePadManager : MonoBehaviour {
    
    //How many gamepads
    public int GamePadCount = 4;

    private Audio audioActor;

    //List of gamepads
    private List<xbox_gamepad> gamepads;

    //Manager variables.
    private static GamePadManager manager;
    private PositionManager posManager;
    private LapsManager lpManager;
    private PlayerSelectActor psActor;
    
    //Public GameObjects.
    [Header("Kart Gamepad Prefabs")]
    private GameObject player2;
    private GameObject player3;
    private GameObject player4;

    private WinActor winActor;


    private ItemManager itemManager;
    private NewPlacementController newPlacementController;

    [HideInInspector]
    public bool isPaused = false;

    private Scene currentScene;

    public AudioSource countSound;
    public AudioSource countSoundEnd;

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

            audioActor = GetComponent<Audio>();
            itemManager = GetComponent<ItemManager>();
            lpManager = GetComponent<LapsManager>();
            posManager = GetComponent<PositionManager>();
            newPlacementController = GetComponent<NewPlacementController>();
            psActor = GetComponent<PlayerSelectActor>();
            winActor = GetComponent<WinActor>();

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

        //Get current scene.
        currentScene = SceneManager.GetActiveScene();
        //If scene is the main scene.
        if (currentScene.buildIndex == 2)
        {
            psActor.enabled = false;
            //enable lap, item, position managers.
            //disable player select actor
            lpManager.enabled = true;
            itemManager.enabled = true;
            posManager.enabled = true;
            winActor.enabled = true;
      
           // audioActor.enabled = true;

            GamePadCount = psActor.playerCount;

            newPlacementController.enabled = true;  
            
            //Find gameobjects.
            if (!findGameObjects)
            {

                player2 = GameObject.Find("2 Player");

                player3 = GameObject.Find("3 Player");

                player4 = GameObject.Find("4 Player");
                findGameObjects = true;
            }

            if (!loadPlayerOnce)
            {
                activatePrefab();
                loadPlayerOnce = true;
            }
           
        }

        //Update gamepads.
		for(int i = 0; i < gamepads.Count; ++i)
        {
            gamepads[i].Update();
        }
        //Activate prefabs based on connected controllers.
      
    }

    //Refresh all gamepads.
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
        if (psActor.playerCount == 2 && player2 != null)
        {
            player2.SetActive(true);
            player3.SetActive(false);
            player4.SetActive(false);
        }
        else if (psActor.playerCount == 3 && player3 != null)
        {
            player3.SetActive(true);
            player2.SetActive(false);
            player4.SetActive(false);
        }
        else if (psActor.playerCount == 4 && player4 != null)
        {
            player2.SetActive(false);
            player3.SetActive(false);
            player4.SetActive(true);
        }
    }

}
