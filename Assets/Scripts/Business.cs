using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class Business {

	public const double STARTING_CURRENCY = 10000.0;

	public Inventory myInventory;

	private List<Lot> myLots;

	public string name;

	private Sales sales;

	public void Init() {
		myInventory = new Inventory(genericCurrency: STARTING_CURRENCY);
		sales = GameDirector.THIS.sales;
	}

	public void LeaseLot(Lot toLease) {
		sales.leaseLot(this, toLease);
	}

	public void SellResource(Site s, Resource r, int quantitity) {
		sales.sell (this, s, r, quantitity);
	}

	public void BuyBuilding(Lot lot, Building build) {
		sales.buyBuilding(this, lot, build);
	}

	public bool TransportGoods(Resource r, double quantity, Site a, Site b) {
		if (myInventory.getAmountOfAt(r, a) >=  quantity) {
			myInventory.setAmountOfAt(r, a, -quantity);

			//TODO: TRANSPORT TIME

			myInventory.setAmountOfAt(r, b, quantity);
			return true;
		} else {
			return false;
		}
	}

	public void setLaborCap(Building b, int amount) {
		b.laborCap = amount;
	}

	public void setWageCap(Building b, int amount) {
		b.employeeWage = amount;
	}
}