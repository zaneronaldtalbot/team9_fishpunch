using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleActor : MonoBehaviour {

    public ParticleSystem particlesOne;
    public ParticleSystem particlesTwo;

    private ParticleSystem p3;

    private PlayerActor kart;



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

                particlesOne.time = 0.1f;
                particlesOne.Play();


                particlesTwo.Play();
                particlesTwo.time = 0.1f;

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

            }
        }
    }
}
