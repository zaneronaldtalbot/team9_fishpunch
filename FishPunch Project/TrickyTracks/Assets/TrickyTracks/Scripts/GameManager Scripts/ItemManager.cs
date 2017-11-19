
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Written By Angus Secomb
//Last editede 19/11/2017
public class ItemManager : MonoBehaviour {

    //Kart Script
    [HideInInspector]
    public PlayerActor wc_kart1, wc_kart2, wc_kart3, wc_kart4;

    //rocketactor.
    private RocketActor rocketActor;

    //Gameobjects.
    private GameObject go_kart1;
    private GameObject go_kart2;
    private GameObject go_kart3;
    private GameObject go_kart4;

    //Rocket mine gameobjects.
    public GameObject Rocket;
    public GameObject Mine;

    private GameObject tempRocket;
    [HideInInspector]
    public bool rocketFired = false;
    
    //Manager.
    private GameObject manager;
    private GamePadManager gpmanager;

    //PsActor.
    private PlayerSelectActor psActor;

    // Use this for initialization
    void Start() {
        //Grab an instance of the manager.
        manager = this.gameObject;
        gpmanager = manager.GetComponent<GamePadManager>();
        rocketActor = Rocket.GetComponentInChildren<RocketActor>();
        psActor = manager.GetComponent<PlayerSelectActor>();

    }
    float timer1 = 0.0f;

    // Update is called once per frame
    void Update() {
        timer1 += Time.deltaTime;
       
        //Grabs copies of the players kart gameobject and script
        //based on the connected controller total.
        switch(psActor.playerCount)
        {
            case 2:
                if (go_kart1 == null)
                {
                    go_kart1 = GameObject.Find("PlayerCharacter_001");
                    wc_kart1 = go_kart1.GetComponent<PlayerActor>();
                }
                if (go_kart2 == null)
                {
                    go_kart2 = GameObject.Find("PlayerCharacter_002");
                    wc_kart2 = go_kart2.GetComponent<PlayerActor>();
                }
                break;
            case 3:
                if (go_kart1 == null)
                {
                    go_kart1 = GameObject.Find("PlayerCharacter_001");
                    wc_kart1 = go_kart1.GetComponent<PlayerActor>();
                }
                if (go_kart2 == null)
                {
                    go_kart2 = GameObject.Find("PlayerCharacter_002");
                    wc_kart2 = go_kart2.GetComponent<PlayerActor>();
                }
                if (go_kart3 == null)
                {
                    go_kart3 = GameObject.Find("PlayerCharacter_003");
                    wc_kart3 = go_kart3.GetComponent<PlayerActor>();
                }
                break;
            case 4:
                if (go_kart1 == null)
                {
                    go_kart1 = GameObject.Find("PlayerCharacter_001");
                    wc_kart1 = go_kart1.GetComponent<PlayerActor>();
                }
                if (go_kart2 == null)
                {
                    go_kart2 = GameObject.Find("PlayerCharacter_002");
                    wc_kart2 = go_kart2.GetComponent<PlayerActor>();
                }
                if (go_kart3 == null)
                {
                    go_kart3 = GameObject.Find("PlayerCharacter_003");
                    wc_kart3 = go_kart3.GetComponent<PlayerActor>();
                }
                if (go_kart4 == null)
                {
                    go_kart4 = GameObject.Find("PlayerCharacter_004");
                    wc_kart4 = go_kart4.GetComponent<PlayerActor>();
                }
                break;
            default:
                break;
        }

        //Checks the item conditions for all the karts.
        if (go_kart1 != null)
        {
            kartItemChecks(wc_kart1, go_kart1);
        }
        if (go_kart2 != null)
        {
            kartItemChecks(wc_kart2, go_kart2);
        }
        if (go_kart3 != null)
        {
            kartItemChecks(wc_kart3, go_kart3);
        }
        if (go_kart4 != null)
        {
            kartItemChecks(wc_kart4, go_kart4);
        }
    }

    void kartItemChecks(PlayerActor wc_kart, GameObject go_kart)
    {
        {
            //If the kart has the item boost and they press
            // "X" boost the player and set get rid of the boost item.
            if (wc_kart.itemBoost)
            {

                    wc_kart.boostPlayer = true;
                    wc_kart.itemBoost = false;
            }
        }

        //If kart grabs item mine and press x
        //Instantiate mine behind the karts position
        //and set itemmine to false.
        if (wc_kart.itemMine)
        {
                Instantiate(Mine, go_kart.transform.position + (-go_kart.transform.forward * 3), go_kart.transform.rotation);
                wc_kart.itemMine = false;
        }

        //If kart grabs item rpg and presses X
        //fire rocket and set item RPG to false.
        if (wc_kart.itemRPG)
        {
                rocketFired = true;
                tempRocket = Instantiate(Rocket, go_kart.transform.position + (go_kart.transform.forward * 5), (go_kart.transform.rotation)) as GameObject;
                wc_kart.itemRPG = false;
        }

        //Fire temprocket.
        if (rocketFired && tempRocket != null)
        {
            tempRocket.transform.Translate(Rocket.transform.forward * rocketActor.rocketSpeed * Time.deltaTime);
        }
    }
}
