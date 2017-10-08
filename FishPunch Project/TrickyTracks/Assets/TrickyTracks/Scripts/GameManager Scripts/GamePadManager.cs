using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamePadManager : MonoBehaviour {
    
    //How many gamepads
    public int GamePadCount = 1;

    private List<xbox_gamepad> gamepads;
    private static GamePadManager manager;

    [Header("Kart Gamepad Prefabs")]
    public GameObject player1, player2, player3, player4;

	// initialize.
	void Awake () {
         
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

            //Lock gamepadcount based on range
            GamePadCount = Mathf.Clamp(GamePadCount, 1, 4);

            gamepads = new List<xbox_gamepad>();

            //Create specified number
            for(int i =0; i < GamePadCount; ++i)
            {
                gamepads.Add(new xbox_gamepad(i + 1));
            }
           
        }

	}
	
	// Update is called once per frame
	void Update () {
		for(int i = 0; i < gamepads.Count; ++i)
        {
            gamepads[i].Update();
        }
        activatePrefab();
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
            return manager;
        }
    }

    public xbox_gamepad GetGamePad(int index)
    {
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
        if (ConnectedTotal() == 1)
        {
            player1.SetActive(true);
        }
        else if (ConnectedTotal() == 2)
        {
            player2.SetActive(true);
        }
        else if (ConnectedTotal() == 3)
        {
            player3.SetActive(true);
        }
        else if (ConnectedTotal() == 4)
        {
            player4.SetActive(true);
        }
       
       
     
    }
}
