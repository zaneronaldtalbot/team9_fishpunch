using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrefabDisabledActor : MonoBehaviour
{
    private NewPlacementController npController;
    private GameObject colliderObject;
    private Vector3 lastPos;
    private GameObject[] objects;
    public float timer = 5.0f;

    public bool prefabSet = false;

    public Material transparentItem;
    private Material noo;

    private Renderer rend;
    // Use this for initialization
    void Start()
    {
        objects = GameObject.FindGameObjectsWithTag("Item");

        rend = GetComponentInChildren<Renderer>();
         rend.material.color = new Color(0, 0, 0, 0);
        npController = GameObject.Find("Manager").GetComponent<NewPlacementController>();
        if (this.gameObject.name == "Buzzsaw(Clone)")
        {
            colliderObject = this.gameObject.transform.Find("SawBlade_001").gameObject;


        }
        else
        {
            colliderObject = this.gameObject.transform.Find("Collider").gameObject;
        }


        colliderObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
        if (this.gameObject.transform.position == lastPos)
        {
            timer -= Time.deltaTime;
        }

        if (timer < 0)
        {
            colliderObject.SetActive(true);
            rend.material.color = new Color(1, 1, 1, 1);
        }

        lastPos = this.gameObject.transform.position;

    }

    void OnTriggerEnter(Collider coll)
    {
        if((coll.gameObject.tag == "Item") || (coll.gameObject.tag == "ItemMine") || (coll.gameObject.tag == "ItemRPG") || (coll.gameObject.tag == "OilSlick")
            || (coll.gameObject.tag == "Boost"))
        {
            //cannotPlace = true;
            Debug.Log("Lol");


        }

        if ((coll.gameObject.tag == "ColliderField"))
        {
        //npc.cannotPlace = true;

        }
    }

    void OnTriggerExit(Collider coll)
    {
        if ((coll.gameObject.tag == "Item") || (coll.gameObject.tag == "ItemMine") || (coll.gameObject.tag == "ItemRPG"))
        {
            //    cannotPlace = false;
            Debug.Log("l");
        }

        if (coll.gameObject.tag == "ColliderField")
        {
            //cannotPlace = false;
        }
    }

}
