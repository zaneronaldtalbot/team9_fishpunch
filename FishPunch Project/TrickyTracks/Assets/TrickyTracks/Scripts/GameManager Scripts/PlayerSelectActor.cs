using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class PlayerSelectActor : MonoBehaviour
{
    private GamePadManager gpManager;
    private GameObject manager;

    [HideInInspector]
    public int playerCount = 0;

    public GameObject player1Ready, player2Ready, player3Ready, player4Ready;
    public GameObject player1Ready2, player2Ready2, player3Ready2, player4Ready2;

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

    [Tooltip("Kart prefab Instances.")]
    public GameObject kartOne, kartTwo, kartThree, kartFour;

    Image playB, exitB;

    public float timer = 60.0f;

    private Text textTimer;

    xbox_gamepad gamepad;

    // Use this for initialization
    void Start()
    {
        manager = GameObject.Find("Manager");
        gpManager = manager.GetComponent<GamePadManager>();
        playB = GameObject.Find("Play").GetComponent<Image>();
        exitB = GameObject.Find("Exit").GetComponent<Image>();
        playB.color = Color.grey;
        exitB.color = Color.grey;

    }

    // Update is called once per frame
    void Update()
    {
        if (timer < 0.0f)
        {
            SceneManager.LoadScene("Peter's Map");
        }


        if(playerCount > 1)
        {
            playB.color = Color.yellow;
            play
        }

        switch (gpManager.ConnectedTotal())
        {
            case 2:
                if (playerReady1 && playerReady2)
                {
                    SceneManager.LoadScene("Peter's Map");
                }
                break;
            case 3:
                if (playerReady1 && playerReady2 && playerReady3)
                {
                    SceneManager.LoadScene("Peter's Map");
                }
                break;
            case 4:
                if (playerReady1 && playerReady2 && playerReady3 && playerReady4)
                {
                    SceneManager.LoadScene("Peter's Map");
                }
                break;
            default:
                break;
        }

        timer -= Time.deltaTime;

         textTimer =  go_timer.GetComponent<Text>();

        int intTimer = (int)timer;
        textTimer.text = "Time Left: " + intTimer.ToString();

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
