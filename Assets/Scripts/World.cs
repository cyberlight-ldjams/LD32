using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class World : MonoBehaviour {

	public GameObject worldPlane;
	public GameDirector gameDirector;
	public List<Site> sites { get; private set; }
	public int minSites = 5;
	public int maxSites = 8;
	public bool isReady {get; set;}
	public Site homesite { get; private set;}
	public PlayerBusiness player { get; set; }
	
	void Start () {
		Debug.Log(player.myInventory);
		isReady = false;
		sites = new List<Site>();
		int total = (int) Random.Range(minSites, maxSites);
		float worldSizeX = worldPlane.transform.localScale.x * 4.5f;
		float worldSizeZ = worldPlane.transform.localScale.z * 4.5f;
		float x = Random.Range(-worldSizeX, worldSizeX);
		float z = Random.Range(-worldSizeZ, worldSizeZ);
		homesite = new Site(6, player);
		gameDirector.setCurrentSite(homesite);
		for (int a = 0; a < 1; a++) {
			if (((Mathf.Abs(0 - x) < worldSizeX / 6.0f) && 
			                (Mathf.Abs(0 - z) < worldSizeZ / 6.0f))) {
				a = -1; // -2 because 1 is added before the next iteration
				x = Random.Range(-worldSizeX, worldSizeX);
				z = Random.Range(-worldSizeZ, worldSizeZ);
			} else {
				newSite(new Vector2(x, z));
			}
		}

		for (int i = 0; i < total; i++) {
			int count = 0;
			for (int j = 0; j < sites.Count; j++) {
				//avoid potential infinite loop of death
				if (count > 1000 && sites.Count > minSites || count > 5000) {
					count = 0;
					if (count > 5000) {
						Debug.Log("Some sites could not be added, " + 
							"consider setting fewer for this size of world, or let them be closer together.");
					}
					x = 0;
					z = 0;
					continue;
				} else if ((Mathf.Abs(sites[j].getPlaneLocation().x - x) < worldSizeX / 6.0f) && 
				           (Mathf.Abs(sites[j].getPlaneLocation().z - z) < worldSizeZ / 6.0f)) {
					x = Random.Range(-worldSizeX, worldSizeX);
					z = Random.Range(-worldSizeZ, worldSizeZ);
					j = -1;
				}
				count++;
			}
			if (x != 0 && z != 0) {
				newSite(new Vector2(x, z));
			}
		}
		isReady = true;
	}

	private void newSite(Vector2 xy) {
		sites.Add(new Site(6, AIBusiness.UNOWNED));
		sites[sites.Count - 1].placeSite(new Vector3 (xy.x, 1.0f, xy.y));
	}
}
