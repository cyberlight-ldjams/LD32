using UnityEngine;
using System;
using System.Collections;

/**
 * Superclass for all workshops
 */
public abstract class Workshop : Building {

	/** The resource this workshop uses */
	public abstract Resource resourceUsed { get; }

	/** The good this workshop produces */
	public abstract Resource goodProduced { get; }

	/** The goods out per goods put in */
	public abstract float inOutRatio { get; }

	/** The rate at which resources are produced, given employees */
	public virtual double productionRate { get { return 0.1 * Math.Sqrt((double)employees); } }

	/**
	 * If enough input resources exist, the workshop produces things
	 */
	public override void Produce() {
		double inAmount = productionRate / inOutRatio;

		double oldAmountInput = owner.myInventory.getAmountOfAt(resourceUsed, site);

		if (oldAmountInput >= inAmount) {
			owner.myInventory.setAmountOfAt(resourceUsed, site, oldAmountInput - inAmount);

			double oldAmountOutput = owner.myInventory.getAmountOfAt(goodProduced, site);
			owner.myInventory.setAmountOfAt(goodProduced, site, oldAmountOutput + productionRate);
		}
	}
}