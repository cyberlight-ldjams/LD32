using UnityEngine;
using System.Collections;

public class PotteryWorkshop : Workshop {
	public override Resource resourceUsed { get { return Resource.Clay; } }
	public override Resource goodProduced { get { return Resource.Pottery; } }
	public override float inOutRatio { get { return 0.8f; } }

	protected override string prefabName { get { return "PotteryWorkshop"; } }
}
