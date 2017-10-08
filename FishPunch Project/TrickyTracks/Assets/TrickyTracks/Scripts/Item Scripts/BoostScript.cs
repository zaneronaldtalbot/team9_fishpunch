﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoostScript : MonoBehaviour
{

    private KartActor2 kart;

    public float rotationSpeed = 50.0f;
    private GameObject boostPrefab;

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
            kart = coll.gameObject.GetComponentInParent<KartActor2>();
            if (!kart.itemMine && !kart.itemRPG)
            {
                kart.itemBoost = true;
            }
        }

    }
}
