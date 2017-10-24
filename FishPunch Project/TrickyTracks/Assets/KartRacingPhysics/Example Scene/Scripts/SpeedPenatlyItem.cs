using UnityEngine;
using System.Collections;

/// <summary>
/// Simple speed penalty item that also spins the vehicle around
/// </summary>
public class SpeedPenatlyItem : PickupItemBase 
{
	protected override void OnPickupCollected(KartController kart)
	{
		kart.SpeedPenalty();
		kart.Spin(2.0f);
	}
}
