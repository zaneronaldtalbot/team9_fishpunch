using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Written By Angus Secomb
//Last edited 20/11/17
public class RocketActor : MonoBehaviour
{
    //Gameobject

    public GameObject rocket;
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


        if (itemManager.rocketFired)
        {
            if (timer > rocketLife)
            {

                timer = 0.0f;

                Destroy(this.gameObject.transform.parent.gameObject);
            }


        }


        timer += Time.deltaTime;

    }

    void OnCollisionEnter(Collision coll)
    {
        if (coll.gameObject.tag == "Terrain")
        {
            Destroy(this.gameObject.transform.parent.gameObject);
        }
        if (coll.gameObject.tag == "Item")
        {
            Debug.Log("it's doing that thing i want");
        }
    }

    void OnTriggerEnter(Collider coll)
    {
        if (coll.gameObject.layer == 9)
        {
            Debug.Log("it's doing that thing i want");
        }
    }
}