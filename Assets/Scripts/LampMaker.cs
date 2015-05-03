using UnityEngine;
using System.Collections;

public class LampMaker : Workshop {
	public override Resource resourceUsed { get { return Resource.Oil; } }
	public override Resource goodProduced { get { return Resource.Lamp; } }
	public override float inOutRatio { get { return 0.5f; } }
}