using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Audio : MonoBehaviour {

    public AudioSource beeping, crash;

    bool playOnce = false, playOnce2 = false;
	// Use this for initialization
	void Start () {
        beeping = GameObject.Find("AudioBeep").GetComponent<AudioSource>();
        crash = GameObject.Find("AudioCrash").GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}


    void mineExplosion()
    {
        playOnce = true;
        if (playOnce)
        {
            if (!beeping.isPlaying)
            {
                beeping.Play();
            }

      
            playOnce = false;
        }

    }
}
