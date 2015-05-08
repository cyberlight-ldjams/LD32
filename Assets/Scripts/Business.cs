using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/**
 * Superclass of all businesses
 */
public abstract class Business {

	/** How often, in quarters, payday for employees occurs */
	public const int PAYDAY = 1;

	/** The default starting currency in base currency units */
	public const double STARTING_CURRENCY = 10000.0;

	/** The inventory of the business */
	public Inventory myInventory;

	/** The color of the business, used to color the selection plane */
	public Color businessColor { get; protected set; }

	/** The lots leased by the business */
	public List<Lot> myLots { get; private set; }

	/** The name of the business */
	public string name { get; protected set; }

	/** Points to the GameDirector's sales unit */
	private Sales sales;

	/** 
	 * Initializes a business with an inventory and reference to sales 
	 */
	public void Init() {
		myInventory = new Inventory(genericCurrency: STARTING_CURRENCY);
		sales = GameDirector.THIS.sales;
		myLots = new List<Lot>();

		// Set up payday for employees
		GameDirector.gameTime.performActionRepeatedly(PAYDAY, () => {
			foreach (Lot l in myLots) {
				if (l.Building != null) {
					Building b = l.Building;
					if (this.myInventory.spendBaseCurrency(b.employees * b.employeeWage) < 0) {
						// If you can't pay your employees, they take what they can get and leave
						this.myInventory.addBaseCurrency(Mathf.Abs((float)this.myInventory.getBaseCurrency()));
						l.Site.employees += b.employees;
						b.employees = 0;
						b.laborCap = 0;
						b.employeeWage = 0;
					}
				}
			}
			if (this.myInventory.getBaseCurrency() > 0) {
				return true;
			} else {
				return false;
			}
		});
	}

	/**
	 * Leases a lot
	 * 
	 * @param toLease the lot to be leased
	 */
	public void LeaseLot(Lot toLease) {
		sales.leaseLot(this, toLease);
	}

	/**
	 * Sells an amount of resources at a given site
	 * 
	 * @param s the site
	 * @param r the resource to sell
	 * @param qty the amount to sell
	 */
	public void SellResource(Site s, Resource r, int qty) {
		sales.sell(this, s, r, qty);
	}

	/**
	 * Buys a building on the given lot
	 * 
	 * @param lot the lot to buy the building on
	 * @param build the building to buy
	 */
	public void BuyBuilding(Lot lot, Building build) {
		if (sales.buyBuilding(this, lot, build)) {

		}
	}

	/**
	 * Transports goods from one site to another
	 * 
	 * @param r the resource to transport
	 * @param qty the amount to send
	 * @param a the site to send from
	 * @param b the site to send to
	 */
	public bool TransportGoods(Resource r, double qty, Site a, Site b) {
		if (myInventory.getAmountOfAt(r, a) >= qty) {
			myInventory.setAmountOfAt(r, a, -qty);

			//TODO: TRANSPORT TIME

			myInventory.setAmountOfAt(r, b, qty);
			return true;
		} else {
			return false;
		}
	}

	/**
	 * Sets the labor cap on a building
	 * 
	 * @param b the building
	 * @param amount the labor cap
	 */
	public void setLaborCap(Building b, int amount) {
		b.laborCap = amount;
	}

	/**
	 * Sets the wage for a building
	 * 
	 * @param b the building
	 * @param amount the wage in currency per quarter
	 */
	public void setWage(Building b, int amount) {
		b.employeeWage = amount;
	}
}