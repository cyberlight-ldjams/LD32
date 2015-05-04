using UnityEngine;
using System.Collections;
using System;

public class Sales {

	public const int LOT_DOWN_PAYMENT = 1000;

	public const int LOT_LEASE_SELL = 400;

	public const int LOT_LEASE_COST = 100;

	public const int LOT_LEASE_DUE = 4; // in quarters

	public const int BUILD_COST = 500;

	public const int BUILD_SELL = 150;

	public int [] totalSales { get; private set; }

	private GameTime quarters;

	private readonly Demand demand;

	private GameDirector director;

	public Sales(GameDirector gd) {
		director = gd;
		demand = director.stager.demand;
		resetSales ();
	
		quarters = GameDirector.gameTime;
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

	public bool leaseLot(Business leasee, Lot toLease) {
		if (toLease.Owner != AIBusiness.UNOWNED) {
			Debug.LogWarning ("Lot already owned");
			return false;
		}
		if (!checkMoney(leasee, LOT_DOWN_PAYMENT)) {
			Debug.LogWarning ("Lot too expensive");
			return false;
		} else {
			leasee.myInventory.spendBaseCurrency(LOT_DOWN_PAYMENT);
			toLease.leaseID = quarters.performActionRepeatedly (LOT_LEASE_DUE, () => { if (leasee.myInventory.spendBaseCurrency(LOT_LEASE_COST) > 0) return true; else return false;});
			return true;
		}
	}

	public void sellLease(Business leasee, Lot toLease) {
		if (leasee == toLease.Owner) {
			leasee.myInventory.addBaseCurrency(LOT_LEASE_SELL);
			quarters.disable(toLease.leaseID);
			toLease.leaseID = 0;

			sellBuilding(leasee, toLease.Building);
		}
	}

	private void resetSales() {
		int resourceCount = Enum.GetValues(typeof(Resource)).Length;
		totalSales = new int[resourceCount];
		for (int i = 0; i < resourceCount; i++) {
			totalSales [i] = 0;
		}
	}

	private bool checkMoney(Business buyer, int amount) {
		return buyer.myInventory.getBaseCurrency() >= amount;
	}

	public bool buyBuilding(Business buyer, Lot lot, Building build) {
		if (buyer != lot.Owner) {
			Debug.LogWarning ("Lot not owned by business");
			return false;
		}
		if (!checkMoney(buyer, BUILD_COST)) {
			Debug.LogWarning ("Building too expensive");
			return false;
		} else {
			buyer.myInventory.spendBaseCurrency(BUILD_COST);
			lot.InstallBuildingAt(build);
			return true;
		}
	}

	public void sellBuilding(Business buyer, Building build) {
		if (buyer == build.lot.Owner) {
			buyer.myInventory.addBaseCurrency(BUILD_SELL);
			build.Demolish();
		}
	}
}
