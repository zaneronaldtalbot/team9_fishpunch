using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MineActor : MonoBehaviour {

    private KartActor2 kart;

	// Use this for initialization
	void Start () {
        
	}
	
	// Update is called once per frame
	void Update () {
        if (kart != null)
        {
            if(kart.itemMine)
            {
                GameObject.Destroy(this.gameObject);
                
            }
        }
	}

    void OnTriggerEnter(Collider coll)
    {
        if(coll.gameObject.tag == "Player")
        {           
            kart = coll.gameObject.GetComponent<KartActor2>();
        
        }
    }
}
