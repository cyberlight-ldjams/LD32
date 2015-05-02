using UnityEngine;
using System.Collections;

/**
 * Superclass for all buildings
 */
public abstract class Building : MonoBehaviour {

	/** Who owns this building */
	public Lot lot;

	/** The building game object */
	public GameObject building;

	public void Demolish() {
		Destroy (this.gameObject);
	}

	public void Make(Vector3 location) {
		Instantiate (building, location, Quaternion.identity);
	}
}
