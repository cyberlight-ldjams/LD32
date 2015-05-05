using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class Site {

	public const int DEFAULT_EMPLOYEES = 15;

	public GameObject SitePlane { get; private set; }
	public List<Lot> Lots { get; private set; }
	public int rows = 3;
	public int cols = 2;
	private Vector2 current;
	public string name { get; private set; }
	public List<Site> neighbors { get; set; }
	public int employees { get; set; }

	public Site (int lotsPerSite, Business owner) {
		name = RandomNameGenerator.generatePlaceName();
		Lots = new List<Lot> ();
		current = new Vector2(0, 0);
		neighbors = new List<Site> ();
		employees = DEFAULT_EMPLOYEES;

		GameObject go;
		go = (GameObject) Object.Instantiate (Resources.Load ("SitePlane2"));
		TextMesh text = go.GetComponentInChildren<TextMesh>();
		text.text = name;
		SitePlane = go;
		current = new Vector2 (-rows/2.0f + 0.5f + go.transform.position.x, 
		                       -cols/2.0f + 0.5f + go.transform.position.z);
		for (int i = 0; i < lotsPerSite; i++) {
			NewLot(owner);
		}
		MoveLots();

		AddLotResources();

		GameDirector.gameTime.performActionRepeatedly(1, () => {this.AllocateLabor(); return true;});
	}

	public void AllocateLabor() {
		List<Building> available = new List<Building>();
		foreach (Lot l in Lots) {
			if (l.Building != null && l.Building.laborCap > 0) {
				Building b = l.Building;
				this.employees += b.employees;
				b.employees = 0;
				available.Add(b);
			}
		}
		while (this.employees > 0 && available.Count > 0) {
			int bestWage = 0;
			List<Building> best = new List<Building>();
			foreach (Building b in available) {
				if (b.employeeWage > bestWage) {
					best.Clear();
					best.Add(b);
					bestWage = b.employeeWage;
				} else if (b.employeeWage == bestWage) {
					best.Add(b);
				}
			}
			if (best.Count == 0) {
				break;
			}
			if (best.Count == 1) {
				giveLabor (best[0]);
				available.Remove(best[0]);
			} else { 
				Debug.Log("Multiple bests!");
				while (this.employees > 0 && best.Count > 1) {
					foreach (Building b in best) {
						if (b.laborCap <= b.employees) {
							best.Remove(b);
						} else if (this.employees > 0) {
							b.employees++;
							this.employees--;
						}
					}
				}
				if (best.Count > 1) {
					giveLabor(best[0]);
					available.Remove(best[0]);
				}
			}
		}
	}

	private void giveLabor(Building b) {
		if (this.employees <= 0) {
			return;
		}
		int needs = this.employees - b.laborCap - b.employees;
		if (this.employees >= needs) {
			this.employees -= needs;
			b.employees += needs;

		} else {
			b.employees += this.employees;
			this.employees = 0;
		}
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

				l.RepositionLotPlane(
					new Vector3 (current.x * 10.1f + SitePlane.transform.position.x, SitePlane.transform.position.y + 1.0f, 
					             current.y * 10.1f + SitePlane.transform.position.z)
				);

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