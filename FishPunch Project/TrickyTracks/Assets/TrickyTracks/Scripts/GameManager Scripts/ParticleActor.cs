using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Written by Angus Secomb
//Last edited 18/11/2017
public class ParticleActor : MonoBehaviour {

    public ParticleSystem particlesOne;
    public ParticleSystem particlesTwo;
    public ParticleSystem sparksOne;
    public ParticleSystem sparksTwo;

    private ParticleSystem p3;

    private PlayerActor kart;

    public float playTime = 2.0f;

	// Use this for initialization
	void Start () {

        if (this.gameObject.tag == "Player")
        {
            kart = gameObject.GetComponent<PlayerActor>();
        }

        p3 = particlesTwo;
	}

    // Update is called once per frame
    void Update()
    {


        if (kart != null)
        {
            if (kart.gamepad.GetButtonDown("B"))
            {               
                    particlesOne.time = playTime;
                sparksOne.time = playTime;

                sparksOne.Play();

                    particlesOne.Play();


                sparksTwo.time = playTime;

                sparksTwo.Play();
                    particlesTwo.Play();
                    particlesTwo.time = playTime;
            }
            else if (kart.gamepad.GetButtonUp("B"))
            {
                if (particlesOne.isPlaying)
                {
                    particlesOne.Stop();
                }
                if (particlesTwo.isPlaying)
                {
                    particlesTwo.Stop();

                }
                if(sparksOne.isPlaying)
                {
                    sparksOne.Stop();
                }
                if(sparksTwo.isPlaying)
                {
                    sparksTwo.Stop();
                }

            }
        }
    }
}
