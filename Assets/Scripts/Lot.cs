using UnityEngine;
using System.Collections;

public class Lot {
	public Site Site { get; set; }
	public Building Building { get; private set; }
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

	public void InstallBuildingAt(Building newBuilding) {
		if (Building != null)
			Building.Demolish ();
		
		Building = newBuilding;
		Building.Install (this, LotPlane.transform.position, LotPlane.transform.rotation);
	}

	public static void InstallBuilding(GameObject selectedObject, Site site, Building b) {
		if (selectedObject != null && selectedObject.name.Contains("LotPlane")) {
			foreach (Lot l in site.Lots) {
				if (selectedObject == l.LotPlane) {
					l.InstallBuildingAt (b);
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
