using UnityEngine;
using System.Collections;

/**
 * A gold mine is a quarry that mines gold
 */
public class GoldMine : Quarry {
	public override Resource resourceProduced { get { return Resource.Gold; } }
}