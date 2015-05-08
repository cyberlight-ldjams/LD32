using UnityEngine;
using System.Collections;

/**
 * Superclass for all buildings
 */
public abstract class Building {

	/** Who owns this building */
	public Lot lot;

	public Business owner { get { return lot.Owner; } }
	public Site site { get { return lot.Site; } }

	/** The building game object */
	private GameObject building;

	/** The building id from the gametime element */
	public int buildingID { get; set; }

	/** The max employees the building takes */
	public int laborCap { get; set; }

	/** The name of the Prefab representing this building */
	protected virtual string prefabName { get { return "Building"; } }

	/** The number of employees currently in the building */
	public int employees = 0;

	/** The wage of the employees in the building */
	public int employeeWage = 0;

	/**
	 * Destroys the building
	 * 
	 * Also makes sure to get the workers to leave before the building is destroyed
	 * You know, to prevent lawsuits and such
	 */
	public void Demolish() {
		int siteEmployees = owner.myInventory.GetEmployeesAt(site);
		lot.Site.employees = siteEmployees + employees;
		lot.Building = null;
		Object.Destroy(building);
	}

	/**
	 * Installs this building at a lot
	 * 
	 * @param lot the lot to install the building at
	 * @param location the location to install the building
	 * @param rotation the rotation of the building on the lot
	 */
	public void Install(Lot lot, Vector3 location, Quaternion rotation) {
		this.lot = lot;
		building = (GameObject)Object.Instantiate(Resources.Load(prefabName), location, rotation);
		BuildingBehavior behavior = building.AddComponent<BuildingBehavior>();
		behavior.Building = this;
	}

	/** Produce whatever it is this building produces. Call this every tick. */
	public virtual void Produce() {
	}
}
