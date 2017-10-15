using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoodCamera : MonoBehaviour {

    public Transform target;

    public float speed = 1;
    public Vector3 direction;
    private Vector3 boom;

	// Use this for initialization
	void Start () {
      direction = target.forward;
        boom = this.transform.position - target.position;
	}
	
	// Update is called once per frame
	void Update () {

        Vector3 target_pos = target.position + boom;

        this.transform.position = target_pos / Time.deltaTime;
        this.transform.rotation = Quaternion.Lerp(transform.rotation, target.rotation, speed * Time.deltaTime);

	}
}
