using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class Site {
	public GameObject SitePlane { get; private set; }
	public List<Lot> Lots { get; private set; }
	public int rows = 3;
	public int cols = 2;
	private Vector2 current;
	public string name { get; private set; }
	public List<Site> neighbors { get; set; }

	public Site (int lotsPerSite, Business owner) {
		name = RandomNameGenerator.generatePlaceName();
		Lots = new List<Lot> ();
		current = new Vector2(0, 0);
		neighbors = new List<Site> ();

		GameObject go;
		go = (GameObject) Object.Instantiate (Resources.Load ("SitePlane"));
		SitePlane = go;
		current = new Vector2 (-rows/2.0f + 0.5f + go.transform.position.x, 
		                       -cols/2.0f + 0.5f + go.transform.position.z);
		for (int i = 0; i < lotsPerSite; i++) {
			NewLot(owner);
		}
		MoveLots();

		AddLotResources();
	}

	private Lot NewLot(Business owner) {
		Lot newLot = new Lot (this, owner, new Vector3 (current.x * 10.1f, 0.1f, current.y * 10.1f), SitePlane.transform.rotation);
		Lots.Add (newLot);

		return newLot;
	}

	private void MoveLots() {
		current = new Vector2 (-rows/2.0f + 0.5f, 
		                       -cols/2.0f + 0.5f);
		foreach (Lot l in Lots) {
			if (current.x > (rows/2.0f)) {
				current.x = -rows/2.0f + 0.5f;
				current.y++;
			}

				l.getLotPlane().transform.position = 
					new Vector3 (current.x * 10.1f + SitePlane.transform.position.x, SitePlane.transform.position.y + 1.0f, 
					             current.y * 10.1f + SitePlane.transform.position.z);

				current.x++;
			
		}
	}

	private void AddLotResources() {
		int resourceCount = 3;
		List<Resource> resources = ResourceExtensions.RandomResources(resourceCount);
		List<Lot> lots = ResourceExtensions.ChooseRandom(Lots.ToArray(), resourceCount);

		for (int i = 0; i < resourceCount; i++) {
			lots[i].Resource = resources[i];
		}
	}

	public void placeSite(Vector3 location) {
		SitePlane.transform.position = location;
		MoveLots();
	}

	public Vector3 getPlaneLocation() {
		return SitePlane.transform.position;
	}
}