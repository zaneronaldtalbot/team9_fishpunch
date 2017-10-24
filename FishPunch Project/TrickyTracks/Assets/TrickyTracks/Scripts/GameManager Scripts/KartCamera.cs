using System;
using UnityEngine;

public class KartCamera : MonoBehaviour
{
    [Serializable]
    public class AdvancedOptions
    {
        public bool updateCameraInUpdate;
        public bool updateCameraInFixedUpdate = true;
        public bool updateCameraInLateUpdate;
    }

    //How smoothly camera follows
    public float smoothing = 6f;

    //where the camera should look
    public Transform lookAtTarget;

    //Where the target of the camera is.
    public Transform positionTarget;
    
    public AdvancedOptions advancedOptions;


    private void FixedUpdate()
    {
        if (advancedOptions.updateCameraInFixedUpdate)
            UpdateCamera();
    }

    private void Update()
    {

        if (advancedOptions.updateCameraInUpdate)
            UpdateCamera();
    }

    private void LateUpdate()
    {
        if (advancedOptions.updateCameraInLateUpdate)
            UpdateCamera();
    }

    private void UpdateCamera()
    {
 
            transform.position = Vector3.Lerp(transform.position, positionTarget.position, Time.deltaTime * smoothing);
            transform.LookAt(lookAtTarget);
    }
}
