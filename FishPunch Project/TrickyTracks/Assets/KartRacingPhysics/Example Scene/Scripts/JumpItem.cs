using UnityEngine;
using System.Collections;

/// <summary>
/// Jump pickup.
/// Makes the kart do a little jump
/// </summary>
public class JumpItem : PickupItemBase 
{
	protected override void OnPickupCollected(KartController kart)
	{
		kart.Jump(1.0f);
	}
}
