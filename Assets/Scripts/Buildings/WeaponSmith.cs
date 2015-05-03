using UnityEngine;
using System.Collections;

public class WeaponSmith : Workshop {
	public override Resource resourceUsed { get { return Resource.Iron; } }
	public override Resource goodProduced { get { return Resource.Weapon; } }
}