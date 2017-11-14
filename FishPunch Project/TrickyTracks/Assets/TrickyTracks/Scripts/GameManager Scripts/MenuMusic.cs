using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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
 
        if(activeScene.buildIndex == 2)
        {
            musicDelay -= Time.deltaTime;

            if (inGameMusic.isPlaying)
            {
                inGameMusic.Stop();
            }


            if (musicDelay < 0)
            {
                if (!menuMusic.isPlaying)
                {
                    menuMusic.Play();
                }
            }

        }



	}
}
