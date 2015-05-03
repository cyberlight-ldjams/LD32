using UnityEngine;
using System.Collections;

public class ChipFactory : Workshop {
	public override Resource resourceUsed { get { return Resource.Stone; } }
	public override Resource goodProduced { get { return Resource.ComputerChip; } }	
	public override float inOutRatio { get { return 0.1f; } }
}