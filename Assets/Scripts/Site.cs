using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Site : MonoBehaviour {
	public GameObject SitePlane;
	public GameObject LotPlane;
	public List<Lot> Lots { get; private set; }
	public int rows = 3;
	public int cols = 2;
	private Vector2 current;

	public Site () {
		current = new Vector2 (-rows/2.0f + 0.5f, -cols/2.0f + 0.5f);
		Lots = new List<Lot> ();
	}

	public Lot NewLot() {
		Lot newLot = SitePlane.AddComponent<Lot>();
		newLot.Site = this;
		Lots.Add (newLot);
		if (current.x > (rows/2.0f)) {
			current.x = -rows/2.0f + 0.5f;
			current.y++;
		}
		
		GameObject go = (GameObject) Instantiate(LotPlane, new Vector3(current.x * 10.1f,0.1f,current.y * 10.1f), SitePlane.transform.rotation);
		newLot.LotPlane = go;
		current.x++;
		return newLot;
	}
}