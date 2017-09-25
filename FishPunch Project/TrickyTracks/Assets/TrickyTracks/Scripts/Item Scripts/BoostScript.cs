using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoostScript : MonoBehaviour {

    private KartActor2 kart;

    public float rotationSpeed = 50.0f;
    private GameObject boostPrefab;

	// Use this for initialization
	void Start () {
        boostPrefab = gameObject.transform.parent.gameObject;
	}


    // Update is called once per frame
    void Update()
    {
        if (kart != null)
        {
            if (kart.itemBoost)
            {
                GameObject.Destroy(boostPrefab);
                GameObject.Destroy(this.gameObject);
            }
        }

        boostPrefab.transform.Rotate(0, rotationSpeed * Time.deltaTime,0);
    }

    void OnTriggerEnter(Collider coll)
    {
        if (coll.gameObject.tag == "Player")
        {
            kart = coll.gameObject.GetComponent<KartActor2>();
        }
    }

}
