using UnityEngine;
using System.Collections;

public class Lot {
	public Site Site { get; set; }
	public Building Building { get; private set; }
	public Business Owner { get; set; }
	public Resource? Resource { get { return Resource_; } set { SetResource(value); } }
	private Resource? Resource_;
	private GameObject LotPlane;
	public Vector3 Location { get; set; }
	public int leaseID { get; set; }

	public Lot(Site site, Business owner, Vector3 position, Quaternion rotation) {
		Site = site;
		Owner = owner;
		LotPlane = (GameObject)Object.Instantiate(Resources.Load("LotPlane"), position, rotation);
		leaseID = 0;
	}

	public void InstallBuildingAt(Building newBuilding) {
		if (Building != null) {
			Building.Demolish();
		}
		
		Building = newBuilding;
		Building.Install(this, LotPlane.transform.position, LotPlane.transform.rotation);
	}

	public static Lot FindLot(GameObject lotPlane, Site site) {
		if (lotPlane != null && lotPlane.name.Contains("LotPlane")) {
			foreach (Lot l in site.Lots) {
				if (lotPlane == l.LotPlane) {
					return l;
				}
			}
		}

		return null;
	}

	public static void InstallBuilding(GameObject selectedObject, Site site, Building b) {
		Lot l = FindLot(selectedObject, site);
		l.InstallBuildingAt(b);
	}

	public void SetResource(Resource? resource) {
		Resource_ = resource;
		if (resource.HasValue) {
			LotPlane.GetComponent<Renderer>().material = resource.Value.Material();
		}
	}

	public bool LotPlaneIs(GameObject otherObject) {
		return LotPlane == otherObject;
	}

	public void RepositionLotPlane(Vector3 newPosition) {
		LotPlane.transform.position = newPosition;
	}
}
