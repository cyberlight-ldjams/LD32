using UnityEngine;
using System.Collections;

/**
 * An oil derrik pumps oil out of the ground
 */
public class OilDerrik : Quarry {
	public override Resource resourceProduced { get { return Resource.Oil; } }
}
