using UnityEngine;
using System.Collections;

/**
 * The behavior of buildings, i.e. to produce resources with employees
 */
public class BuildingBehavior : MonoBehaviour {

	/** The building */
	public Building Building;

	/**
	 * If the building exists, and the game isn't paused, produce things
	 */
	public void Update() {
		// // PAUSED BEHAVIOR // //
		if (GameDirector.PAUSED) {
			return; // Don't produce things
		}

		// Make sure the building exists, if it doesn't, it doesn't have a behavior
		// So it shouldn't be producing things... probably
		if (Building != null) {
			Building.Produce();
		}
	}

}