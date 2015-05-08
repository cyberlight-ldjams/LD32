using UnityEngine;
using System.Collections;

/**
 * A lot is a place at a site that can be leased and built upon
 * 
 * Some lots have resources on them, some do not
 */
public class Lot {

	/** The site this lot is on */
	public Site Site { get; set; }

	/** The building on this lot */
	public Building Building { get; private set; }

	/** This lot's owner - technically leasee */
	public Business Owner { get; set; }

	/** The resource on this lot, if there is one */
	public Resource? Resource { get { return Resource_; } set { SetResource(value); } }

	/** The private reference to the resource on this lot */
	private Resource? Resource_;

	/** This lot's LotPlane game object */
	private GameObject LotPlane;

	/** This lot's location on the site */
	public Vector3 Location { get; set; }

	/** The lease ID for this lot, used with the game timer */
	public int leaseID { get; set; }

	/**
	 * Creates a lot given its site, owner, position, and rotation
	 * 
	 * @param site the site this lot is on
	 * @param owner the owner of this lot
	 * @param position the position of this lot on the site
	 * @param rotation the rotation of this lot
	 */
	public Lot(Site site, Business owner, Vector3 position, Quaternion rotation) {
		Site = site;
		Owner = owner;
		LotPlane = (GameObject)Object.Instantiate(Resources.Load("LotPlane"), position, rotation);
		leaseID = 0;
	}

	/**
	 * Installs a building on the lot
	 * 
	 * @param newBuilding the building to install
	 */
	public void InstallBuildingAt(Building newBuilding) {
		// If there is already a building on this lot, demolish it
		if (Building != null) {
			Building.Demolish();
		}

		// If no one owns this lot, don't build on it
		if (Owner == AIBusiness.UNOWNED) {
			return;
		}

		// Install the building
		Building = newBuilding;
		Building.Install(this, LotPlane.transform.position, LotPlane.transform.rotation);
	}

	/**
	 * Finds the lot object given a lot plane and its site
	 * 
	 * @param lotPlane the lot plane game object
	 * @param site the site this lot is on
	 * @return this lot object
	 */
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

	/**
	 * Installs a building on a lot given its LotPlane, site, and the building
	 * 
	 * @param selectedObject the LotPlane
	 * @param site the site the lot is on
	 * @param build the building to build on the lot
	 */
	public static void InstallBuilding(GameObject selectedObject, Site site, Building build) {
		Lot lot = FindLot(selectedObject, site);
		lot.InstallBuildingAt(build);
	}

	/**
	 * Set this lot to have the given resource
	 * 
	 * @param resource the resource this lot should have
	 */
	public void SetResource(Resource? resource) {
		Resource_ = resource;
		if (resource.HasValue) {
			LotPlane.GetComponent<Renderer>().material = resource.Value.Material();
		}
	}

	/**
	 * Checks if the given game object is this lot's LotPlane
	 * 
	 * @param otherObject the object to check against
	 * @return whether or not the given object is this lot's lot plane
	 */
	public bool LotPlaneIs(GameObject otherObject) {
		return LotPlane == otherObject;
	}

	/**
	 * Repositions this lot's LotPlane
	 * 
	 * @param newPosition the new position for this LotPlane
	 */
	public void RepositionLotPlane(Vector3 newPosition) {
		LotPlane.transform.position = newPosition;
	}
}
