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
	
		quarters.performActionRepeatedly (1, () => { demand.addQuarterSales(totalSales); resetSales(); });
	}

	public bool sell (Business seller, Site site, Resource r, int quantity, Business customer = null) {

		Inventory sellerInv = seller.myInventory;

		//Do we have enough
		if (sellerInv.getAmountOfAt (r, site) < quantity) {
			Debug.LogWarning ("Trying to sell more than we have");
			return false;
		}

		sellerInv.setAmountOfAt (r, site, quantity);
		float profit = demand.getPrice (r) * quantity;

		sellerInv.addBaseCurrency (profit);
		totalSales [r] = totalSales [r] + quantity;

		return true;
	}

	private void resetSales() {
		int resourceCount = Enum.GetValues (Resource).Length;
		totalSales = new int[resourceCount];
		for (int i = 0; i < resourceCount; i++) {
			totalSales [i] = 0;
		}
	}
}
