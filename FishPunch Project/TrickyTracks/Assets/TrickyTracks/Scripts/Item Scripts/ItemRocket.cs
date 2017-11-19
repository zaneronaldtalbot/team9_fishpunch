using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Written By Angus Secomb
//Last edited 20/10/17
public class ItemRocket : MonoBehaviour {

    public float rotationSpeed = 50.0f;
    
    [HideInInspector]
    public PlayerActor kart;
    private GameObject rocketPrefab;

	// Use this for initialization
	void Start () {
        rocketPrefab = gameObject.transform.parent.gameObject;
	}
	
	// Update is called once per frame
	void Update () {

        
        if (kart != null)
        {
            //Destroy rocket item if picked up.
            if(kart.itemRPG)
            {
                GameObject.Destroy(rocketPrefab);
                GameObject.Destroy(this.gameObject);
               
            }
        }
        //Rotate object.
        rocketPrefab.transform.Rotate(0, rotationSpeed * Time.deltaTime, 0);
    }

    void OnTriggerEnter(Collider coll)
    {
        //If it hits a player grab kart script and set item rpg to true.
        if (coll.gameObject.tag == "Player")
        {
            kart = coll.gameObject.GetComponentInParent<PlayerActor>();

            if (!kart.itemBoost && !kart.itemMine)
            {
                kart.itemRPG = true;
            }
    
        }

        if (coll.gameObject.layer == 9)
        {
            Debug.Log("it's doing that thing i want");
        }
    }
}
