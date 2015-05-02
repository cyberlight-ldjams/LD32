using UnityEngine;
using System.Collections;

/**
 * Superclass for all buildings
 */
public abstract class Building : MonoBehaviour {

	/** Who owns this building */
	public int lot; //TODO: change int to lot

	public void Demolish() {
		Destroy (this.gameObject);
	}
}
