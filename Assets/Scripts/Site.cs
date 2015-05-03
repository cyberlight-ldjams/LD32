using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Site {
	private GameObject SitePlane;
	public List<Lot> Lots { get; private set; }
	public int rows = 3;
	public int cols = 2;
	private Vector2 current;

	public Site (int lotsPerSite, Business owner) {
		current = new Vector2 (-rows/2.0f + 0.5f, -cols/2.0f + 0.5f);
		Lots = new List<Lot> ();

		GameObject go = (GameObject) Object.Instantiate (Resources.Load ("SitePlane"));
		SitePlane = go;
		for (int i = 0; i < lotsPerSite; i++) {
			NewLot(owner);
		}
	}

	private Lot NewLot(Business owner) {
		if (current.x > (rows/2.0f)) {
			current.x = -rows/2.0f + 0.5f;
			current.y++;
		}

		Lot newLot = new Lot (this, owner, new Vector3 (current.x * 10.1f, 0.1f, current.y * 10.1f), SitePlane.transform.rotation);
		Lots.Add (newLot);

		current.x++;
		return newLot;
	}
}