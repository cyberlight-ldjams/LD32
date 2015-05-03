using UnityEngine;
using System.Collections;

/**
 * Superclass for all quarries, 
 */
public abstract class Quarry : Building {

	/** The resource this quarry produces */
	public abstract Resource resourceProduced { get; }

	public void Update () {
		double oldAmount = owner.myInventory.getAmountOfAt(resourceProduced, site);
		owner.myInventory.setAmountOfAt(resourceProduced, site, oldAmount + 0.1);
	}

}