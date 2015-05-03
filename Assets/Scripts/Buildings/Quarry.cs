using UnityEngine;
using System.Collections;

/**
 * Superclass for all quarries, 
 */
public abstract class Quarry : Building {

	/** The resource this quarry produces */
	public abstract Resource resourceProduced { get; }

	public override void Produce () {
		double oldAmount = owner.myInventory.getAmountOfAt(resourceProduced, site);
		owner.myInventory.setAmountOfAt(resourceProduced, site, oldAmount + 0.1);
	}

}