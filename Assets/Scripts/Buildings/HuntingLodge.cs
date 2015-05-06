using UnityEngine;
using System.Collections;

/**
 * A hunting lodge is a place where hunters gather to go and hunt, so it produces meat
 */
public class HuntingLodge : Quarry {
	public override Resource resourceProduced { get { return Resource.Meat; } }
}