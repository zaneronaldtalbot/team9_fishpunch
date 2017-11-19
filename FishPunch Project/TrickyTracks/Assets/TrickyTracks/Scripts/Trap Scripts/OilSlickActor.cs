using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Written By Angus Secomb
//Last edited 5/11/17
public class OilSlickActor : MonoBehaviour
{
    private PlayerActor kart;


    // Use this for initialization
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    
     
    }

    void OnCollisionEnter(Collision coll)
    {
        if (coll.gameObject.tag == "Player")
        {

                kart = coll.gameObject.GetComponentInParent<PlayerActor>();
                kart.hitSlick = true;
                Destroy(this.gameObject.transform.parent.gameObject);
            
        }

    }

    void OnTriggerEnter(Collider coll)
    {
        if (coll.gameObject.tag == "Player")
        {
                kart = coll.gameObject.GetComponentInParent<PlayerActor>();
                kart.hitSlick = true;
                Destroy(this.gameObject.transform.parent.gameObject);
        }
    }
}
