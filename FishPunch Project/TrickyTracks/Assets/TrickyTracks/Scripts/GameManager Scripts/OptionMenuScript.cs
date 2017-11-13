using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using XInputDotNetPure;

public class OptionMenuScript : MonoBehaviour {

    bool playerIndexSet = false;
    PlayerIndex playerIndex;
    GamePadState state;
    GamePadState prevState;

    private GamePadManager gpManager;
    private xbox_gamepad gamepad1;
    private xbox_gamepad gamepad2;
    private xbox_gamepad gamepad3;
    private xbox_gamepad gamepad4;

    // direction control
    float deadZone = 0.9f;
    float coolDown = 0.3f;
    float cdCopy = 0.3f;

    // sliders
    public GameObject MasterSlider, MusicSlider, SfxSlider, BackMenu;
    public GameObject AudioControlsSwitch;

    // controller changes
    public Text controlChange;

    private int controlChoice = 1;

    // text color change
    Image masteri, musici, sfxi, controli, backi;

    Color y = new Color(1, 0.92f, 0.016f, 1);

    // for menu switch
    private int buttonIndex = 1;

    private bool isControllerMenu = false;

    public Slider MasterVolume;
    public Slider MusicVolume;
    public Slider SfxVolume;

    [Range(0, 1)]
    public float MasterValue = 0.5f;
    [Range(0, 1)]
    public float MusicValue = 0.5f;
    [Range(0, 1)]
    public float SFXValue = 0.5f;

    public GameObject AudioMenu, ControllerMenu;

    public AudioSource[] sounds;
    [HideInInspector]
    public AudioSource music;
    
    public AudioSource[] SFX;
    [HideInInspector]
    public AudioSource SFX1;

    public void SetMasterVolume(float value)
    {
        AudioListener.volume = MasterVolume.value;

        Debug.Log(MasterVolume.value);
    }
    public void setMusicVolume(float value)
    {
        
        music.volume = MusicVolume.value;

        Debug.Log(MusicVolume.value);
    }

    public void setSfxVolume(float value)
    {
        
        SFX1.volume = SfxVolume.value;

        Debug.Log(SfxVolume.value);
    }

    public void controlsLeft()
    {
        controlChoice -= 1;


        if (controlChoice <= 0)
            controlChoice = 3;
    }
    public void controlsRight()
    {
        controlChoice += 1;


        if (controlChoice >= 4)
            controlChoice = 1;
    }

    public void displayToggle()
    {
        if (isControllerMenu == true)
        {
            AudioMenu.SetActive(true);

            ControllerMenu.SetActive(false);

            AudioControlsSwitch.GetComponent<Text>().text = "" + "Controls";
            isControllerMenu = false;
        }
        else
        {
            AudioMenu.SetActive(false);

            ControllerMenu.SetActive(true);

            AudioControlsSwitch.GetComponent<Text>().text = "" + "Audio";
            isControllerMenu = true;
        }
    }

    public void back(string loadlevel)
    {
        SceneManager.LoadScene(loadlevel);
    }



	// Use this for initialization
	void Start () {
        gpManager = this.gameObject.GetComponent<GamePadManager>();
        gamepad1 = GamePadManager.Instance.GetGamePad(1);
        gamepad2 = GamePadManager.Instance.GetGamePad(2);
        gamepad3 = GamePadManager.Instance.GetGamePad(3);
        gamepad4 = GamePadManager.Instance.GetGamePad(4);

        //masteri = MasterSlider.GetComponent<Image>();
        //musici = MusicSlider.GetComponent<Image>();
        //sfxi = SfxSlider.GetComponent<Image>();
        //
        //controli = AudioControlsSwitch.GetComponent<Image>();
        //backi = BackMenu.GetComponent<Image>();
        controlChoice = 1;



        sounds = GetComponents<AudioSource>();
        music = sounds[0];
        SFX1 = sounds[1];
        SFX[0] = SFX1;

        controlChange.text = "test";

        
    }


    // Update is called once per frame
    void Update()
    {

        MasterValue = MasterVolume.value;
        MusicValue = MusicVolume.value;
        SFXValue = SfxVolume.value;

        SetMasterVolume(MasterValue);
        setMusicVolume(MusicValue);
        setSfxVolume(SFXValue);

        if (!playerIndexSet || !prevState.IsConnected)
        {
            for (int i = 0; i < 4; ++i)
            {
                PlayerIndex testPlayerIndex = (PlayerIndex)i;
                GamePadState testState = GamePad.GetState(testPlayerIndex);
                if (testState.IsConnected)
                {
                    Debug.Log(string.Format("GamePad found {0}", testPlayerIndex));
                    playerIndex = testPlayerIndex;
                    playerIndexSet = true;
                }
            }
        }


        switch (buttonIndex)
        {
            case 1:
                //masteri.color = y;
                //musici.color = Color.white;
                //sfxi.color = Color.white;
                //controli.color = Color.white;
                //backi.color = Color.white;

                if (state.ThumbSticks.Left.Y < deadZone && coolDown < 0)
                {
                    buttonIndex = 5;
                    coolDown = cdCopy;
                }

                if (state.ThumbSticks.Left.Y < -deadZone && coolDown < 0)
                {
                    buttonIndex = 2;
                    coolDown = cdCopy;
                }


                if (state.ThumbSticks.Left.X > deadZone) // changing the mastervolume slider in the scene
                {
                    MasterVolume.value += 0.01f;
                }

                if (state.ThumbSticks.Left.X < -deadZone)
                {
                    MasterVolume.value -= 0.01f;
                }

                break;

            case 2:

                //masteri.color = Color.white;
                //musici.color = y;
                //sfxi.color = Color.white;
                //controli.color = Color.white;
                //backi.color = Color.white;

                if (state.ThumbSticks.Left.Y > deadZone && coolDown < 0)
                {
                    buttonIndex = 1;
                    coolDown = cdCopy;
                }

                if (state.ThumbSticks.Left.Y < -deadZone && coolDown < 0)
                {
                    buttonIndex = 3;
                    coolDown = cdCopy;
                }

                if (state.ThumbSticks.Left.X > deadZone) // changing the musicvolume slider in the scene
                {
                    MusicVolume.value += 0.01f;
                }

                if (state.ThumbSticks.Left.X < -deadZone)
                {
                    MusicVolume.value -= 0.01f;
                }
                break;

            case 3:
                //masteri.color = y;
                //musici.color = Color.white;
                //sfxi.color = Color.white;
                //controli.color = Color.white;
                //backi.color = Color.white;

                if (state.ThumbSticks.Left.Y > deadZone && coolDown < 0)
                {
                    buttonIndex = 2;
                    coolDown = cdCopy;
                }

                if (state.ThumbSticks.Left.Y < -deadZone && coolDown < 0)
                {
                    buttonIndex = 4;
                    coolDown = cdCopy;
                }

                if (state.ThumbSticks.Left.X > deadZone) //changing the sound effects volume slider in the scene
                {
                    SfxVolume.value += 0.01f;
                }

                if (state.ThumbSticks.Left.X < -deadZone)
                {
                    SfxVolume.value -= 0.01f;
                }
                break;

            case 4: // for the controls to audio switch
                //masteri.color = y;
                //musici.color = Color.white;
                //sfxi.color = Color.white;
                //controli.color = Color.white;
                //backi.color = Color.white;

                if (state.ThumbSticks.Left.Y > deadZone && coolDown < 0)
                {
                    buttonIndex = 3;
                    coolDown = cdCopy;
                }

                if (state.ThumbSticks.Left.Y < -deadZone && coolDown < 0)
                {
                    buttonIndex = 5;
                    coolDown = cdCopy;
                }





                break;
            case 5: // goes to main menu
                //masteri.color = y;
                //musici.color = Color.white;
                //sfxi.color = Color.white;
                //controli.color = Color.white;
                //backi.color = Color.white;

                if (state.ThumbSticks.Left.Y > deadZone && coolDown < 0)
                {
                    buttonIndex = 4;
                    coolDown = cdCopy;
                }

                if (state.ThumbSticks.Left.Y < -deadZone && coolDown < 0)
                {
                    buttonIndex = 1;
                    coolDown = cdCopy;
                }

                if (state.Buttons.A == ButtonState.Pressed)
                {

                }

                break;

            default:
                break;
        }


        if (isControllerMenu == true)
        {
            if (state.Buttons.LeftShoulder == ButtonState.Pressed)
            {
                controlsLeft(); // changing the controls layout in the menu
            }

            if (state.Buttons.RightShoulder == ButtonState.Pressed)
            {
                controlsRight(); // changing the controls layout in the menu
            }
        }

        //if (gamepad1 == true)
        //{
        switch (controlChoice)
        {
            case 1:
                controlChange.text = "" + "Default " + controlChoice.ToString();

                Debug.Log(controlChoice);

                break;

            case 2:
                controlChange.text = "" + "Default " + controlChoice.ToString();

                Debug.Log(controlChoice);
                break;

            case 3:
                controlChange.text = "" + "Default " + controlChoice.ToString();

                Debug.Log(controlChoice);
                break;

            default:
                break;
        }

    }
}
