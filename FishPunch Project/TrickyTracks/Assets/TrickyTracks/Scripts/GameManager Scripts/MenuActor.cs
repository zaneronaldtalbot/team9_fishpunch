using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuActor : MonoBehaviour {

    public void LoadLevel(string loadlevel)
    {
        SceneManager.LoadScene(loadlevel);


    }

    public void CloseApplication()
    {
        Application.Quit();
    }

	// Use this for initialization
	void Start () {

     
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
