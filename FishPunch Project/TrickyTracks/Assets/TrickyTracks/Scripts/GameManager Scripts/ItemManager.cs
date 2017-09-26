using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour {

    //Kart Scripts
    [HideInInspector]
    public KartActor2 kart1, kart2, kart3, kart4;

    [HideInInspector]
    public KartActor2 temporaryKart;



    private GameObject go_kart1;
    private GameObject go_kart2;
    private GameObject go_kart3;
    private GameObject go_kart4;

    public GameObject Rocket;
    [HideInInspector]
    public bool rocketFired = false;

    private string item;

	// Use this for initialization
	void Start () {
        //Find game objects.
        go_kart1 = GameObject.Find("PlayerCharacter_001");
        go_kart2 = GameObject.Find("PlayerCharacter_002");
        go_kart3 = GameObject.Find("PlayerCharacter_003");
        go_kart4 = GameObject.Find("PlayerCharacter_004");
        //Find script components.
        kart1 = go_kart1.GetComponent<KartActor2>();
        if (go_kart2 != null)
        {
            kart2 = go_kart2.GetComponent<KartActor2>();
        }
        if (go_kart3 != null)
        {
            kart3 = go_kart3.GetComponent<KartActor2>();
        }
        if (go_kart4 != null)
        {
            kart4 = go_kart4.GetComponent<KartActor2>();
        }
        temporaryKart = kart1;

    }
	
	// Update is called once per frame
	void Update () {


            if (kart1.itemBoost)
            {
                if (kart1.gamepad.GetButtonDown("X"))
                {
                    Debug.Log("boost");
                    kart1.boostPlayer = true;
                }
            }
        


        if(kart1.itemMine)
        {
            kart1.playerDisabled = true;
        }

        if(kart1.itemRPG)
        {
            if(kart1.gamepad.GetButtonDown("X"))
            {
                
                temporaryKart = kart1;
                rocketFired = true;
                Instantiate(Rocket, go_kart1.transform.position + (go_kart1.transform.forward * 4), (go_kart1.transform.rotation));
                kart1.itemRPG = false;
            }
        }
		
	}
}
