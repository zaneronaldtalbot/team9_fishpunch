using UnityEngine;
using System.Collections;

/// <summary>
/// Simple speed boost pickup item.
/// </summary>
public class SpeedBoostItem : PickupItemBase 
{
	protected override void OnPickupCollected(KartController kart)
	{
		kart.SpeedBoost();
	}
}
