using UnityEngine;
using System.Collections;

public class TrackMaker : Workshop {
	public override Resource resourceUsed { get { return Resource.Iron; } }
	public override Resource goodProduced { get { return Resource.RailroadTie; } }
	public override float inOutRatio { get { return 0.7f; } }
}
