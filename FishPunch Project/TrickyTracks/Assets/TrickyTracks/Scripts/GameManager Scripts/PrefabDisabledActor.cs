using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Written By Angus Secomb
//Last edited 5/11/17
public class PrefabDisabledActor : MonoBehaviour
{
    private NewPlacementController npController;
    public AudioSource sound;
    private GameObject colliderObject;
    private Vector3 lastPos;
    private GameObject[] objects;
    public float timer = 5.0f;

    public bool prefabSet = false;

    public Material transparentItem;
    private Material noo;

    public Renderer rendblade;
    private Renderer rend;
    // Use this for initialization
    void Start()
    {
        objects = GameObject.FindGameObjectsWithTag("Item");

        rend = GetComponentInChildren<Renderer>();
         rend.material.color = new Color(0, 0, 0, 0);
        if (rendblade != null)
        {
            rendblade.material.color = new Color(0, 0, 0, 0);
        }
        npController = GameObject.Find("Manager").GetComponent<NewPlacementController>();
        if (this.gameObject.name == "Buzzsaw(Clone)")
        {
            colliderObject = this.gameObject.transform.Find("SawBlade_001").gameObject.transform.Find("Collider").gameObject;


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
            if (rendblade != null)
            {
                rendblade.material.color = new Color(1, 1, 1, 1);
            }
        }

        lastPos = this.gameObject.transform.position;

    }

}
