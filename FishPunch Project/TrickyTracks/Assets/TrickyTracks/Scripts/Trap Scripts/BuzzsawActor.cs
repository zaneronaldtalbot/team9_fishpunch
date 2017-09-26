﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuzzsawActor : MonoBehaviour {

    private GameObject sawBlade;
    private MeshRenderer bladeRender;
    private KartActor2 kart;
    
    public float sawSpeed = 3.0f;
    public float bladeSpinSpeed = 500.0f;
    private bool goLeft = true;
    private bool goRight = false;
	// Use this for initialization
	void Start () {
        sawBlade = this.gameObject;
        bladeRender = sawBlade.GetComponentInChildren<MeshRenderer>();
	}

    // Update is called once per frame
    void Update() {

      //  sawBlade.transform.Rotate(5 * Time.deltaTime, 0, 0);

        if (goLeft)
        {
            sawBlade.transform.Translate(0, 0, -sawSpeed * Time.deltaTime);
            bladeRender.transform.Rotate(-bladeSpinSpeed * Time.deltaTime, 0, 0);
        }
        if (goRight)
        {
            sawBlade.transform.Translate(0, 0, sawSpeed * Time.deltaTime);
            bladeRender.transform.Rotate(bladeSpinSpeed * Time.deltaTime, 0, 0);
        }
	}

    void OnTriggerEnter(Collider coll)
    {
        if(coll.gameObject.name == "Trigger2")
        {
            goLeft = false;
            goRight = true;
        }

        if(coll.gameObject.name == "Trigger1")
        {
            goRight = false;
            goLeft = true;
        }

        if(coll.gameObject.tag == "Player")
        {
            kart = coll.gameObject.GetComponent<KartActor2>();
            kart.playerDisabled = true;
        }
    }

    void OnTriggerStay(Collider coll)
    {
        if(coll.gameObject.tag == "Trigger1")
        {
            goRight = true;
        }
    }

}