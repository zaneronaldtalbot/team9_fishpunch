using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketActor : MonoBehaviour {

    private GameObject go_kart;
    private GameObject go_targetKart;
    private KartActor2 kart;
    private KartActor2 targetKart;
    private GameObject rocket;
  
    public float rocketLife = 5.0f;
    public float rocketSpeed = 10.0f;

    private float timer = 0.0f;
	// Use this for initialization
	void Start () {
        rocket = GetComponent<GameObject>();
	}
	
	// Update is called once per frame
	void Update () {

        if (kart.fireRPG)
        {
            Instantiate(rocket);
            timer += Time.deltaTime;
            rocket.transform.forward = kart.transform.forward;
            rocket.transform.Translate(kart.transform.forward * rocketSpeed * Time.deltaTime);

            if (timer > rocketLife)
            {
                timer = 0.0f;
                kart.fireRPG = false;
                Destroy(this.gameObject);
            }
        }
	}

    void OnTriggerEnter(Collider coll)
    {
        if(coll.gameObject.tag == "Player")
        {
            go_kart = coll.gameObject;
            kart = go_kart.GetComponent<KartActor2>();
            kart.itemRPG = true;
        }
    }

    void OnCollisionEnter(Collision coll)
    {
        if (coll.gameObject.tag == "Player")
        {
            go_targetKart = coll.gameObject;
            targetKart = go_targetKart.GetComponent<KartActor2>();
            targetKart.playerDisabled = true;
            Destroy(this.gameObject);

        }
    }
}
