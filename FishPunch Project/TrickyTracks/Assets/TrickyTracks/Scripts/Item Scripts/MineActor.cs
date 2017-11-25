using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Written By Angus Secomb
//Last edited 24/10/17
public class MineActor : MonoBehaviour {

    private PlayerActor kart;

	// Use this for initialization
	void Start () {
        
	}
	
	// Update is called once per frame
	void Update () {
     
	}

    void OnTriggerEnter(Collider coll)
    {
        if (coll.gameObject.tag == "Player")
        {
            kart = coll.gameObject.GetComponentInParent<PlayerActor>();
            if (!kart.immuneToDamage)
            {
                kart.playerDisabled = true;
            }
                GameObject.Destroy(this.gameObject.transform.parent.gameObject);
        }
        if(coll.gameObject.tag == "RPG")
        { 
            GameObject.Destroy(coll.gameObject.transform.parent.gameObject);
            GameObject.Destroy(this.gameObject.transform.parent.gameObject);
        }

        if (coll.gameObject.layer == 9)
        {
            Debug.Log("it's doing that thing i want");
        }

    }
}
