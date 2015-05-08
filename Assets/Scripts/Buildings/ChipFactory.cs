using UnityEngine;
using System.Collections;

/**
 * The chip factory, a workshop that produces computer chips when given stone by extracting silicon
 */
public class ChipFactory : Workshop {
	public override Resource resourceUsed { get { return Resource.Stone; } }
	public override Resource goodProduced { get { return Resource.ComputerChip; } }	
	public override float inOutRatio { get { return 0.1f; } }
}