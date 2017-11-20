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
        //Assign instances of manager components
        manager = GameObject.Find("Manager");
        gpManager = manager.GetComponent<GamePadManager>();
        playB = GameObject.Find("Play").GetComponent<Image>();
        exitB = GameObject.Find("Exit").GetComponent<Image>();
        //grey out selectable buttons.
        playB.color = Color.grey;
        exitB.color = Color.grey;
        gamepads = new List<xbox_gamepad>();

        //Assign gamepads.
        for (int i = 1; i <= GamePadManager.Instance.GamePadCount; ++i)
        {
            gamepads.Add(GamePadManager.Instance.GetGamePad(i));
        }


    }

    // Update is called once per frame
    void Update()
    {

        //Activate kart on platform based on playercount.
        switch (playerCount)
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

        //Changes colours of buttons based on buttonindex.
        if (playerCount > 1)
        {

            if (buttonIndex == 1)
            {
                playB.color = Color.yellow;
                exitB.color = Color.grey;
            }
            if (buttonIndex == 2)
            {
                exitB.color = Color.yellow;
                playB.color = Color.grey;
            }

        }

        //cooldown for switching between buttons.
        coolDown -= Time.deltaTime;

        //Checks all gamepads for button press then loads approriate map
        //based on buttonIndex.
        for (int i = 0; i < playerCount; ++i)
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
     
        //if player one moves up and down change the button index
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

        //Assign controllers to karts.
        int n = GamePadManager.Instance.ConnectedTotal();
        if (n < 5)
        {
            for (int i = 1; i <= n; ++i)
            {

                assignControllerToKart(i);
            }
        }
     
      
    }

    void assignControllerToKart(int index)
    {
        //Create gamepad instance.
        gamepad = GamePadManager.Instance.GetGamePad(index);


        //If start is pressed
        if (gamepad.GetButtonDown("Start"))
        {
            //and gamepad has not been assigned
            if (!gamepad.isAssigned)
            {
                //Add +1 to player count
                playerCount++;
                gamepad.isAssigned = true;

                //Assign controller index based on playercount
                //and activate/deactivate the appropriate UI.
                switch (playerCount)
                {
                    case 1:
                 //       gamepad = GamePadManager.Instance.GetGamePad(1);
                        GameObject.Find("Player1JoinText").SetActive(false);
                        player1Ready.SetActive(true);
                        player1Ready2.SetActive(true);
                        gamepad.newControllerIndex = 1;
                        beepBeep.Play();
                        break;
                    case 2:
                  //      gamepad = GamePadManager.Instance.GetGamePad(2);
                        GameObject.Find("Player2JoinText").SetActive(false);
                        player2Ready.SetActive(true);
                        player2Ready2.SetActive(true);
                        gamepad.newControllerIndex = 2;
                        beepBeep.Play();
                        break;
                    case 3:
                    //    gamepad = GamePadManager.Instance.GetGamePad(3);
                        GameObject.Find("Player3JoinText").SetActive(false);
                        player3Ready.SetActive(true);
                        player3Ready2.SetActive(true);
                        gamepad.newControllerIndex = 3;
                        beepBeep.Play();
                        break;
                    case 4:
                   //     gamepad = GamePadManager.Instance.GetGamePad(4);
                        GameObject.Find("Player4JoinText").SetActive(false);
                        player4Ready.SetActive(true);
                        player4Ready2.SetActive(true);
                        gamepad.newControllerIndex = 4;
                        beepBeep.Play();
                        break;
                    default:
                        break;
                }
            }
        }
    }
}
