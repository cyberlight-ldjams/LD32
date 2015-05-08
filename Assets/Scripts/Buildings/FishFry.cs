using UnityEngine;
using System.Collections;

/**
 * A fish fry is a restaurant where fish is fried up and served for dinner
 */
public class FishFry : Workshop {
	public override Resource resourceUsed { get { return Resource.Fish; } }
	public override Resource goodProduced { get { return Resource.Dinner; } }
	public override float inOutRatio { get { return 1.0f; } }
}
