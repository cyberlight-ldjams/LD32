using UnityEngine;
using System.Collections;

public class Brickworks : Workshop {
	public override Resource resourceUsed { get { return Resource.Clay; } }
	public override Resource goodProduced { get { return Resource.Brick; } }
	public override float inOutRatio { get { return 0.9f; } }
}
