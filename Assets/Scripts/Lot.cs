using UnityEngine;
using System.Collections;

public class Lot : MonoBehaviour {
	public Site Site { get; private set; }
	public Building Building { get; private set; }
	public Business Owner { get; set; }
	public Resource? Resource { get; private set; }

	public Lot (Site site) {
		Site = site;
	}

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

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
