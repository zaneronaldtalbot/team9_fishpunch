using UnityEngine;
using System.Collections;

/// <summary>
/// Oil slick item.
/// Makes the kart slide a little, and slows it down slightly
/// </summary>
public class OilSlickItem : PickupItemBase 
{
	protected override void OnPickupCollected(KartController kart)
	{
		kart.SpeedPenalty(0.25f, 1.0f, 1.0f);
		kart.Wiggle(1.0f);
	}
}
