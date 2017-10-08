using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemMine : MonoBehaviour
{

    public float rotationSpeed = 50.0f;

    private KartActor2 kart;
    private GameObject minePrefab;


    // Use this for initialization
    void Start()
    {
        minePrefab = this.gameObject.transform.parent.gameObject;
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
            kart = coll.gameObject.GetComponentInParent<KartActor2>();
            if (!kart.itemRPG && !kart.itemBoost)
            {
                kart.itemMine = true;
            }
        }
    }
}
