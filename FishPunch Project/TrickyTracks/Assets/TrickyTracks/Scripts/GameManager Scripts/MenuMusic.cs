using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//Written By Angus Secomb
//Last edited 18/11/17
public class MenuMusic : MonoBehaviour {

    public AudioSource menuMusic;
    public AudioSource inGameMusic;
    private Scene activeScene;

    private float musicDelay = 3.0f;

	// Use this for initialization
	void Start () {
  

        DontDestroyOnLoad(this.gameObject);
	}
	
	// Update is called once per frame
	void Update () {

        activeScene = SceneManager.GetActiveScene();

        if (activeScene.buildIndex == 1)
        {
            musicDelay = 3;

            if(inGameMusic.isPlaying)
            {
                inGameMusic.Stop();
            }

            if(!menuMusic.isPlaying)
            {
                menuMusic.Play();
            }
        }


        if(activeScene.buildIndex == 2)
        {
            musicDelay -= Time.deltaTime;

            if (menuMusic.isPlaying)
            {
                menuMusic.Stop();
            }


            if (musicDelay < 0)
            {
                if (!inGameMusic.isPlaying)
                {
                    inGameMusic.Play();
                }
            }

        }



	}
}
