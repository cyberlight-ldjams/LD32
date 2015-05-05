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
	public static AIBusiness UNOWNED = new AIBusiness(0);

	/** The business' stance */
	public int stance { get; private set; }

	/** The business' affinity */
	public int affinity { get; private set; }

	/**
	 * Creates a Business AI with a random name and a random stance
	 */
	public AIBusiness() {
		name = RandomNameGenerator.generateBusinessName();

		float rand1 = Random.Range(0,3);

		stance = (int) rand1;
	}

	/**
	 * Creates an "UNOWNED" business
	 */
	private AIBusiness(int i) {
		name = "Unowned";
		stance = NEUTRAL;
	}

	/**
	 * Method called on the failure of an AI business
	 */
	public void Failure() {
		//TODO : make this do something
	}
}
