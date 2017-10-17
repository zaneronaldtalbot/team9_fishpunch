using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour {

    //Kart Scripts
    [HideInInspector]
    public KartActor2 kart1, kart2, kart3, kart4;

    private RocketActor rocketActor;

    private GameObject go_kart1;
    private GameObject go_kart2;
    private GameObject go_kart3;
    private GameObject go_kart4;

    public GameObject Rocket;
    public GameObject Mine;

    private GameObject tempRocket;
    [HideInInspector]
    public bool rocketFired = false;

    private GameObject manager;
    private GamePadManager gpmanager;

    // Use this for initialization
    void Start() {
        //Grab an instance of the manager.
        manager = this.gameObject;
        gpmanager = manager.GetComponent<GamePadManager>();
        rocketActor = Rocket.GetComponentInChildren<RocketActor>();

    }
    float timer1 = 0.0f;

    // Update is called once per frame
    void Update() {
        timer1 += Time.deltaTime;

        

        //Grabs copies of the players kart gameobject and script
        //based on the connected controller total.
        switch(gpmanager.ConnectedTotal())
        {
            case 1:
                if (go_kart1 == null)
                {
                    go_kart1 = GameObject.Find("PlayerCharacter_001");
                    kart1 = go_kart1.GetComponent<KartActor2>();
                }
                break;
            case 2:
                if (go_kart1 == null)
                {
                    go_kart1 = GameObject.Find("PlayerCharacter_001");
                    kart1 = go_kart1.GetComponent<KartActor2>();
                }
                if (go_kart2 == null)
                {
                    go_kart2 = GameObject.Find("PlayerCharacter_002");
                    kart2 = go_kart2.GetComponent<KartActor2>();
                }
                break;
            case 3:
                if (go_kart1 == null)
                {
                    go_kart1 = GameObject.Find("PlayerCharacter_001");
                    kart1 = go_kart1.GetComponent<KartActor2>();
                }
                if (go_kart2 == null)
                {
                    go_kart2 = GameObject.Find("PlayerCharacter_002");
                    kart2 = go_kart2.GetComponent<KartActor2>();
                }
                if (go_kart3 == null)
                {
                    go_kart3 = GameObject.Find("PlayerCharacter_003");
                    kart3 = go_kart3.GetComponent<KartActor2>();
                }
                break;
            case 4:
                if (go_kart1 == null)
                {
                    go_kart1 = GameObject.Find("PlayerCharacter_001");
                    kart1 = go_kart1.GetComponent<KartActor2>();
                }
                if (go_kart2 == null)
                {
                    go_kart2 = GameObject.Find("PlayerCharacter_002");
                    kart2 = go_kart2.GetComponent<KartActor2>();
                }
                if (go_kart3 == null)
                {
                    go_kart3 = GameObject.Find("PlayerCharacter_003");
                    kart3 = go_kart3.GetComponent<KartActor2>();
                }
                if (go_kart4 == null)
                {
                    go_kart4 = GameObject.Find("PlayerCharacter_004");
                    kart4 = go_kart4.GetComponent<KartActor2>();
                }
                break;
            default:
                break;
        }







        //Checks the item conditions for all the karts.
        if (go_kart1 != null)
        {
            kartItemChecks(kart1, go_kart1);
        }
        if (go_kart2 != null)
        {
            kartItemChecks(kart2, go_kart2);
        }
        if (go_kart3 != null)
        {
            kartItemChecks(kart3, go_kart3);
        }
        if (go_kart4 != null)
        {
            kartItemChecks(kart4, go_kart4);
        }



    }

    void kartItemChecks(KartActor2 kart, GameObject go_kart)
    {
        {
            //If the kart has the item boost and they press
            // "X" boost the player and set get rid of the boost item.
            if (kart.itemBoost)
            {
                if (kart.gamepad.GetButtonDown("X"))
                {
                    kart.boostPlayer = true;
                    kart.itemBoost = false;
                }
            }
        }

        //If kart grabs item mine and press x
        //Instantiate mine behind the karts position
        //and set itemmine to false.
        if (kart.itemMine)
        {
            if (kart.gamepad.GetButtonDown("X"))
            {
                Instantiate(Mine, go_kart.transform.position + (-go_kart.transform.forward * 3), go_kart.transform.rotation);
                kart.itemMine = false;
            }
        }

        //If kart grabs item rpg and presses X
        //fire rocket and set item RPG to false.
        if (kart.itemRPG)
        {
            if (kart.gamepad.GetButtonDown("X"))
            {
                rocketFired = true;
                tempRocket = Instantiate(Rocket, go_kart.transform.position + (go_kart.transform.forward * 5), (go_kart.transform.rotation)) as GameObject;
                kart.itemRPG = false;
            }
        }

        //Fire temprocket.
        if (rocketFired && tempRocket != null)
        {
            tempRocket.transform.Translate(Rocket.transform.forward * rocketActor.rocketSpeed * Time.deltaTime);
        }
    }
}
