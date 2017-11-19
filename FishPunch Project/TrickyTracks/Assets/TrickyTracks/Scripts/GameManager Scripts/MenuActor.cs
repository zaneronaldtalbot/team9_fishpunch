using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
//Written by Angus Secomb
//Last edited on 17/11/17
public class MenuActor : MonoBehaviour {

    private GamePadManager gpManager;
    private xbox_gamepad gamepad1;

    private AudioSource gearShiftOne, gearShiftTwo, beepBeep;

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
        gearShiftOne = GameObject.Find("GearShiftOne").GetComponent<AudioSource>();
        gearShiftTwo = GameObject.Find("GearShiftTwo").GetComponent<AudioSource>();
        beepBeep = GameObject.Find("BeepBeep").GetComponent<AudioSource>();
        //playButton = GameObject.Find("PlayBTN");
        //optionButton = GameObject.Find("OptionsBTN");
        //exitButton = GameObject.Find("ExitBTN");
        playi = playButton.GetComponent<Image>();
        optioni = optionButton.GetComponent<Image>();
        exiti = exitButton.GetComponent<Image>();
    }

    // Update is called once per frame
    void Update () {
        Debug.Log(buttonIndex);
        switch(buttonIndex)
        {
            case 1:
                playi.color = y;
                optioni.color = Color.grey ;
                exiti.color = Color.grey;
                coolDown -= Time.deltaTime;
                if (gamepad1.GetStick_L().Y > deadZone && coolDown < 0)
                {
                    gearShiftOne.Play();
                    buttonIndex = 3;
                    coolDown = cdCopy;
                }
                if(gamepad1.GetStick_L().Y < -deadZone && coolDown < 0)
                {
                    gearShiftTwo.Play();
                    buttonIndex = 2;
                    coolDown = cdCopy;
                }

                if(gamepad1.GetButtonDown("Start"))
                {
                    beepBeep.Play();
                    LoadLevel(1);
                    GameObject.Destroy(this.gameObject);
                }

                if(gamepad1.GetButtonDown("A"))
                {
                    beepBeep.Play();
                    LoadLevel(1);
                    GameObject.Destroy(this.gameObject);
                }
                break;
            case 2:
                coolDown -= Time.deltaTime;
                optioni.color = y;
                playi.color = Color.grey;
                exiti.color = Color.grey;

                if (gamepad1.GetStick_L().Y > deadZone && coolDown < 0)
                {
                    gearShiftOne.Play();
                    buttonIndex = 1;
                    coolDown = cdCopy;
                }
                if (gamepad1.GetStick_L().Y < -deadZone && coolDown < 0)
                {
                    gearShiftTwo.Play();
                    buttonIndex = 3;
                    coolDown = cdCopy;
                }

                if (gamepad1.GetButtonDown("Start"))
                {
                    beepBeep.Play();
                    LoadLevel(3);
                }

                if (gamepad1.GetButtonDown("A"))
                {
                    beepBeep.Play();
                    LoadLevel(3);
                }
                break;
            case 3:
                exiti.color = y;
                playi.color = Color.grey;
                optioni.color = Color.grey;
                coolDown -= Time.deltaTime;
                if (gamepad1.GetStick_L().Y > deadZone && coolDown < 0)
                {
                    gearShiftOne.Play();
                    coolDown = cdCopy;
                    buttonIndex = 2;
                }
                if (gamepad1.GetStick_L().Y < -deadZone && coolDown < 0)
                {
                    gearShiftTwo.Play();
                    buttonIndex = 1;
                    coolDown = cdCopy;
                }

                if (gamepad1.GetButtonDown("A"))
                {
                    beepBeep.Play();
                    CloseApplication();
                }
                break;
            default:
                break;
        }
	}
}
