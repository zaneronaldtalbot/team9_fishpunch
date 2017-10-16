using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrefabDisabledActor : MonoBehaviour {

    private GameObject colliderObject;

    public float timer = 5.0f;

    public bool prefabSet = false;

	// Use this for initialization
	void Start () {
        if(this.gameObject.name == "Buzzsaw(Clone)")
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
	void Update ()
    {

            timer -= Time.deltaTime;
        

        if(timer < 0)
        {
            colliderObject.SetActive(true);
        }



	}



}
