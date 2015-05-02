using UnityEngine;
using System.Collections;

public class Lot : MonoBehaviour {
	public Site Site { get; set; }
	public Building Building { get; private set; }
	private GameObject building;
	private GameObject oldBuilding;
	public Business Owner { get; set; }
	public Resource? Resource { get; private set; }
	public GameObject InitialLotPlane;
	public GameObject LotPlane { get { return getLotPlane (); } set { setLotPlane (value); } }
	private GameObject LotPlane_;
	public Vector3 Location { get; set; }

	public Lot() {
		LotPlane = InitialLotPlane;
	}

	public void DemolishBuilding() {
		oldBuilding = building;
		DestroyBuilding();
		Building = null;
		building = null;
	}

	private void DestroyBuilding () {
		if (oldBuilding != null) {
			Object.Destroy(oldBuilding);
			oldBuilding = null;
		}
	}

	public BuildingT NewBuilding<BuildingT> (BuildingT newBuilding) where BuildingT : Building {
		Building = newBuilding;
		oldBuilding = building;
		building = (GameObject) Instantiate(newBuilding.building, LotPlane.transform.position, LotPlane.transform.rotation);
		DestroyBuilding ();
		return newBuilding;
	}

	public void setLotPlane(GameObject lp) {
		LotPlane_ = lp;
	}

	public GameObject getLotPlane() {
		return LotPlane_;
	}
}
