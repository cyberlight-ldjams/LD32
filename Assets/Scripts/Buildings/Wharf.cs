using UnityEngine;
using System.Collections;

/**
 * A wharf is a place where ships are sent out to gather fish
 */
public class Wharf : Quarry {
	public override Resource resourceProduced { get { return Resource.Fish; } }
}
