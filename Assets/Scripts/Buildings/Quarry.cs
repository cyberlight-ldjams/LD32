using UnityEngine;
using System;
using System.Collections;

/**
 * Superclass for all quarries, 
 */
public abstract class Quarry : Building {

	/** The resource this quarry produces */
	public abstract Resource resourceProduced { get; }

	public virtual double productionRate { get { return 0.1 * Math.Sqrt((double)employees); } }

	public override void Produce () {
		double oldAmount = owner.myInventory.getAmountOfAt(resourceProduced, site);
		owner.myInventory.setAmountOfAt(resourceProduced, site, oldAmount + productionRate);
	}

}