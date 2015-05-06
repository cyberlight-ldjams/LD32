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
		businessColor = GameDirector.getBusinessColor();
	}
}
