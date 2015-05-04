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

	public static Quarry NewAppropriateQuarry(Resource resource) {
		switch (resource) {
		case Resource.Clay:
			return new ClayPit();
		case Resource.Oil:
			return new OilDerrik();
		case Resource.Meat:
			return new HuntingLodge();
		case Resource.Iron:
			return new IronMine();
		case Resource.Stone:
			return new StoneQuarry();
		case Resource.Fish:
			return new Wharf();
		case Resource.Gold:
			return new GoldMine();
		case Resource.Timber:
			return new Timberyard();
		default:
			throw new ArgumentException(string.Format("No quarry for resource {0}", resource));
		}
	}

}