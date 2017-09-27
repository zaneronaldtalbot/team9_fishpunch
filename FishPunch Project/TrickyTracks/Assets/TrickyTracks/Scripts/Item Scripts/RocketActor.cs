using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketActor : MonoBehaviour
{

    private ItemManager itemManager;
    private GameObject manager;
    public float rocketLife = 5.0f;
    public float rocketSpeed = 10.0f;

    [HideInInspector]
    public bool rocketLifeOver = false;

    private float timer = 0.0f;
    // Use this for initialization
    void Start()
    {
      //  rocket = this.gameObject.transform.parent.gameObject;
        manager = GameObject.Find("Manager");
        itemManager = manager.GetComponent<ItemManager>();
    }

    // Update is called once per frame
    void Update()
    {
        //rocket.transform.position = kart.gameObject.transform.position;

        if(itemManager.rocketFired)
        {
            if (timer > rocketLife)
            {

                timer = 0.0f;
               
                Destroy(this.gameObject.transform.parent.gameObject);
            }


        }
       
        
            timer += Time.deltaTime;


          
 
    }

 
}