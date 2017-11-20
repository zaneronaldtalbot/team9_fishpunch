using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoostBuzzsaw : MonoBehaviour {

    public float boostValue = 80.0f;
    public float boostTime = 2.0f;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnTriggerEnter(Collider coll)
    {
        if(coll.gameObject.tag == "Player")
        {
            coll.gameObject.GetComponentInParent<PlayerActor>().kart.SpeedBoost(boostValue, 2, boostTime, 1);
        }
    }
}
