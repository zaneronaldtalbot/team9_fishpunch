using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// The Kart Controller. Handles all the dynamics for the kart and also implements some helper functions for 
/// things like speed boosts/penalties, and making the kart spin/wiggle/jump.
/// Edited By Angus Secomb
/// Last edited 18/11/17
/// </summary>
[RequireComponent(typeof(Rigidbody))]
public class KartController : MonoBehaviour
{
	public float topSpeedMPH = 30.0f;			// vehicle's top speed in miles per hour
	public float accelTime = 1.0f;				// time in seconds the vehicle takes to go from stationary to top speed
	public float traction = 0.4f;				// 0-1 value that determines how much traction the vehicle has on the road
	public float decelerationSpeed = 0.5f;      // 0-1 value that determines how quickly the vehicle comes to a rest when thrust is released
    public float decelerationSpeedCopy;         //Copy of the deceleration speed value used to reset values after drifting.
    public float breakPower = 0.0f;             //How fast the kart breaks.
    public float tractionCopy;                  //Copy of the karts traction that resets traction to default after drifting.

	public Transform body;						// the kart body object

	public Transform wheelFL;					// location of front left wheel
	public Transform wheelFR;					// location of front right wheel
	public Transform wheelBL;					// location of rear left wheel
	public Transform wheelBR;					// location of rear right wheel

	public float wheelRadiusFront = 0.5f;		// radius of the front wheels in meters
	public float wheelRadiusBack = 0.6f;		// radius of the rear wheel in meters
	public float maxSteerAngle = 30.0f;			// this is maximum angle the wheels will turn

	public float steerSpeed = 0.5f;				// speed at which the vehicle turns
	public float offRoadDrag = 2.0f;			// drag mulitplier to apply when driving off-road
	public float airDrag = 0.5f;				// drag mulitplier to apply when the vehicle is not on the ground
	
	// this controls how much (if at all) the body of the kart is rotated when steering. it's totally physically incorrect
	// but can give a nice visual effect, making the kart seem to slide around the corners more.
	public float visualOversteerAmount = 0.2f;

	/// <summary>
	/// Gets or sets the thrust (i.e. how far down is the accelerator pressed)
	/// </summary>
	/// <value>The amount of thrust to apply to the vehicle. Varies between -1 and 1 (-1 indicates full throttle but in reverse gear)</value>
	public float Thrust
	{
		get { return thrust; }
		set { thrust = Mathf.Clamp(value, -1.0f, 1.0f); }
	}

    public void LockSteering(float steerValue, float time)
    {
        steerLockTimer = time;
        steerLockValue = steerValue;
    }

	/// <summary>
	/// Gets or sets the steering value.
	/// </summary>
	/// <value>Amount to steer. -1 is full lock left, +1 is full lock right, 0 is straight ahead</value>
	public float Steering
	{
		get { return steer; }
		set { steer = Mathf.Clamp(value, -1.0f, 1.0f);; }
	}
	
	/// <summary>
	/// Gets the current speed of the vehicle in miles per hour
	/// </summary>
	/// <value>The current speed of the vehicle in miles per hour</value>
	public float MPH
	{
		get { return currentMPH; }
	}

	/// <summary>
	/// Gets a value indicating whether the vehicle is currently touching the ground
	/// </summary>
	/// <value><c>true</c> if the vehicle is touching the ground; otherwise, <c>false</c>.</value>
	public bool IsGrounded
	{
		get { return isGrounded; }
	}

	/// <summary>
	/// Gets a value indicating whether the vehicle is off road.
	/// </summary>
	/// <value><c>true</c> if the vehicle is off road; otherwise, <c>false</c>.</value>
	public bool IsOffRoad
	{
		get { return isOffRoad; }
	}

	/// <summary>
	/// Gets a value indicating whether the vehicle is upside down (i.e. the local up-vector is pointing down)
	/// </summary>
	/// <value><c>true</c> if the vehicle is overturned; otherwise, <c>false</c>.</value>
	public bool IsOverturned
	{
		get { return isOverturned; }
	}

	/// <summary>
	/// Spin the vehicle 720 degrees around the Y-axis
	/// </summary>
	/// <param name="time">Length of time in seconds to complete the spin</param>
	public void Spin(float time)
	{
		spinTime = time;
		spinTimer = 0.0f;
	}

	/// <summary>
	/// Wiggle the vehicle around the Y-axis (e.g. for wheel-spin or oil slick effects)
	/// </summary>
	/// <param name="time">Length of time in seconds to complete the wiggle</param>
	public void Wiggle(float time)
	{
		wiggleTime = time;
		wiggleTimer = 0.0f;
	}

	/// <summary>
	/// Make the vehicle do a jump
	/// </summary>
	/// <param name="height">Height in meters to jump</param>
	public void Jump(float height)
	{
		// Make the vehicle jump up
		if(isGrounded)
		{
			float gravity = 9.81f;
			float verticalSpeed = Mathf.Sqrt(2.0f * height * gravity);
			Vector3 vel = physicsBody.velocity;
			vel.y += verticalSpeed;
			physicsBody.velocity = vel;
		}
	}
	
	/// <summary>
	/// Give the vehicle a temporary speed boost
	/// </summary>
	/// <param name="boostTopSpeedMPH">New top speed for the vehicle in miles per hour</param>
	/// <param name="boostAccelTime">New acceleration speed</param>
	/// <param name="boostTime">Length of time (in sec) the boost lasts before starting to fade</param>
	/// <param name="fadeTime">Length of time the boost takes to fade out</param>
	public void SpeedBoost(float boostTopSpeedMPH, float boostAccelTime, float boostTime, float fadeTime)
	{
		boostMPH = boostTopSpeedMPH;
		boostAccel = boostAccelTime;
		boostAmount = 1.0f;
		boostAmountVel = 0.0f;
		boostTimer = boostTime;
		boostFadeTime = fadeTime;
		penaltyAmount = 0.0f;
	}
	
	/// <summary>
	/// Give the vehicle a temporary speed boost (with default parameters)
	/// </summary>
	public void SpeedBoost()
	{
		SpeedBoost(1.6f * topSpeedMPH, 0.25f * accelTime, 1.0f, 1.0f);
	}
	
	/// <summary>
	/// Apply a speed penalty to the vehicle
	/// </summary>
	/// <param name="amount">Strength of the penalty between 0 and 1. 1.0 will stop the vehicle completely</param>
	/// <param name="penaltyTime">Time in seconds that the penalty lasts before starting to fade out</param>
	/// <param name="fadeTime">Length of time the penalty takes to fade out</param>
	public void SpeedPenalty(float amount, float penaltyTime, float fadeTime)
	{
		penaltyAmount = amount;
		penaltyTimer = penaltyTime;
		penaltyFadeTime = fadeTime;
		penaltyAmountVel = 0.0f;
		boostAmount = 0.0f;
	}
	
	/// <summary>
	/// Apply a speed penalty to the vehicle (with default parameters)
	/// </summary>
	public void SpeedPenalty()
	{
		SpeedPenalty (0.4f, 0.5f, 1.0f);
	}

	#region Implementation

	private float thrust = 0.0f;			// our current thrust value
	private float steer = 0.0f;				// our current steering value

	private float currentMPH = 0.0f;		// our current speed in miles-per-hour

	// a couple of useful constants
	private const float metresToMiles = 1.0f / 1609.344f;
	private const float secondsToHours = 3600.0f;
	
	private bool isGrounded = true;			// are we on the ground?
	private bool isOffRoad = false;			// are we driving off road?
	private bool isOverturned = false;		// is the kart upside-down?
	
	private float engineThrust;				// current thrust value including any penalties

	private float visualOversteerVel;

	private WheelCollider[] wheelColliders;
	private GameObject steeringFL;
	private GameObject steeringFR;
    
    [HideInInspector]
	public Rigidbody physicsBody;
	private Transform visualBody;

	private float spinTime;
	private float spinTimer;

	private float wiggleTime;
	private float wiggleMaxAngle = 15.0f;	// maximum angle a wiggle rotates the vehicle by
	private float wiggleTimer;

    private float steerLockTimer;
    private float steerLockValue;

	private float boostMPH;
	private float boostAccel;
	private float boostAmount;
	private float boostAmountVel;
	private float boostTimer;
	private float boostFadeTime;
	
	private float penaltyAmount;
	private float penaltyAmountVel;
	private float penaltyTimer;
	private float penaltyFadeTime;
	private float penaltyDrag = 10.0f;		// drag multiplier used to slow the vehicle during a penalty

	void Start () 
	{
		// set up a new parent transform to hold the visual parts of the kart, so they can be manipulated independantly of the physics.
		CreateVisualBody();

		// create some colliders around the wheels.
		wheelColliders = new WheelCollider[4];
		wheelColliders[0] = CreateWheelCollider(wheelFL.position, wheelRadiusFront);
		wheelColliders[1] = CreateWheelCollider(wheelFR.position, wheelRadiusFront);
		wheelColliders[2] = CreateWheelCollider(wheelBL.position, wheelRadiusBack);
		wheelColliders[3] = CreateWheelCollider(wheelBR.position, wheelRadiusBack);

		// create some extra transforms so we can rotate the front wheels more easily for steering
		steeringFL = new GameObject("SteeringFL");
		steeringFR = new GameObject("SteeringFR");
		steeringFL.transform.position = wheelFL.position;
		steeringFR.transform.position = wheelFR.position;
		steeringFL.transform.rotation = wheelFL.rotation;
		steeringFR.transform.rotation = wheelFR.rotation;
		steeringFL.transform.parent = wheelFL.parent;
		steeringFR.transform.parent = wheelFR.parent;
		wheelFL.parent = steeringFL.transform;
		wheelFR.parent = steeringFR.transform;

		physicsBody = GetComponent<Rigidbody>();

        decelerationSpeedCopy = decelerationSpeed;
        tractionCopy = traction;

      

		// set an artificially low center of gravity to aid in stability.
		Vector3 frontAxleCenter = 0.5f * (wheelFL.localPosition + wheelFR.localPosition);
		Vector3 rearAxleCenter = 0.5f * (wheelBL.localPosition + wheelBR.localPosition);
		Vector3 vehicleCenter = 0.5f * (frontAxleCenter + rearAxleCenter);
		float avgWheelRadius = 0.5f * (wheelRadiusFront + wheelRadiusBack);
		physicsBody.centerOfMass = vehicleCenter - 0.8f*avgWheelRadius*Vector3.up;
	}

	private void CreateVisualBody()
	{
		visualBody = new GameObject("VisualBody").transform;
		visualBody.position = body.position;
		visualBody.rotation = body.rotation;
		visualBody.localScale = Vector3.one;
        visualBody.tag = "Player";
		while (body.childCount > 0)
		{
			body.GetChild(0).parent = visualBody;
		}
		visualBody.parent = body;
	}

	private WheelCollider CreateWheelCollider(Vector3 position, float radius)
	{
		GameObject wheel = new GameObject("WheelCollision");
		wheel.transform.parent = body;
		wheel.transform.position = position;
		wheel.transform.localRotation = Quaternion.identity;
        wheel.tag = "Player";
		WheelCollider collider = wheel.AddComponent<WheelCollider>();
		collider.radius = radius;
		collider.suspensionDistance = 0.1f;
#if UNITY_5
		collider.mass = 5.0f;
		collider.forceAppPointDistance = 0.1f;
		collider.center = 0.5f * collider.suspensionDistance * Vector3.up;
#endif

		// we calculate our own sideways friction and slippage, so we don't need the wheel collider to do it too.
		WheelFrictionCurve sideFriction = collider.sidewaysFriction;
		sideFriction.stiffness = 0.01f;
		collider.sidewaysFriction = sideFriction;

		return collider;
	}

	void FixedUpdate()
	{
		// calculate our current velocity in local space (i.e. so z is forward, x is sideways etc)
		Vector3 relVel = transform.InverseTransformDirection(physicsBody.velocity);
		// our current speed is the forward part of the velocity - note this will be negative if we are reversing.
		currentMPH = relVel.z * metresToMiles * secondsToHours;

		engineThrust = thrust;

		// cast a ray to check if we are grounded or not
		Vector3 frontWheelBottom = 0.5f*(wheelFR.position + wheelFL.position) - new Vector3(0,0.5f,1.0f) * wheelRadiusFront;
		RaycastHit hit;
		isGrounded = Physics.Raycast(frontWheelBottom, -Vector3.up, out hit, 2.0f*wheelRadiusFront);

		// check if the ground beneath us is tagged as 'off road'
		if(isGrounded)
			isOffRoad = hit.collider.gameObject.CompareTag("OffRoad");

		// check if the vehicle has overturned. we don't do anything with this, but a controller script could use it
		// to reset a vehicle that has been overturned for a certain amount of time for example.
		isOverturned = transform.up.y < 0.0f;

		// reduce the thrust if we are currently suffering a penalty.
		engineThrust *= (1.0f - penaltyAmount*penaltyAmount);

		// only apply thrust if the wheels are touching the ground
		if(isGrounded)
			ApplyThrust();
		ApplyDrag();
		ApplySteering();

		// update boost, penalty, spin effects etc 
		ApplyEffects();

		// calculate the angle that the wheels should have rolled since the last frame given our current speed
		float wheelRotationFront = (relVel.z / wheelRadiusFront) * Time.deltaTime * Mathf.Rad2Deg;
		float wheelRotationRear = (relVel.z / wheelRadiusBack) * Time.deltaTime * Mathf.Rad2Deg;
		// now rotate each wheel
		wheelFL.Rotate(wheelRotationFront, 0.0f, 0.0f);
		wheelFR.Rotate(wheelRotationFront, 0.0f, 0.0f);
		wheelBL.Rotate(wheelRotationRear, 0.0f, 0.0f);
		wheelBR.Rotate(wheelRotationRear, 0.0f, 0.0f);
	}

	private void ApplyDrag()
	{
		// get our velocity relative to our local orientation (i.e. forward motion is along z-axis etc)
		Vector3 relVel = transform.InverseTransformDirection(physicsBody.velocity);

		Vector3 drag = Vector3.zero;

		// calculate our drag coeeficients based on the current handling parameters

		// strength of drag force resisting the vehicle's forward motion
		float forwardDrag = Mathf.Lerp(0.1f, 0.5f, traction);
		// strength of drag force resisting the vehicle's sideways motion
		float lateralDrag = Mathf.Lerp(1.0f, 5.0f, traction);
		// strength of drag that slows the vehicle down when thrust is not pressed (basically just affects deceleration time)
		float engineDrag = Mathf.Lerp(0.0f, 5.0f, decelerationSpeed);

		// calculate drag in forward direction
		// engine drag slows the vehicle down when the accelerator is not being pressed
		drag.z = relVel.z * (forwardDrag + ((1.0f - Mathf.Abs(engineThrust)) * engineDrag));
		// add some additional drag when driving off road
		if(isOffRoad)
			drag.z += relVel.z * offRoadDrag;

		// lateral (sideways drag) slows the vehicle in the direction perpendicular to that in which it is facing.
		drag.x = relVel.x * lateralDrag;

		// when the vehicle is not grounded, reduce the drag force.
		if(!isGrounded)
			drag *= airDrag;

		// if we are currently suffering a penalty, then increase the drag to slow the car down.
		drag = Vector3.Lerp(drag, penaltyDrag * drag, penaltyAmount);

		// transform the drag force back into world space
		drag = transform.TransformDirection(drag);

		// apply the drag by reducing our current velocity directly
		Vector3 vel = physicsBody.velocity;
		vel -= drag * Time.deltaTime;
		physicsBody.velocity = vel;
	}

	private void ApplyThrust()
	{
		// calc our current top speed and acceleration values, taking into account any active boost.
		float topSpeed = Mathf.Lerp(topSpeedMPH, boostMPH, boostAmount);
		float accelerationTime = Mathf.Lerp(accelTime, boostAccel, boostAmount);
#if UNITY_5
		// compensate for differences in Unity 5's WheelCollider implementation
		accelerationTime *= 0.75f;
#endif
		accelerationTime = Mathf.Max(0.01f, accelerationTime);

		float topSpeedMetresPerSec = topSpeed / (metresToMiles * secondsToHours);
		// limit the speed the vehicle can move at in reverse
		float topSpeedReverse = 0.2f * topSpeed;
		// calculate our acceleration value in m/s^2
		float accel = topSpeedMetresPerSec / accelerationTime;

		// if we're at or over the top speed, then don't accelerate any more
		if(currentMPH >= topSpeed || currentMPH <= -topSpeedReverse)
			accel = 0.0f;

		// calculate our final acceleration vector
		Vector3 thrustDir = transform.forward;
		Vector3 accelVec = accel * thrustDir * engineThrust;

		// add our acceleration to our current velocity
		Vector3 vel = physicsBody.velocity;
		vel += accelVec * Time.deltaTime;
		physicsBody.velocity = vel;

		// apply the brakes automatically when the throttle is off to stop the vehicle rolling by itself.
		float brakeTorque = breakPower;
		const float maxBrakeTorque = 20.0f;
		// modify the braking amount based on the current speed so we come to a gentle stop.
		if(engineThrust == 0.0f && currentMPH < 10.0f)
			brakeTorque = maxBrakeTorque * Mathf.Clamp01(decelerationSpeed * (10.0f - currentMPH));
		foreach(WheelCollider wheel in wheelColliders)
		{
			wheel.brakeTorque = brakeTorque;
#if UNITY_5
			// The WheelColliders in Unity 5 require a non-zero motor torque before they will move at all.
			wheel.motorTorque = 0.01f * engineThrust;
#endif
		}

	}

	private void ApplySteering()
	{
		float steerAngle = steer * maxSteerAngle;

		// rotate the front wheels
		steeringFL.transform.localRotation = Quaternion.Euler(0, steerAngle, 0);
		steeringFR.transform.localRotation = Quaternion.Euler(0, steerAngle, 0);

		// only turn the vehicle when we're on the ground and moving
		if(isGrounded && physicsBody.velocity.sqrMagnitude > 0.1f)
		{
			// reverse the steering direction when the vehicle is moving backwards
			Vector3 relVel = transform.InverseTransformDirection(physicsBody.velocity);
			steerAngle *= Mathf.Sign(relVel.z);

			// rotate the vehicle
			Quaternion steerRot = Quaternion.Euler(0, steerAngle * Time.deltaTime * (1.0f + 2.0f*steerSpeed), 0);
			physicsBody.MoveRotation(transform.rotation * steerRot);


			// also rotate the body a little for visual effect
			float currentOversteerAngle = visualBody.localRotation.eulerAngles.y;
			float oversteerAngle = Mathf.SmoothDampAngle(currentOversteerAngle, visualOversteerAmount * steerAngle, ref visualOversteerVel, 0.5f);
			visualBody.localRotation = Quaternion.Euler(0, oversteerAngle, 0);
		}
	}
	
	private float WiggleCurve(float t)
	{
		// basically a sine wave that smoothly fades out over 0-1
		float numWiggles = 3.0f;
		float wave = Mathf.Sin(t * numWiggles * 2.0f*Mathf.PI);
		float amplitude = (1.0f + 2.0f*t*t*t - 3.0f*t*t);
		return wave * amplitude;
	}

	private void UpdateSpin()
	{
		if(spinTime > 0.0f)
		{
			spinTimer += Time.deltaTime;
			// calculate how far through the spin we are.
			float t = spinTimer / spinTime;
			if(t >= 1.0f)
			{
				// spin has finished
				t = 0.0f;
				spinTime = 0.0f;
			}

			Vector3 rot = visualBody.localRotation.eulerAngles;
			// simple easing curve to slow the rotation down towards the end.
			float easing = 1.0f - (1.0f-t)*(1.0f-t);
			// rotate the vehicle by some fraction of 720 degrees
			rot.y = 720.0f * easing;
			visualBody.localRotation = Quaternion.Euler(rot);
		}
	}

    private void UpdateSteerLock()
    {
        steerLockTimer -= Time.deltaTime;
        if(steerLockTimer > 0)
        {
           // steer = steerLockValue;

            steer = Mathf.SmoothDamp(steer, steerLockValue, ref steer, 2.0f);
        }
       
    }

	private void UpdateWiggle()
	{
		if(wiggleTime > 0.0f)
		{
			wiggleTimer += Time.deltaTime;
			// calculate how far through the wiggle we are.
			float t = wiggleTimer / wiggleTime;
			if(t >= 1.0f)
			{
				// wiggle has finished
				t = 0.0f;
				wiggleTime = 0.0f;
			}

			Vector3 rot = visualBody.localRotation.eulerAngles;
			// rotation is given by the wiggle curve (basically a sine wave that fades out over 0-1)
			rot.y = wiggleMaxAngle * WiggleCurve(t);
			visualBody.localRotation = Quaternion.Euler(rot);
		}
	}

	private void UpdateBoost()
	{
		boostTimer -= Time.deltaTime;
		if(boostTimer < 0)
		{
			// fade the boost out after the boostTimer has run out
			boostAmount = Mathf.SmoothDamp(boostAmount, 0.0f, ref boostAmountVel, boostFadeTime);
		}
	}

	private void UpdatePenalty()
	{
		penaltyTimer -= Time.deltaTime;
		if(penaltyTimer < 0)
		{
			// fade the penalty out after the penaltyTimer has run out
			penaltyAmount = Mathf.SmoothDamp(penaltyAmount, 0.0f, ref penaltyAmountVel, penaltyFadeTime);
		}
	}

	private void ApplyEffects()
	{
		// update all the different effects
		UpdateWiggle();
		UpdateSpin();
		UpdateBoost();
		UpdatePenalty();
        UpdateSteerLock();
	}

	#endregion
}
