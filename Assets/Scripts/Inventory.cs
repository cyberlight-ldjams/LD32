using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class Inventory {



	/** the base currency, unmodified by any stage modifiers */
	private double genericCurrency;

	/** contains all valid resource items */
	private List<Item> items; 

	/** Create a new inventory (only needed by business class) */
	public Inventory(double genericCurrency, List<Item> items) {

		this.genericCurrency = genericCurrency;
		this.items = items;

	}

	/** get the items in your inventory
	 * @return a list of <Item>s
	 */
	private List<Item> getInventory() {
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
		foreach(Item i in items) {
			if(i.itemType.Equals(resource)) {
				return i.quantity;
			}
		}
		return 0.0;
	}

	public void setAmountOfAt(Resource resource, Site site, double amount) {
		foreach (Item i in items) {
			if(i.itemType.Equals(resource) && i.location.Equals(site)) {
				i.quantity = amount;
				break;
			}
		}





	}

	/** Container object for inventory items */
	public class Item {

			public Resource itemType { get; private set; }

			public double quantity { get; set; }

			public Site location { get; private set; }

			/** Creates a new item
			 * @param int itemType the enum value of the resource
			 * @param double quantity how much of the item we have here
			 * @param Site the location the inventory item is in
			 */
			public Item (Resource itemType, double quantity, Site site) {
				this.itemType = itemType;
				this.quantity = quantity;
				location = site;
			}
		}

}
