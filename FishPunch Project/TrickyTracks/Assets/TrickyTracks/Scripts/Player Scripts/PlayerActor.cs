using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Actor uses altered kart controller
//by deercat from the asset store.

[RequireComponent(typeof(KartController))]
public class PlayerActor : MonoBehaviour {

    KartController kart;

    public int playerNumber = 1;
    public float breakPower = 20.0f;
    public float driftTraction = 0f;
    public float driftDeceleration = 0.05f;

    private xbox_gamepad gamepad;

	// Use this for initialization
	void Start () {
        kart = GetComponent<KartController>();
        
	}

    // Update is called once per frame
    void Update() {

        switch (playerNumber)
        {
            case 1:
                gamepad = GamePadManager.Instance.GetGamePad(1);
                break;
            case 2:
                gamepad = GamePadManager.Instance.GetGamePad(2);
                break;
            case 3:
                gamepad = GamePadManager.Instance.GetGamePad(3);
                break;
            case 4:
                gamepad = GamePadManager.Instance.GetGamePad(4);
                break;
            default:
                break;

        }
              Debug.Log(kart.physicsBody.velocity.sqrMagnitude);
        if (gamepad.GetTriggerDown_R())
        {
            kart.Thrust = gamepad.GetTrigger_R();
        }
        else if(!gamepad.GetTriggerDown_L())
        {
            kart.Thrust = 0;
        }


        if (gamepad.GetTriggerDown_L())
        {
            kart.Thrust = -gamepad.GetTrigger_L();
        }
        else if(!gamepad.GetTriggerDown_R())
        {
            kart.Thrust = 0.0f;
        }
        kart.Steering = gamepad.GetStick_L().X;

        if (gamepad.GetButton("B"))
        {
            if (kart.Steering != 0 && kart.physicsBody.velocity.sqrMagnitude > 50.0f)
            {
                kart.traction = driftTraction;

                kart.decelerationSpeed = driftDeceleration;
                kart.breakPower = breakPower;
            }
            
        }
        else
        {
            kart.decelerationSpeed = kart.decelerationSpeedCopy;
            kart.breakPower = 0.0f;
            kart.traction = 0.4f;
        }
            
        //}

	}
}
