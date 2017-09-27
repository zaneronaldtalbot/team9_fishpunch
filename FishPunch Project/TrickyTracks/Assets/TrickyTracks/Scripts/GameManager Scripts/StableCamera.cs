using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StableCamera : MonoBehaviour {

    public GameObject TheCar;
    public float CarX;
    public float CarY;
    public float CarZ;
    private Vector3 boom;


    public float speed = 2.0f;

    void Start()
    {
        boom = this.transform.position - TheCar.transform.position;
    }
	
	// Update is called once per frame
	void Update () {

      
        CarX = TheCar.transform.eulerAngles.x;
        CarY = TheCar.transform.eulerAngles.y;
        CarZ = TheCar.transform.eulerAngles.z;

        transform.eulerAngles = new Vector3(CarX - CarX, CarY, CarZ - CarZ);
        Vector3 target_pos = transform.eulerAngles + boom;
        this.transform.eulerAngles = Vector3.Lerp(transform.eulerAngles, target_pos, speed * Time.deltaTime);
	}
}
