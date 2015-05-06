using UnityEngine;
using System.Collections;

/**
 * The player
 */
public class PlayerBusiness : Business {

	/**
	 * Initializes the player business
	 */
	public PlayerBusiness() {
		Init();

		// For now, set a random color and name
		businessColor = GameDirector.getBusinessColor();
		name = RandomNameGenerator.generateBusinessName();
	}
}
