using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AIBusiness : Business {

	public const int AGGRESSIVE = 0;

	public const int PASSIVE = 1;

	public const int NEUTRAL = 2;

	public const int WATER_AFFINITY = 3;

	public const int MINING_AFFINITY = 4;

	public const int FOREST_AFFINITY = 5;

	public static AIBusiness UNOWNED = new AIBusiness(0);

	public int stance { get; private set; }

	/**
	 * Creates a Business AI with a random name and a random stance
	 */
	public AIBusiness() {
		name = RandomNameGenerator.generateBusinessName();

		float rand1 = Random.Range(0,6);

		stance = (int) rand1;
	}

	private AIBusiness(int i) {
		name = "Unowned";
		stance = NEUTRAL;
	}

	/**
	 * Method called on the failure of an AI business
	 */
	public void Failure() {

	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
