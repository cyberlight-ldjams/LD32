using UnityEngine;
using System.Collections;

/**
 * A track maker produces railroad ties from iron
 */
public class TrackMaker : Workshop {
	public override Resource resourceUsed { get { return Resource.Iron; } }
	public override Resource goodProduced { get { return Resource.RailroadTie; } }
	public override float inOutRatio { get { return 0.8f; } }
}
