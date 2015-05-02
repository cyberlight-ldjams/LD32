using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class Business : MonoBehaviour {

	public Inventory myInventory;

	private List<Lot> myLots;

	private string name;

	public void Start() {
		myInventory = new Inventory(genericCurrency: 0.0, items: new List<Inventory.Item>());
	}
}
