using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuActor : MonoBehaviour {

    private GamePadManager gpManager;
    private xbox_gamepad gamepad1;

    float deadZone = 0.9f;

    float coolDown = 0.3f;
    float cdCopy = 0.3f;

    public GameObject playButton, optionButton, exitButton;

    Image playi, optioni, exiti;
    
    Color y = new Color(1, 0.92f, 0.016f, 1);

    private int buttonIndex = 1;


    public void LoadLevel(string loadlevel)
    {
        SceneManager.LoadScene(loadlevel);
    }

    public void LoadLevel(int levelIndex)
    {
        SceneManager.LoadScene(levelIndex);
    }

    public void CloseApplication()
    {
        Application.Quit();
    }

	// Use this for initialization
	void Start () {
        gpManager = this.gameObject.GetComponent<GamePadManager>();
        gamepad1 = GamePadManager.Instance.GetGamePad(1);
        //playButton = GameObject.Find("PlayBTN");
        //optionButton = GameObject.Find("OptionsBTN");
        //exitButton = GameObject.Find("ExitBTN");
        playi = playButton.GetComponent<Image>();
        optioni = optionButton.GetComponent<Image>();
        exiti = exitButton.GetComponent<Image>();
    }

    // Update is called once per frame
    void Update () {

        switch(buttonIndex)
        {
            case 1:
                playi.color = y;
                optioni.color = Color.white;
                exiti.color = Color.white;
                coolDown -= Time.deltaTime;
                if (gamepad1.GetStick_L().Y > deadZone && coolDown < 0)
                {
                    buttonIndex = 3;
                    coolDown = cdCopy;
                }
                if(gamepad1.GetStick_L().Y < -deadZone && coolDown < 0)
                {
                    buttonIndex = 2;
                    coolDown = cdCopy;
                }

                if(gamepad1.GetButtonDown("A"))
                {
                    LoadLevel(1);
                    GameObject.Destroy(this.gameObject);
                }
                break;
            case 2:
                coolDown -= Time.deltaTime;
                optioni.color = y;
                playi.color = Color.white;
                exiti.color = Color.white;

                if (gamepad1.GetStick_L().Y > deadZone && coolDown < 0)
                {
                    buttonIndex = 1;
                    coolDown = cdCopy;
                }
                if (gamepad1.GetStick_L().Y < -deadZone && coolDown < 0)
                {
                    buttonIndex = 3;
                    coolDown = cdCopy;
                }

                if (gamepad1.GetButtonDown("A"))
                {
                    LoadLevel(3);
                }
                break;
            case 3:
                exiti.color = y;
                playi.color = Color.white;
                optioni.color = Color.white;
                coolDown -= Time.deltaTime;
                if (gamepad1.GetStick_L().Y > deadZone && coolDown < 0)
                {
                    coolDown = cdCopy;
                    buttonIndex = 2;
                }
                if (gamepad1.GetStick_L().Y < -deadZone && coolDown < 0)
                {
                    buttonIndex = 1;
                    coolDown = cdCopy;
                }

                if (gamepad1.GetButtonDown("A"))
                {
                    CloseApplication();
                }
                break;
            default:
                break;
        }
	}
}
