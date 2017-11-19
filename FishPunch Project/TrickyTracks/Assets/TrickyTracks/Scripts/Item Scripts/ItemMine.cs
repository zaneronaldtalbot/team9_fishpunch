using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Written By Angus Secomb
//Last edited 19/10/17
public class ItemMine : MonoBehaviour
{

    public float rotationSpeed = 50.0f;

    private PlayerActor kart;
    private GameObject minePrefab;

    private Audio audioActor;

    // Use this for initialization
    void Start()
    {
        minePrefab = this.gameObject.transform.parent.gameObject;
        audioActor = GameObject.Find("Manager").GetComponent<Audio>();
    }

    // Update is called once per frame
    void Update()
    {


        if (kart != null)
        {
            //Destroy mine pick up if kart grabs it.
                if (kart.itemMine)
                {
                   
                    GameObject.Destroy(minePrefab);
                    GameObject.Destroy(gameObject);
                }
            
        }
        //Rotate mine.
        minePrefab.transform.Rotate(0, rotationSpeed * Time.deltaTime, 0);
    }

    void OnTriggerEnter(Collider coll)
    {
        //If player hits mine set item mine to true.
        if (coll.gameObject.tag == "Player")
        {
            kart = coll.gameObject.GetComponentInParent<PlayerActor>();
            if (!kart.itemRPG && !kart.itemBoost)
            {
                kart.itemMine = true;
            }
        }

        if (coll.gameObject.layer == 9)
        {
            Debug.Log("it's doing that thing i want");
        }
    }
}
