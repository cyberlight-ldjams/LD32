using UnityEngine;
using System.Collections;
using System;

/**
 * Controls the sale of items in the marketplace, i.e. all sales in the game
 */
public class Sales {

	/** The down payment to begin leasing a lot */
	public const int LOT_DOWN_PAYMENT = 1000;

	/** The amount of money received from selling a lease */
	public const int LOT_LEASE_SELL = 400;

	/** The yearly cost of leasing a lot */
	public const int LOT_LEASE_COST = 100;

	/** Currently the lease is due once a year, changing this value changes that */
	public const int LOT_LEASE_DUE = 4; // in quarters

	/** The amount a building costs to make */
	public const int BUILD_COST = 500;

	/** The amount a building is worth if sold */
	public const int BUILD_SELL = 150;

	/** The total sales that have occured this quarter - should probably become a dictionary */
	public int [] totalSales { get; private set; }

	/** The game timer */
	private GameTime gameTimer;

	/** The game's demand element */
	private readonly Demand demand;

	/** The game director */
	private GameDirector director;

	/**
	 * Creates a sales object for the game director
	 * 
	 * @param gd the game director this object will be used by
	 */
	public Sales(GameDirector gd) {
		director = gd;
		demand = director.stager.demand;
		resetSales();
	
		gameTimer = GameDirector.gameTime;
		gameTimer.performActionRepeatedly(1, () => {
			demand.addQuarterSales(totalSales);
			resetSales();
			return true;});
	}

	/**
	 * Sells resources at a given site
	 * 
	 * 
	 * 
	 * @param seller the business selling the resources
	 * @param site the site where the resources are held for sale
	 * @param r the resource being sold
	 * @param quantity the amount of resources sold
	 * @param customer optional, used if the sale was to another business instead of to consumers
	 * @param perUnit the amount the other business offered per item
	 * @return whether the sale was successful (there was enough inventory to sell and/or the other business had enough money)
	 */
	public bool sell(Business seller, Site site, Resource r, int quantity, Business customer = null, float perUnit = 0.0f) {

		Inventory sellerInv = seller.myInventory;

		// Do we have enough?
		if (sellerInv.getAmountOfAt(r, site) < quantity) {
			Debug.LogWarning("Trying to sell more than we have");
			return false;
		}

		// If this is a B2B transaction...
		if (customer != null) {
			if (perUnit <= 0.0f) {
				Debug.LogWarning("You can't get somethin' for nothin'");
				return false;
			}

			float totalProfit = quantity * perUnit;
			if (checkMoney(customer, totalProfit)) {
				Debug.LogWarning("The other business doesn't have enough money");
				return false;
			}

			sellerInv.setAmountOfAt(r, site, sellerInv.getAmountOfAt(r, site) - quantity);
			customer.myInventory.setAmountOfAt(r, site, customer.myInventory.getAmountOfAt(r, site) - quantity);
			sellerInv.addBaseCurrency(totalProfit);
			customer.myInventory.spendBaseCurrency(totalProfit);

			// B2B sales don't impact the market demand, so don't add anything to the total sales count
			return true;
		}

		// Sell the items in the marketplace, using demand to get the price
		sellerInv.setAmountOfAt(r, site, quantity);

		float profit = demand.getPrice(r) * quantity;
		sellerInv.addBaseCurrency(profit);

		// The integer value of the resource to be used to get its position in the array
		int rLoc = (int)r;

		// Log the sales
		totalSales [rLoc] = totalSales [rLoc] + quantity;

		return true;
	}

	/**
	 * Leases a lot
	 * 
	 * @param leasee the business leasing the lot
	 * @param toLease the lot to be leased
	 */
	public bool leaseLot(Business leasee, Lot toLease) {
		if (toLease.Owner != AIBusiness.UNOWNED) {
			Debug.LogWarning("Lot already owned");
			return false;
		}
		if (!checkMoney(leasee, LOT_DOWN_PAYMENT)) {
			Debug.LogWarning("Lot too expensive");
			return false;
		} else {
			leasee.myInventory.spendBaseCurrency(LOT_DOWN_PAYMENT);
			toLease.leaseID = gameTimer.performActionRepeatedly(LOT_LEASE_DUE, () => {
				if (leasee.myInventory.spendBaseCurrency(LOT_LEASE_COST) > 0) {
					return true;
				} else {
					return false;
				}
			});
			toLease.Owner = leasee;
			leasee.myLots.Add(toLease);
			return true;
		}
	}

	/**
	 * Sells a lease back
	 * 
	 * @param leasee the business selling its lease
	 * @param toLease the lot being leased back
	 */
	public void sellLease(Business leasee, Lot toLease) {
		if (leasee == toLease.Owner) {
			leasee.myInventory.addBaseCurrency(LOT_LEASE_SELL);
			gameTimer.disable(toLease.leaseID);
			toLease.leaseID = 0;

			sellBuilding(leasee, toLease.Building);

			leasee.myLots.Remove(toLease);
			toLease.Owner = AIBusiness.UNOWNED;
		}
	}

	/**
	 * Resets totalSales at the end of a quarter
	 */
	private void resetSales() {
		int resourceCount = Enum.GetValues(typeof(Resource)).Length;
		totalSales = new int[resourceCount];
		for (int i = 0; i < resourceCount; i++) {
			totalSales [i] = 0;
		}
	}

	/**
	 * Checks if a business has enough money for something
	 * 
	 * @param buyer the business
	 * @param amount the amount of money to check against
	 * @return whether the business has at least that amount of money
	 */
	private bool checkMoney(Business buyer, float amount) {
		return buyer.myInventory.getBaseCurrency() >= amount;
	}

	/**
	 * Buys a building for a business at a lot if the business has enough money
	 * 
	 * @param buyer the business buying the building
	 * @param lot the lot the building will go on
	 * @param build the building to buy
	 * @return whether the sale went through
	 */
	public bool buyBuilding(Business buyer, Lot lot, Building build) {
		if (buyer != lot.Owner) {
			Debug.LogWarning("Lot not owned by business");
			return false;
		}
		if (!checkMoney(buyer, BUILD_COST)) {
			Debug.LogWarning("Building too expensive");
			return false;
		} else {
			buyer.myInventory.spendBaseCurrency(BUILD_COST);
			lot.InstallBuildingAt(build);
			return true;
		}
	}

	/**
	 * Sells a building
	 * 
	 * @param buyer the business selling the building
	 * @param build the building being sold
	 */
	public void sellBuilding(Business buyer, Building build) {
		if (build != null && buyer == build.lot.Owner) {
			buyer.myInventory.addBaseCurrency(BUILD_SELL);
			build.Demolish();
		}
	}
}
