using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoostBuzzsaw : MonoBehaviour {

    public float boostValue = 80.0f;
    public float boostTime = 2.0f;
    public float boostRespawnTime = 15.0f;

    public ParticleSystem particle;


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        boostRespawnTime -= Time.deltaTime;
        if(boostRespawnTime < 0)
        {
            particle.Play();
            boostRespawnTime = 15.0f;
        }
        if(boostRespawnTime > 0)
        {
            particle.Stop();
        }


	}

    void OnTriggerEnter(Collider coll)
    {
        if(coll.gameObject.tag == "Player")
        {
            if (boostRespawnTime < 0)
            {
                coll.gameObject.GetComponentInParent<PlayerActor>().kart.SpeedBoost(boostValue, 2, boostTime, 1);

            }
        }
    }
}
