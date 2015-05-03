using UnityEngine;
using System.Collections;

public class Steakhouse : Workshop {
	public override Resource resourceUsed { get { return Resource.Meat; } }
	public override Resource goodProduced { get { return Resource.Dinner; } }
}
