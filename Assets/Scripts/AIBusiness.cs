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

	public string name { get; private set; }

	public int stance { get; private set; }

	/**
	 * Creates a Business AI with a random name and a random stance
	 */
	public AIBusiness() {
		name = "";

		string[] types = {"Corp.", "Factories", "Firm", "Institute", "Venture", "Cartel", "Syndicate", "Trust", "Megacorp", "Mom and Pop", "Business", "Stores"};
		string[] ownerFirst = {"Elmer", "Rene", "Victor", "Pamela", "Rosemarie", "Claude", "Sherri", "Al", "Cornelius", "Clyde", "Malcolm", "Oscar", "Jerome", "Elsa", "Donald", "Devin" };
		string[] ownerLast = {"Cobb", "Saunders", "Huff", "Wilkerson", "Barker", "Beck", "Reed", "Valdez", "Jenkins", "Delgado", "Webb", "Horton", "Larson", "Cooper", "Day", "Christensen" };

		int rand1 = Random.Range(0,types.Length);
		int rand2 = Random.Range(0,ownerFirst.Length);
		int rand3 = Random.Range(0,ownerLast.Length);

		name = ownerFirst[rand2] + " " + ownerLast[rand3] + " " + types[rand1];
		Debug.Log(name);

		rand1 = Random.Range(0,5);

		stance = rand1;
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
