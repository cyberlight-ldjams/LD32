using UnityEngine;
using System.Collections;

/**
 * A furniture workshop takes timber and makes it into furniture
 */
public class FurnitureWorkshop : Workshop {
	public override Resource resourceUsed { get { return Resource.Timber; } }
	public override Resource goodProduced { get { return Resource.Furniture; } }
	public override float inOutRatio { get { return 0.9f; } }
}