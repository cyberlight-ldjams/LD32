using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class Inventory : MonoBehaviour {



	/** the base currency, unmodified by any stage modifiers */
	private double genericCurrency;

	/** contains all valid resource items */
	private List<Item> items; 

	public Inventory(double genericCurrency) {
		this (genericCurrency, null);
	}
	/** Create a new inventory (only needed by business class) */
	public Inventory(List<double> items) {
		this (0.0, items);
	}

	/** Create a new inventory (only needed by business class) */
	public Inventory() {
		this (0.0, null);
		}

	/** Create a new inventory (only needed by business class) */
	public Inventory(double genericCurrency, List<double> items) {

		this.genericCurrency = genericCurrency;
		this.items = items;

	}

	/** get the items in your inventory
	 * @return a list of <Item>s
	 */
	public List<Item> getInventory() {
			return items;
		}

	/** Container object for inventory items */
	public class Item {

			public int itemType { get; private set; }

			public double quantity { get; set; }

			public Site location { get; private set; }

			/** Creates a new item
			 * @param int itemType the enum value of the resource
			 * @param double quantity how much of the item we have here
			 * @param Site the location the inventory item is in
			 */
			public Item (int itemType, double quantity, Site site) {
				this.itemType = itemType;
				this.quantity = quantity;
				location = site;
			}
		}

}
