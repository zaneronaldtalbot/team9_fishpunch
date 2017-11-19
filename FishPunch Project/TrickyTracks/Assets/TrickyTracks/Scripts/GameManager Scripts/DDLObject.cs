using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Written by Angus Secomb
public class DDLObject : MonoBehaviour {

	// Use this for initialization
	void Start () {

        DontDestroyOnLoad(this.gameObject);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
