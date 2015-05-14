using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Stager {

	public Stage currentStage { get; private set; }

	public Demand demand { get; private set; }

	public List<Resource> available { get; private set; }

	public string currencyName { get; private set; }

	private string[] currencyNames = { "Pelts", "Coins", "Credits" };

	public Stager() {
		currentStage = Stage.Archaic;
		demand = new Demand();
		demand.generateAgeDemand(currentStage);
		available = new List<Resource>();
		currencyName = currencyNames[0];

		// Initially available resources
		available.Add(Resource.Brick);
		available.Add(Resource.Clay);
		available.Add(Resource.Fish);
		available.Add(Resource.Gold);
		available.Add(Resource.Meat);
		available.Add(Resource.Pottery);
		available.Add(Resource.Stone);
		available.Add(Resource.Timber);
	}

	/**
	 * Increments the current stage and updates demand for the next age
	 */
	public Stage nextStage() {
		currentStage++; // Go to the next stage
		demand.generateAgeDemand(currentStage);

		if (currentStage == Stage.Medieval) {
			currencyName = currencyNames[1];
			available.Add (Resource.Iron);
			available.Add (Resource.Weapon);
			available.Add (Resource.Furniture);
			available.Add (Resource.Dinner);
		} else if (currentStage == Stage.Renaissance) {
			currencyName = currencyNames[1];
			available.Add (Resource.Lamp);
			available.Add (Resource.Oil);
			available.Add (Resource.RailroadTie);
		} else if (currentStage == Stage.Machine) {
			currencyName = currencyNames[2];
			available.Add (Resource.Plastic);
			available.Add (Resource.ComputerChip);
		}

		return currentStage;
	}

	public List<Resource> availableResources() {
		return available;
	}
}