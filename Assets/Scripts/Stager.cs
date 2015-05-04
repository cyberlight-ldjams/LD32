using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Stager {

	public Stage currentStage { get; private set; }

	public Demand demand { get; private set; }

	// Determine what resources can be produced based on the age

	public List<Resource> available;

	public Stager() {
		currentStage = Stage.Archaic;
		demand = new Demand();
		demand.generateAgeDemand(currentStage);
		available = new List<Resource>();
	}

	/**
	 * Increments the current stage and updates demand for the next age
	 */
	public Stage nextStage() {
		currentStage++; // Go to the next stage
		demand.generateAgeDemand(currentStage);




		return currentStage;
	}

	public List<Resource> availableResources() {
		return available;
	}
}