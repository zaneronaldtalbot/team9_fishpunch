using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemRocket : MonoBehaviour {

    public float rotationSpeed = 50.0f;
    
    [HideInInspector]
    public KartActor2 kart;
    private GameObject rocketPrefab;

	// Use this for initialization
	void Start () {
        rocketPrefab = gameObject.transform.parent.gameObject;
	}
	
	// Update is called once per frame
	void Update () {

        if (kart != null)
        {
            if(kart.itemRPG)
            {
                GameObject.Destroy(rocketPrefab);
                GameObject.Destroy(this.gameObject);
               
            }
        }
        rocketPrefab.transform.Rotate(0, rotationSpeed * Time.deltaTime, 0);
    }

    void OnTriggerEnter(Collider coll)
    {
        if (coll.gameObject.tag == "Player")
        {
            kart = coll.gameObject.GetComponentInParent<KartActor2>();

            if (!kart.itemBoost && !kart.itemMine)
            {
                kart.itemRPG = true;
            }
    
        }
    }
}
