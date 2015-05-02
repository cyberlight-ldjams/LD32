using UnityEngine;
using System.Collections;

public class Lot : MonoBehaviour {
	public Site Site { get; set; }
	public Building Building { get; private set; }
	public Business Owner { get; set; }
	public Resource? Resource { get; private set; }
	public GameObject LotPlane { get; set; }
	public Vector3 Location { get; set; }

	public void DestroyBuilding () {
		if (Building != null) {
			Building.Demolish ();
			Building = null;
		}
	}

	public BuildingT NewBuilding<BuildingT> () where BuildingT : Building, new() {
		DestroyBuilding ();

		BuildingT newBuilding = new BuildingT ();
		Building = newBuilding;
		return newBuilding;
	}
}
