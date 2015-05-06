using UnityEngine;
using System.Collections;

/**
 * The clay pit, a quarry that collects clay from a pit
 */
public class ClayPit : Quarry {
	public override Resource resourceProduced { get { return Resource.Clay; } }

	protected override string prefabName { get { return "ClayPit"; } }
}
