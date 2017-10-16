using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoodCamera : MonoBehaviour {

    public float turningScale = 1.0f;
    public GameObject kartObject;
    public GameObject cameraPivot;
    private KartActor2 kart;

	// Use this for initialization
	void Start () {
        kart = kartObject.GetComponent<KartActor2>();
	}
	
	// Update is called once per frame
	void Update () {

        
        cameraPivot.transform.Rotate(0, kart.turnValue / turningScale,0);
	}
}
