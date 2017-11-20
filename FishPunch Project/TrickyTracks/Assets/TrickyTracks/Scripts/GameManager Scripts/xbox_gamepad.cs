using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XInputDotNetPure;
using XboxCtrlrInput;

//Written by Angus Secomb
//Last edited 27/10/17

//Stores state of a single gamepad button.
public struct xButton
{
    public ButtonState prev_state;
    public ButtonState state;
}

//Stores state of a single gamepad trigger.
public struct TriggerState
{
    public float prev_value;
    public float current_value;
}

//Rumble (vibration) event
class xRumble
{
    public float timer; // Rumbe timer
    public float fadeTime; //Fade-out (in seconds)
    public Vector2 power; //Intensity of rumble.

    public void Update()
    {
        this.timer -= Time.deltaTime;
    }
}

public class xbox_gamepad : MonoBehaviour {

    private GamePadState prev_state; // previous game pad state.
    private GamePadState state;

    private int gamepadIndex; // Numeric Index (1,2,3 or 4)
    private PlayerIndex playerIndex; //Xinput player index
    private List<xRumble> rumbleEvents; //Stores rumble events.
    XboxController controller;
    //Button input map (explained soon!)
    private Dictionary<string, xButton> inputMap;
  
    //States for all buttons/inputs supported
    private xButton A, B, X, Y; // Action (face) buttons
    private xButton Dpad_Up, Dpad_Down, Dpad_Left, Dpad_Right;

    private xButton Guide; // Xbox logo button
    private xButton Back, Start;
    private xButton L3, R3; // Thumbstick buttons.
    private xButton LB, RB; // 'Bumper' (shoulder) buttons.
    private TriggerState LT, RT; // Triggers

    [HideInInspector]
    public float triggerRotation = 1;

    public int newControllerIndex = 1;

    [HideInInspector]
    public bool isAssigned = false;

    //Constructor
    public xbox_gamepad(int index)
    {
        //set gamepad index.
        gamepadIndex = index - 1;
        playerIndex = (PlayerIndex)gamepadIndex;
 
        //Create rumble container and input map
        rumbleEvents = new List<xRumble>();
        inputMap = new Dictionary<string, xButton>();
    }
   
	
	// Update is called once per frame
	public void Update () {
        
        //Get current state
        state = GamePad.GetState(playerIndex);

        //Check gamepad is connected
        if(state.IsConnected)
        {
            A.state = state.Buttons.A;
            B.state = state.Buttons.B;
            X.state = state.Buttons.X;
            Y.state = state.Buttons.Y;

            Dpad_Up.state = state.DPad.Up;
            Dpad_Down.state = state.DPad.Down;
            Dpad_Left.state = state.DPad.Left;
            Dpad_Right.state = state.DPad.Right;

            Guide.state = state.Buttons.Guide;
            Back.state = state.Buttons.Back;
            Start.state = state.Buttons.Start;
            L3.state = state.Buttons.LeftStick;
            R3.state = state.Buttons.RightStick;
            LB.state = state.Buttons.LeftShoulder;
            RB.state = state.Buttons.RightShoulder;

            //Read trigger values into trigger states.
            LT.current_value = state.Triggers.Left;
            RT.current_value = state.Triggers.Right;

            UpdateInputMap();
            HandleRumble();
        }
	}
    
    public void Refresh()
    {
        //this saves the current state for next update
        prev_state = state;

        //Check gamepad is connected
        if (state.IsConnected)
        {
            A.prev_state = prev_state.Buttons.A;
            B.prev_state = prev_state.Buttons.B;
            X.prev_state = prev_state.Buttons.X;
            Y.prev_state = prev_state.Buttons.Y;

            Dpad_Up.prev_state = prev_state.DPad.Up;
            Dpad_Down.prev_state = prev_state.DPad.Down;
            Dpad_Left.prev_state = prev_state.DPad.Left;
            Dpad_Right.prev_state = prev_state.DPad.Right;

            Guide.prev_state = prev_state.Buttons.Guide;
            Back.prev_state = prev_state.Buttons.Back;
            Start.prev_state = prev_state.Buttons.Start;
            L3.prev_state = prev_state.Buttons.LeftStick;
            R3.prev_state = prev_state.Buttons.RightStick;
            LB.prev_state = prev_state.Buttons.LeftShoulder;
            RB.prev_state = prev_state.Buttons.RightShoulder;

            //Read trigger values into trigger states.
            LT.prev_value = prev_state.Triggers.Left;
            RT.prev_value = prev_state.Triggers.Right;

            UpdateInputMap(); // Update inputMap dictionary..
        }
    }

    //Return numeric gamepad index
    public int Index { get { return gamepadIndex; } }

    //Return gamepad connection state
    public bool IsConnected {  get { return state.IsConnected; } }

    void UpdateInputMap()
    {
        inputMap["A"] = A;
        inputMap["B"] = B;
        inputMap["X"] = X;
        inputMap["Y"] = Y;


        inputMap["Dpad_Up"] = Dpad_Up;
        inputMap["Dpad_Down"] = Dpad_Down;
        inputMap["Dpad_Left"] = Dpad_Left;
        inputMap["Dpad_Right"] = Dpad_Right;

        inputMap["Guide"] = Guide;
        inputMap["Back"] = Back;
        inputMap["Start"] = Start;

        //Thumbstick buttons
        inputMap["L3"] = L3;
        inputMap["R3"] = R3;

        //Shoulder buttons
        inputMap["LB"] = LB;
        inputMap["RB"] = RB;

    }

    public bool GetButton(string button)
    {
        return inputMap[button].state == ButtonState.Pressed ? true : false;
    }

    public bool GetButtonDown(string button)
    {
        return (inputMap[button].prev_state == ButtonState.Released &&
                inputMap[button].state == ButtonState.Pressed) ? true : false;
    }

    public bool GetButtonUp(string button)
    {
        return (inputMap[button].prev_state == ButtonState.Pressed &&
                inputMap[button].state == ButtonState.Released) ? true : false;
    }
    //Update and handle rumble events
    void HandleRumble()
    {
        //Ignore if there are no events
        if(rumbleEvents.Count > 0)
        {
            Vector2 currentPower = new Vector2(0, 0);

            for(int i = 0; i < rumbleEvents.Count; ++i)
            {
                rumbleEvents[i].Update();

                if(rumbleEvents[i].timer < 0)
                {
                    //Calculate current power
                    float timeLeft = Mathf.Clamp(rumbleEvents[i].timer / rumbleEvents[i].fadeTime, 0f, 1f);
                    currentPower = new Vector2(Mathf.Max(rumbleEvents[i].power.x * timeLeft, currentPower.x),
                                               Mathf.Max(rumbleEvents[i].power.y * timeLeft, currentPower.y));

                    GamePad.SetVibration(playerIndex, currentPower.x, currentPower.y);
                }
                else
                {
                    //Remove event
                    rumbleEvents.Remove(rumbleEvents[i]);
                }
            }
        }
    }

    //Add rumble event.
    public void AddRumble(float timer, Vector2 power, float fadeTime)
    {
        xRumble rumble = new xRumble();

        rumble.timer = timer;
        rumble.power = power;
        rumble.fadeTime = fadeTime;
        rumbleEvents.Add(rumble);
    }

    //return axes of left stick
    public GamePadThumbSticks.StickValue GetStick_L()
    {
        return state.ThumbSticks.Left;
    }

    //return axes right stick
    public GamePadThumbSticks.StickValue GetStick_R()
    {
        return state.ThumbSticks.Right;
    }

    //return axis of the left trigger
    public float GetTrigger_L() { return state.Triggers.Left; }

    //right trigger
    public float GetTrigger_R() { return state.Triggers.Right; }

    public bool GetTriggerDown_R()
    {
        return (RT.current_value >= 0.1) ? true : false;
    }

    public bool GetTriggerDown_L()
    {
        return (LT.current_value >= 0.1) ? true : false;
    }

    //checks if left trigger was tapped on current frame.
    public bool GetTriggerTap_L()
    {
        return (LT.prev_value == 0f && LT.current_value >= 0.1f) ? true : false;
    }

    //checks if right trigger was tapped on a current frame.
    public bool GetTriggerTap_R()
    {
        return (RT.prev_value == 0f && RT.current_value >= 0.1f) ? true : false;
    }
}
