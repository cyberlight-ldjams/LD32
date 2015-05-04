using UnityEngine;
using System.Collections;
using System;

public class Sales {

	public int [] totalSales { get; private set; }

	private GameTime<bool> quarters;

	private readonly Demand demand;

	private GameDirector director;

	public Sales(GameDirector gd) {
		director = gd;
		demand = director.stager.demand;
		resetSales ();
	
		quarters = new GameTime<bool>();
		quarters.performActionRepeatedly (1, () => { demand.addQuarterSales(totalSales); resetSales(); return true;});
	}

	public bool sell (Business seller, Site site, Resource r, int quantity, Business customer = null) {

		Inventory sellerInv = seller.myInventory;

		//Do we have enough
		if (sellerInv.getAmountOfAt (r, site) < quantity) {
			Debug.LogWarning ("Trying to sell more than we have");
			return false;
		}

		int rLoc = (int) r;

		sellerInv.setAmountOfAt (r, site, quantity);
		float profit = demand.getPrice (r) * quantity;

		sellerInv.addBaseCurrency (profit);
		totalSales [rLoc] = totalSales [rLoc] + quantity;

		return true;
	}

	private void resetSales() {
		int resourceCount = Enum.GetValues(typeof(Resource)).Length;
		totalSales = new int[resourceCount];
		for (int i = 0; i < resourceCount; i++) {
			totalSales [i] = 0;
		}
	}
}
