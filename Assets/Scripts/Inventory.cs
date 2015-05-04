using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Inventory {



	/** the base currency, unmodified by any stage modifiers */
	private double genericCurrency;

	/** contains all valid resource items */
	private Dictionary<ItemKey, double> items; 

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
	 * @param double change the currency to add (can be negative to subtrack funds)
	 */
	public void addBaseCurrency(double change) {
		genericCurrency = genericCurrency + change;
	}

	public double getAmountOfAt(Resource resource, Site site) {
		ItemKey tuple = new ItemKey { itemType = resource, location = site };
		if (items.ContainsKey(tuple))
			return items[tuple];
		else
			return 0.0;
	}

	public void setAmountOfAt(Resource resource, Site site, double amount) {
		ItemKey tuple = new ItemKey { itemType = resource, location = site };
		items[tuple] = amount;
	}

	private Dictionary<Site, int> employees = new Dictionary<Site, int>();

	public int GetEmployeesAt(Site site) {
		if (employees.ContainsKey(site))
			return employees[site];
		else
			return 0;
	}

	public void SetEmployeesAt(Site site, int numEmployees) {
		employees[site] = numEmployees;
	}

	public struct ItemKey {
		public Resource itemType; 
		public Site location;
	}

}
