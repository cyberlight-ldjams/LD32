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
		double oldAmountOutput = owner.myInventory.getAmountOfAt(goodProduced, site);

		// If we have enough resources to produce at the maximum level, do that
		if (oldAmountInput >= inAmount) {
			owner.myInventory.setAmountOfAt(resourceUsed, site, oldAmountInput - inAmount);
			owner.myInventory.setAmountOfAt(goodProduced, site, oldAmountOutput + (productionRate * inAmount));
		} 

		// Otherwise, use up all the rest of the resource
		else {
			owner.myInventory.setAmountOfAt(resourceUsed, site, 0);
			owner.myInventory.setAmountOfAt(goodProduced, site, oldAmountOutput + (productionRate * oldAmountInput));
		}
	}

	public static Workshop NewAppropriateWorkshop(Resource resourceUsed) {
		switch (resourceUsed) {
			case Resource.Clay:
				return new PotteryWorkshop();
			case Resource.Oil:
				return new LampMaker();
			case Resource.Meat:
				return new Steakhouse();
			case Resource.Iron:
				return new WeaponSmith();
			case Resource.Stone:
				return new ChipFactory();
			case Resource.Fish:
				return new FishFry();
			case Resource.Timber:
				return new FurnitureWorkshop();
			default:
				throw new ArgumentException(string.Format("No workshop for resource {0}", resourceUsed));
		}
	}
}