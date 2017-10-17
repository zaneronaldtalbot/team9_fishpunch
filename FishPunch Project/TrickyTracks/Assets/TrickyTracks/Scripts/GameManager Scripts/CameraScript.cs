using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour {

    [Header("Camera Things")]
    public Camera playerCamera;

    public Transform carCam;
    public Transform Car;
    public Rigidbody carPhysics;

    //Vector3 posOffset;

    [Range(-100,100)]
    public int xTest;
    [Range(-100, 100)]
    public int yTest;
    [Range(-100, 100)]
    public int zTest;

   
    public float rotationThreshold = 1f; // if car speed is less then the value the camera will look forwards at the car (default)

    public float cameraStickiness = 10f; // how cloase the camera is to the car

    [Range(0,100)]
    public float cameraRotationSpeed = 5f; // camera smoother
    public float CameraRotDamping = 5f;
    private float rotOffset;

    void Start()
    {
        carCam = Camera.main.GetComponent<Transform>();
        Car = GetComponent<Transform>();
        carPhysics = GetComponent<Rigidbody>();

        //posOffset = carCam.transform.position - Car.transform.position;

        //SetUpCamera(); // sets up the 1 - 4 player cameras
    }

    void LateUpdate()
    {
        Quaternion look;

        Vector3 offset = Car.transform.position - transform.position;

        Vector3 targetCarPosition = new Vector3(Car.position.x, Car.position.y + yTest, Car.position.z);

        carCam.position = Vector3.Lerp(Car.position, targetCarPosition + offset, cameraStickiness * Time.fixedDeltaTime);

        if (carPhysics.velocity.magnitude < rotationThreshold)
        {
            look = Quaternion.LookRotation(Car.forward);
        }
        else
        {
            look = Quaternion.LookRotation(carPhysics.velocity.normalized);
            
        }


        //Quaternion rotOffset = Quaternion.Euler(0, 90, 0);
        
        look = Quaternion.Slerp(Car.rotation, look, cameraRotationSpeed * Time.fixedDeltaTime);
        carCam.rotation = look;
        //transform.LookAt(Car);
    }

    
    /*
    void LateUpdate()
    {
        float wantedAngle = Car.transform.eulerAngles.y;

        float myAngle = transform.localEulerAngles.y;

        myAngle = Mathf.LerpAngle(myAngle, wantedAngle, cameraRotationSpeed * Time.deltaTime);
        Quaternion currentRotation = Quaternion.Euler(0, myAngle, 0);
        transform.position = Car.pos + (currentRotation * posOffset);

        //transform.LookAt(Car);
    }
    */

//    private void SetUpCamera()
//    {
//        float cameraWidth = 1; // the size of the screen that the camera is taking up ( Width )
//        float cameraHeight = 0.5f; // the size of the screen that the camera is taking up ( Height )
//
//        float cameraXpos = 0; // where the camera is looking on the screen
//        float cameraYpos = 0.5f;
//
//        if (KartActor2.NumOfPlay == 2) // if it is 2 player have the camera 
//        {
//            cameraWidth = 1;
//        }
//        else
//        {
//            cameraWidth = 0.5f;
//        }
//
//        if (KartActor2.PlayNum == 2)
//        { // sets the position of the player camera
//            cameraYpos = 0;
//            if (KartActor2.NumOfPlay > 2)
//            {
//                cameraXpos = 0;
//            }
//            else
//            {
//                cameraXpos = 0.5f;
//            }
//        }
//        else if (KartActor2.PlayNum == 3) // if 3 players set camera to this pos
//        {
//            cameraXpos = 0.5f;
//        }
//
//        playerCamera.rect = new Rect(cameraXpos, cameraYpos, cameraWidth, cameraHeight);
//    }
    /*
    
    
    private Quaternion lastFrameCarRotation;
    public GameObject cameraPivot;
    [Range(0, 20)]
    public float rotationSpeed = 1;

    void Start()
    {
        SetUpCamera(); // sets up the 1 - 4 player cameras
    }

    void LateUpdate()
    {
        RotateCamera();
        transform.LookAt(cameraPivot.transform);
    }




    private void RotateCamera()
    {
        cameraPivot.transform.rotation = lastFrameCarRotation; // set the last frames rotation to the camera pivot cube.

        var q = Quaternion.LookRotation(cameraPivot.transform.position - transform.position);
        cameraPivot.transform.rotation = Quaternion.RotateTowards(cameraPivot.transform.rotation, q, rotationSpeed * Time.deltaTime); // makes q look at the object smoothly

        lastFrameCarRotation = transform.rotation; // The last rotation is saved as the next frame.
    }


    
    */
}
