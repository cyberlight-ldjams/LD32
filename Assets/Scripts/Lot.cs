using UnityEngine;
using System.Collections;

public class Lot {
	public Site Site { get; set; }
	public Building Building { get; private set; }
	private GameObject building;
	private GameObject oldBuilding;
	public Business Owner;
	public Resource? Resource { get; private set; }
	private GameObject LotPlane { get { return getLotPlane (); } set { setLotPlane (value); } }
	private GameObject LotPlane_;
	public Vector3 Location { get; set; }

	public Lot(Site site, Business owner, Vector3 position, Quaternion rotation) {
		Site = site;
		Owner = owner;
		LotPlane = (GameObject) Object.Instantiate (Resources.Load ("LotPlane"), position, rotation);
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
		building = (GameObject) Object.Instantiate(newBuilding.building, LotPlane.transform.position, LotPlane.transform.rotation);
		building.GetComponent<BuildingT>().lot = this;
		DestroyBuilding ();
		return newBuilding;
	}

	public static void MakeBuilding(GameObject selectedObject, Site site, Building b) {
		if (selectedObject != null && selectedObject.name.Contains("LotPlane")) {
			foreach (Lot l in site.Lots) {
				if (selectedObject == l.LotPlane) {
					l.NewBuilding(b);
					break;
				}
			}
		}
	}

	public void setLotPlane(GameObject lp) {
		LotPlane_ = lp;
	}

	public GameObject getLotPlane() {
		return LotPlane_;
	}
}
