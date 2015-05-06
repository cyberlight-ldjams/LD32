using UnityEngine;
using System.Collections;

/**
 * A pottery workshop is a workshop where clay is turned into pottery
 */
public class PotteryWorkshop : Workshop {
	public override Resource resourceUsed { get { return Resource.Clay; } }
	public override Resource goodProduced { get { return Resource.Pottery; } }
	public override float inOutRatio { get { return 0.8f; } }

	protected override string prefabName { get { return "PotteryWorkshop"; } }
}
