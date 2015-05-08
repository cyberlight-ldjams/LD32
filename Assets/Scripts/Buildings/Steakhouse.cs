using UnityEngine;
using System.Collections;

/**
 * A steakhouse is a restaurant where meat is chopped up and cooked, usually grilled, to be served as dinner
 */
public class Steakhouse : Workshop {
	public override Resource resourceUsed { get { return Resource.Meat; } }
	public override Resource goodProduced { get { return Resource.Dinner; } }
	public override float inOutRatio { get { return 1.0f; } }
}
