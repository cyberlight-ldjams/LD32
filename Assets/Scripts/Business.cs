using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class Business {

	public const double STARTING_CURRENCY = 10000.0;

	public Inventory myInventory;

	private List<Lot> myLots;

	public string name;

	public void Init() {
		myInventory = new Inventory(genericCurrency: STARTING_CURRENCY);
	}

	public void LeaseLot(Lot toLease) {
		GameDirector.THIS.sales.leaseLot(this, toLease);
	}
}
