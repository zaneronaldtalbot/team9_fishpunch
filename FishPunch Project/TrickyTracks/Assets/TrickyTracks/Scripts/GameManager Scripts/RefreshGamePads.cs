using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//Written by Angus Secomb
//Last edited 5/10/17
public class RefreshGamePads : MonoBehaviour {

	
	// Update is called once per frame
	void Update () {
        GamePadManager.Instance.Refresh();
	}
}
