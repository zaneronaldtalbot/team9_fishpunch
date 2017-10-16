using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StableCamera : MonoBehaviour {



    ///Test
    public GameObject car;
    public float rotationDamping;

    [HideInInspector]
    public float distance;
    //public float height;
    //public float heightDamping;
    [HideInInspector]
    public float zoomRatio;
    [HideInInspector]
    public float defaultFOV; // default field of view
    [HideInInspector]
    private float rotationVector;
    private Vector3 Offset;

    void Awake()
    {
        Offset = transform.position - car.transform.position;

    }
    //void FixedUpdate()
    //{
    //    Vector3 localVelocity = car.InverseTransformDirection(car.GetComponent<Rigidbody>().velocity);
    //    if (localVelocity.z < 0.5f)
    //    {
    //        rotationVector = car.eulerAngles.y;
    //    }
    //    else
    //    {
    //        rotationVector = car.eulerAngles.y;
    //    }

    //    float acceleration = car.GetComponent<Rigidbody>().velocity.magnitude;
    //    Camera.main.fieldOfView = defaultFOV + acceleration * zoomRatio * Time.deltaTime;
    //}
    
    void LateUpdate()
    {
        float wantedAngle = car.transform.eulerAngles.y;

        //float wantedHeight = car.position.y + height;
        float myAngle = transform.localEulerAngles.y;
        //float myHeight = transform.position.y;

        myAngle = Mathf.LerpAngle(myAngle, wantedAngle, rotationDamping * Time.deltaTime);
        //myHeight = Mathf.LerpAngle(myHeight, wantedHeight, heightDamping * Time.deltaTime);

        Quaternion currentRotation = Quaternion.Euler(0, myAngle + 90, 0);
    

        transform.position = car.transform.position +  (currentRotation * Offset);
        //transform.position = currentRotation * Vector3.right * distance;
        //transform.rotation = currentRotation. * Offset;
        //Vector3 temp = transform.position;
        //temp.y = myHeight;
        //transform.position = temp;

        transform.LookAt(car.transform);
    }


    ///old code

    /*
    public GameObject TheCar;
    
    public float CarX;
    public float CarY;
    public float CarZ;
    private Vector3 Offset;

    public float speed = 10f;
   
    
    void Start()
    {
        Offset = transform.position - TheCar.transform.position;
        
    }
	
	// Update is called once per frame
	void LateUpdate () {

        
        CarX = TheCar.transform.eulerAngles.x;
        CarY = TheCar.transform.eulerAngles.y;
        CarZ = TheCar.transform.eulerAngles.z;

        //float angle = Quaternion.Angle(transform.rotation, target.rotation);
        transform.eulerAngles = new Vector3(CarX - CarX, CarY, CarZ - CarZ);

        Vector3 target_pos = TheCar.transform.position + Offset;
        transform.LookAt(transform.position);

        transform.position = Vector3.Lerp(transform.position, target_pos, speed * Time.deltaTime);

        
        */

}
