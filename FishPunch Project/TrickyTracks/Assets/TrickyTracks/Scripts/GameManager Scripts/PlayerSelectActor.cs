using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

//Written by Angus Secomb
//Last edited 20/11/17
public class PlayerSelectActor : MonoBehaviour
{
    private GamePadManager gpManager;
    private GameObject manager;

    [HideInInspector]
    public int playerCount = 0;

    public GameObject player1Ready, player2Ready, player3Ready, player4Ready;
    public GameObject player1Ready2, player2Ready2, player3Ready2, player4Ready2;

    public GameObject platformP1, platformP2, platformP3, platformP4;

    public GameObject kartOne, kartTwo, kartThree, kartFour;

    public AudioSource beepBeep;

    public GameObject go_timer;
    [HideInInspector]
   public bool playerReady1 = false;
    [HideInInspector]
    public bool playerReady2 = false;
    [HideInInspector]
   public bool playerReady3 = false;
    [HideInInspector]
    public bool playerReady4 = false;

    private int playerOneIndex = 0, playerTwoIndex = 0, playerThreeIndex = 0, playerFourIndex = 0;

    Image playB, exitB;

    int buttonIndex = 1;

    xbox_gamepad gamepad;


    public Image player1, player2, player3, player4;

    public Sprite red, blue, green, yellow, white;

    List<xbox_gamepad> gamepads = new List<xbox_gamepad>();

    float deadZone = 0.9f;

    float coolDown = 0.3f;
    float cdCopy = 0.3f;


    // Use this for initialization
    void Start()
    {
        
        manager = GameObject.Find("Manager");
        gpManager = manager.GetComponent<GamePadManager>();
        playB = GameObject.Find("Play").GetComponent<Image>();
        exitB = GameObject.Find("Exit").GetComponent<Image>();
        playB.color = Color.grey;
        exitB.color = Color.grey;
        gamepads = new List<xbox_gamepad>();

        for (int i = 1; i <= GamePadManager.Instance.GamePadCount; ++i)
        {
            gamepads.Add(GamePadManager.Instance.GetGamePad(i));
        }


    }

    // Update is called once per frame
    void Update()
    {

       
        switch(playerCount)
        {
            case 1:

                platformP1.SetActive(true);
                kartOne.SetActive(true);

                break;
            case 2:

                platformP2.SetActive(true);
                kartTwo.SetActive(true);

                break;
            case 3:
                platformP3.SetActive(true);
                kartThree.SetActive(true);
        
                break;
            case 4:
                platformP4.SetActive(true);
                kartFour.SetActive(true);
                break;

        }

        if(playerCount > 1)
        {
          
            if(buttonIndex == 1)
            {
                playB.color = Color.yellow;
                exitB.color = Color.grey;
            }
            if(buttonIndex == 2)
            {
                exitB.color = Color.yellow;
                playB.color = Color.grey;
            }
            
        }

        coolDown -= Time.deltaTime;



            for(int i = 0; i < playerCount; ++i)
        {
            switch (playerCount)
            {

                case 2:
                    if ((playerReady1 && playerReady2) || (playerReady1 && playerReady3) || (playerReady1 && playerReady4) || (playerReady2 && playerReady3) ||
                  (playerReady2 && playerReady4) || (playerReady3 && playerReady4))
                    {

                        if (gamepads[i].GetButtonDown("A") && buttonIndex == 1)
                        {
                            SceneManager.LoadScene("Peter's Map");
                        }
                        else if (gamepads[i].GetButtonDown("A") && buttonIndex == 2)
                        {
                            GameObject.Destroy(manager);
                            GameObject.Destroy(GameObject.Find("Music"));
                            SceneManager.LoadScene("Set_Pieces");
                        }

                    }


                    break;
                case 3:
                    if ((playerReady1 && playerReady2) || (playerReady1 && playerReady3) || (playerReady1 && playerReady4) || (playerReady2 && playerReady3) ||
                   (playerReady2 && playerReady4) || (playerReady3 && playerReady4))
                    {
                        if (gamepads[i].GetButtonDown("A") && buttonIndex == 1)
                        {
                            SceneManager.LoadScene("Peter's Map");
                        }
                        else if (gamepads[i].GetButtonDown("A") && buttonIndex == 2)
                        {
                            GameObject.Destroy(manager);
                            GameObject.Destroy(GameObject.Find("Music"));
                            SceneManager.LoadScene("Set_Pieces");
                        }

                    }
                    break;
                case 4:
                    {
                        if ((playerReady1 && playerReady2) || (playerReady1 && playerReady3) || (playerReady1 && playerReady4) || (playerReady2 && playerReady3) ||
                                          (playerReady2 && playerReady4) || (playerReady3 && playerReady4))
                        {

                            if (gamepads[i].GetButtonDown("A") && buttonIndex == 1)
                            {
                                SceneManager.LoadScene("Peter's Map");
                            }
                            else if (gamepads[i].GetButtonDown("A") && buttonIndex == 2)
                            {
                                GameObject.Destroy(manager);
                                GameObject.Destroy(GameObject.Find("Music"));
                                SceneManager.LoadScene("Set_Pieces");
                            }

                        }
                        break;
                    }
            }

        if (gamepads[0].GetStick_L().Y > deadZone && (coolDown < 0) && buttonIndex == 2)
        {
            coolDown = cdCopy;
            buttonIndex = 1;
            
        }


        if (gamepads[0].GetStick_L().Y < -deadZone && (coolDown < 0) && buttonIndex == 1)
        {
            coolDown = cdCopy;
            buttonIndex = 2;
        }
      }


        int n = GamePadManager.Instance.ConnectedTotal();
        for (int i = 1; i <= n; ++i)
        {
               
               startPressed(i);
        }

     
      
    }

    void selectKart(int playerIndex)
    {




    }


    void startPressed(int index)
    {
    
        gamepad = GamePadManager.Instance.GetGamePad(index);


            if (gamepad.GetButtonDown("Start"))
            {


            switch (index)
            {
                case 0:
                    break;
                case 1:
                    if (!playerReady1)
                    {
                        GameObject.Find("Player1JoinText").SetActive(false);
                        player1Ready.SetActive(true);
                        player1Ready2.SetActive(true);
                        playerCount++;
                        playerReady1 = true;
                        beepBeep.Play();
                    }
                    break;
                case 2:
                    if (!playerReady2)
                    {
                        GameObject.Find("Player2JoinText").SetActive(false);
                        player2Ready.SetActive(true);
                        player2Ready2.SetActive(true);
                        playerCount++;
                        playerReady2 = true;
                        beepBeep.Play();
                    }

                    break;
                case 3:
                    if (!playerReady3)
                    {
                        GameObject.Find("Player3JoinText").SetActive(false);
                        player3Ready.SetActive(true);
                        player3Ready2.SetActive(true);
                        playerCount++;
                        playerReady3 = true;
                        beepBeep.Play();
                    }

                    break;
                case 4:
                    if (!playerReady4)
                    {
                        GameObject.Find("Player4JoinText").SetActive(false);
                        player4Ready.SetActive(true);
                        player4Ready2.SetActive(true);
                        playerCount++;
                        playerReady4 = true;
                        beepBeep.Play();
                    }

                    break;
                    default:
                        break;     
            }
        }
    }
}
