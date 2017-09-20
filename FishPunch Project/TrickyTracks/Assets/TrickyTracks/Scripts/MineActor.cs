using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MineActor : MonoBehaviour {

    private KartActor2 kart;
    private Transform kartTransform;

	// Use this for initialization
	void Start () {
 
	}
	
	// Update is called once per frame
	void Update () { 
	}

    private void OnTriggerEnter(Collider coll)
    {
        if(coll.gameObject.tag == "Player")
        {           
            kart = coll.gameObject.GetComponent<KartActor2>();
            kart.playerDisabled = true;
          
            GameObject.Destroy(this.gameObject);
        }
    }
}
