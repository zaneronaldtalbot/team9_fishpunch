using UnityEngine;
using System.Collections;

/// <summary>
/// Base class for pickup items.
/// Handles hiding and showing the item when it is collected, and rotates the items to face the camera
/// </summary>
public abstract class PickupItemBase : MonoBehaviour 
{
	protected abstract void OnPickupCollected(KartController kart);

	void OnTriggerEnter(Collider other)
	{
		// we need to look on the attachedRigidbody for the KartController, because the colliders are attached to a child
		// object.
		KartController kart = other.attachedRigidbody.GetComponent<KartController>();
		if(kart != null)
			OnPickupCollected(kart);
		
		// hide this item for a few seconds.
		Hide();
		Invoke("Show", 5.0f);
	}
	
	void Hide()
	{
		gameObject.SetActive(false);
	}
	
	void Show()
	{
		gameObject.SetActive(true);
	}

	void Update()
	{
		// always face the camera
		Camera cam = Camera.main;
		transform.LookAt(transform.position + cam.transform.rotation * Vector3.back, cam.transform.rotation * Vector3.up);
	}
}
