using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RefreshGamePads : MonoBehaviour {

	
	// Update is called once per frame
	void Update () {
        GamePadManager.Instance.Refresh();
	}
}
