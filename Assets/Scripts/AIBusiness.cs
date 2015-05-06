using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AIBusiness : Business {

	// // STANCES OF AI BUSINESSES // //

	public const int AGGRESSIVE = 0;

	public const int PASSIVE = 1;

	public const int NEUTRAL = 2;

	// // AFFINITIES FOR AI BUSINESSES (OPTIONAL) // //

	public const int NO_AFFINITY = 0;

	public const int WATER_AFFINITY = 1;

	public const int MINING_AFFINITY = 2;

	public const int FOREST_AFFINITY = 3;


	/** Creates an AI business to denote that a lot is unowned */
	public static AIBusiness UNOWNED = new AIBusiness(true);

	/** The business' stance */
	public int stance { get; private set; }

	/** The business' affinity */
	public int affinity { get; private set; }


	/**
	 * Creates a Business AI given a name, stance, and color
	 */
	public AIBusiness(string name, int stance, Color color) {
		this.name = name;
		this.stance = stance;
		this.businessColor = color;
		GameDirector.setBusinessColor(color);
	}

	/**
	 * Creates a Business AI with a random name and a random stance
	 */
	public AIBusiness() {
		makeRandomBusiness();
	}

	/**
	 * Makes a business with a random name and stance and color
	 */
	private void makeRandomBusiness() {
		name = RandomNameGenerator.generateBusinessName();

		int rand1 = Random.Range(0, 3);
		stance = rand1;
		
		businessColor = GameDirector.getBusinessColor();
	}

	/**
	 * Creates an "UNOWNED" business
	 */
	private AIBusiness(bool unowned) {
		if (unowned) {
			name = "Unowned";
			stance = NEUTRAL;

			// Color is a light gray
			businessColor = new Color(0.8f, 0.8f, 0.8f);

			// Let the director know we used the color
			GameDirector.setBusinessColor(businessColor);
		} else {
			makeRandomBusiness();
		}
	}

	/**
	 * Method called on the failure of an AI business
	 */
	public void Failure() {
		//TODO : make this do something
	}
}
