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

	/** The name of the Prefab representing this building */

	protected virtual string prefabName { get { return "Building"; } }

	public int employees = 0;

	public void Demolish() {
		Object.Destroy (building);
	}

	public void Install(Lot lot, Vector3 location, Quaternion rotation) {
		this.lot = lot;
		building = (GameObject) Object.Instantiate (Resources.Load (prefabName), location, rotation);
		BuildingBehavior behavior = building.AddComponent<BuildingBehavior>();
		behavior.Building = this;
	}

	/** Produce whatever it is this building produces. Call this every tick. */
	public virtual void Produce() { }
}
