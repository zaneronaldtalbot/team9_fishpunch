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
     
	}

    void OnTriggerEnter(Collider coll)
    {
        if(coll.gameObject.tag == "Player")
        {
            kart = coll.gameObject.GetComponentInParent<KartActor2>();
            kart.playerDisabled = true;
            GameObject.Destroy(this.gameObject.transform.parent.gameObject);
        }
    }
}
