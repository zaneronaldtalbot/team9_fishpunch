using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketActor : MonoBehaviour
{

    //Gameobjects
    private GameObject go_kart;
    private GameObject rocket;

    
    private KartActor2 kart;
    private KartActor2 targetKart;
    private ItemRocket itemRocket;
    private ItemManager itemManager;
    private GameObject manager;
    public float rocketLife = 5.0f;
    public float rocketSpeed = 10.0f;

    private float timer = 0.0f;
    // Use this for initialization
    void Start()
    {
        rocket = this.gameObject.transform.parent.gameObject;
        manager = GameObject.Find("Manager");
        itemManager = manager.GetComponent<ItemManager>();
        kart = itemManager.temporaryKart;
    }

    // Update is called once per frame
    void Update()
    {
            //rocket.transform.position = kart.gameObject.transform.position;
       

     //   if (kart.fireRPG)
     //   {

            timer += Time.deltaTime;
           // rocket.transform.forward = kart.gameObject.transform.parent.transform.forward;
            rocket.transform.Translate(rocket.transform.forward * rocketSpeed * Time.deltaTime);

            if (timer > rocketLife)
            {

                timer = 0.0f;
              //  kart.fireRPG = false;
                Destroy(this.gameObject.transform.parent.gameObject);
            }
    //    }
    }

    //    void OnCollisionEnter(Collision coll)
    //    {
    //        if (coll.gameObject.tag == "Player")
    //        {
    //            go_targetKart = coll.gameObject;

    //            targetKart = go_targetKart.GetComponent<KartActor2>();
    //            targetKart.playerDisabled = true;
    //            Destroy(this.gameObject);

    //        }
    //    }
    //}
}