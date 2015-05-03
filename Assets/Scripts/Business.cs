using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class Business {

	public Inventory myInventory;

	private List<Lot> myLots;

	public string name;

	public void Init() {
		myInventory = new Inventory(genericCurrency: 0.0, items: new List<Inventory.Item>());
	}
}
