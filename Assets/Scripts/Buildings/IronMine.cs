using UnityEngine;
using System.Collections;

/**
 * An iron mine is a quarry where iron is mined for
 */
public class IronMine : Quarry {
	public override Resource resourceProduced { get { return Resource.Iron; } }
}