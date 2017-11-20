using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//Written By Angus Secomb
//Last edited 20/11/17
public class MenuMusic : MonoBehaviour {

    public AudioSource menuMusic;
    public AudioSource musicOne;

    public AudioSource musicTwo;
    private Scene activeScene;

    bool newSong = false;
    private int randNum;

    private float musicDelay = 3.0f;

	// Use this for initialization
	void Start () {

        randNum = Random.Range(0, 2);
        DontDestroyOnLoad(this.gameObject);
	}
	
	// Update is called once per frame
	void Update () {
       
        activeScene = SceneManager.GetActiveScene();

        if (activeScene.buildIndex == 1)
        {
            musicDelay = 3;

            if(musicOne.isPlaying)
            {
                musicOne.Stop();
            }

            if(!menuMusic.isPlaying)
            {
                menuMusic.Play();
            }
        }


        if(activeScene.buildIndex == 2)
        {
            musicDelay -= Time.deltaTime;


            switch(randNum)
            {
                case 0:
                    if (menuMusic.isPlaying)
                    {
                        menuMusic.Stop();
                    }


                    if (musicDelay < 0)
                    {
                        if (!musicOne.isPlaying && !newSong)
                        {
                            newSong = true;
                            musicOne.Play();
                        }
                    }

                    if(!musicOne.isPlaying && newSong)
                    {
                        musicTwo.Play();
                    }
                    break;
                case 1:
                    if (menuMusic.isPlaying)
                    {
                        menuMusic.Stop();
                    }


                    if (musicDelay < 0)
                    {
                        if ((!musicTwo.isPlaying) && !newSong)
                        {
                            newSong = true;
                            musicTwo.Play();
                        }

                        
                    }

                    if(!musicTwo.isPlaying && newSong)
                    {
                        musicOne.Play();
                    }
                    break;
                default:
                    break;
            }
        

        }



	}
}
