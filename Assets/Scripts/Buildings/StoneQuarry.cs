using UnityEngine;
using System.Collections;

/**
 * A stone quarry is a quarry where stone is collected
 */
public class StoneQuarry : Quarry {
	public override Resource resourceProduced { get { return Resource.Stone; } }
}