using UnityEngine;
using System.Collections;

/**
 * A timeryard is a place where timber is cut into lumber, but it produces timber because we don't need a chain of workshops for making lumber like the real world
 */
public class Timberyard : Quarry {
	public override Resource resourceProduced { get { return Resource.Timber; } }
}