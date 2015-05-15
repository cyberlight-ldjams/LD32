using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

/**
 * The inventory of a business
 */
public class Inventory {

	/** the base currency, unmodified by any stage modifiers */
	private double genericCurrency;

	/** contains all valid resource items */
	private Dictionary<ItemKey, double> items; 

    /** The maximum amount of any resource that can be stored at a lot */
    private double maxResource = 800.0;

	/** Create a new inventory (only needed by business class) */
	public Inventory(double genericCurrency) {

		this.genericCurrency = genericCurrency;
		this.items = new Dictionary<ItemKey, double>();

	}

	/** get the items in your inventory
	 * @return a list of <Item>s
	 */
	private Dictionary<ItemKey, double> getInventory() {
		return items;
	}

	public double getBaseCurrency() {
		return genericCurrency;
	}

	/** 
	 * Modifies the currency currently held by the business
	 * @param double change the currency to subtract (can be postive to add funds)
	 * @return the amount of currency the business has after the change
	 */
	public double spendBaseCurrency(double change) {
		addBaseCurrency(-change);
		return getBaseCurrency();
	}

	/** 
	 * Modifies the currency currently held by the business
	 * @param double change the currency to add (can be negative to subtract funds)
	 */
	public void addBaseCurrency(double change) {
		genericCurrency = genericCurrency + change;
	}

	public double getAmountOfAt(Resource resource, Site site) {
		ItemKey tuple = new ItemKey { itemType = resource, location = site };
		if (items.ContainsKey(tuple)) {
			return items [tuple];
		} else {
			return 0.0;
		}
	}

	public void setAmountOfAt(Resource resource, Site site, double amount) {
		ItemKey tuple = new ItemKey { itemType = resource, location = site };
		items [tuple] = Math.Min(amount, maxResource);
	}

	/**
	 * Gets the number of employees at the site
	 */
	public int GetEmployeesAt(Site site) {
		return site.employees;
	}

	/**
	 * Sets the labor cap at the specified building to numEmployees
	 * 
	 * @param b the building
	 * @param numEmployees the labor cap
	 */
	public void SetEmployeesCap(Building b, int numEmployees) {
		if (numEmployees >= 0) {
			b.laborCap = numEmployees;
		} else {
			b.laborCap = 0;
		}
	}


	/**
	 * The structure of an item key
	 * A resource and its location at a site
	 */
	public struct ItemKey {
		public Resource itemType; 
		public Site location;
	}

}
