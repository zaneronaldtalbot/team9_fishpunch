using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Written by Angus Secomb
//Last edited 10/11/17
public class BoostScript : MonoBehaviour
{

    private PlayerActor kart;

    public float rotationSpeed = 50.0f;
    private GameObject boostPrefab;

    public float boostValue = 80.0f;
    public float boostTime = 2.0f;

    // Use this for initialization
    void Start()
    {
        boostPrefab = gameObject.transform.parent.gameObject;
    }


    // Update is called once per frame
    void Update()
    {
        if (kart != null)
        {
            //Destroy boost pick up if player picks it up.
            if (kart.itemBoost)
            {
                GameObject.Destroy(boostPrefab);
                GameObject.Destroy(this.gameObject);
            }

        }

        //Rotate boost pick up
        boostPrefab.transform.Rotate(0, rotationSpeed * Time.deltaTime, 0);
    }

    void OnTriggerEnter(Collider coll)
    {
        //If player hits boost set item boost to true.
        if (coll.gameObject.tag == "Player")
        {
            kart = coll.gameObject.GetComponentInParent<PlayerActor>();
            kart.kart.SpeedBoost(boostValue, 2, boostTime, 1);
        }

    }
}
