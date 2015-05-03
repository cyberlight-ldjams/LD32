using UnityEngine;
using System.Collections;

public class ClayPit : Quarry {
	public override Resource resourceProduced {	get { return Resource.Clay; } }

	protected override string prefabName { get { return "ClayPit"; } }
}
