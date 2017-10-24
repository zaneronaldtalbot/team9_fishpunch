using UnityEngine;
using System.Collections;

/// <summary>
/// A simple example ai controller that always tries to drive towards a target object.
/// This is not intended as a complete AI controller, but merely as a starting point.
/// </summary>

[RequireComponent(typeof(KartController))]
public class ExampleAIController : MonoBehaviour 
{
	// target position for the ai, move this around the track a little way in front of the kart to make the kart
	// drive around the track. You can test this out in the editor by dragging it around the track as the game
	// is playing.
	public Transform targetObject; 

	private KartController kart;

	void Start () 
	{
		// keep a reference to the kart component
		kart = GetComponent<KartController>();
	}
	
	void Update () 
	{
		if(targetObject != null)
		{
			// get a vector from our current position to the target
			Vector3 delta = targetObject.position - transform.position;
			// transform that vector so that it is relative to the current facing direction of the kart
			Vector3 directionToObject = transform.InverseTransformDirection(delta);
			// now compute the angle from our current facing direction to the direction we want to travel in
			float angleToTarget = Mathf.Atan2(directionToObject.x, directionToObject.z) * Mathf.Rad2Deg;

			// convert the angle to a -1 => 1 steering value
			kart.Steering = angleToTarget / kart.maxSteerAngle;
			// always try and drive at top speed, for a more realistic ai you'll want to change this when
			// approaching sharp bends and so on.
			kart.Thrust = 1.0f;
		}
		else
		{
			kart.Thrust = 0.0f;
			kart.Steering = 0.0f;
		}
	}
}
