using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Site : MonoBehaviour {
	public GameObject SitePlane;
	public List<Lot> Lots { get; private set; }

	public Lot NewLot() {
		Lot newLot = new Lot (this);
		Lots.Add (newLot);
		return newLot;
	}

	// Use this for initialization
	void Start () {
		Instantiate (SitePlane);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
