using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/**
 * This class contains the whole world - a reference to every site, the WorldPlane
 */
public class World : MonoBehaviour {

	/** The world plane - ALL OF EXISTANCE!!! */
	public GameObject worldPlane; // Set in the Unity Editor

	/** A reference to the GameDirector */
	private GameDirector gameDirector;

	/** An exhaustive list of every site in the world! */
	public List<Site> sites { get; private set; }

	/** The minimum number of sites allowed in the world */
	public int minSites = 5;

	/** The maximum number of sites allowed in the world */
	public int maxSites = 8;

	/** The number of rows of lots in each site */
	public int siteRows = 2;

	/** The number of columns of lots in each site */
	public int siteCols = 3;

	/** Whether or not the world has been fully generated */
	public bool isReady { get; set; } // This is later set to false by the GameDirector

	/** The "home site" where the player first starts */
	public Site homesite { get; private set; }

	/** The player */
	public PlayerBusiness player { get; set; }

	/**
	 * Creates the world and all the sites and so, by extension, all the lots
	 */
	void Start() {
		isReady = false; // The world isn't ready!
		gameDirector = GameDirector.THIS;
		sites = new List<Site>();

		// Create the homesite
		homesite = newSite(new Vector2(0.0f, 0.0f));

		int total = (int)Random.Range(minSites, maxSites);
		float worldSizeX = worldPlane.transform.localScale.x * 4.5f;
		float worldSizeZ = worldPlane.transform.localScale.z * 4.5f;
		float x = Random.Range(-worldSizeX, worldSizeX);
		float z = Random.Range(-worldSizeZ, worldSizeZ);

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
				} else if ((Mathf.Abs(sites [j].getPlaneLocation().x - x) < worldSizeX / 6.0f) && 
					(Mathf.Abs(sites [j].getPlaneLocation().z - z) < worldSizeZ / 6.0f)) {
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

		// Connect all sites together based on closest neighbors
		connectSites();

		// Set the current site in the game director and therefore also the HUD
		gameDirector.setCurrentSite(homesite);

		isReady = true; // The world has been made, and is therefore ready
	}

	/**
	 * Creates a new site at the given location
	 * 
	 * @param xy the location of the new site (actually x and z coordiantes)
	 * @return the created site
	 */
	private Site newSite(Vector2 xy) {
		Site site = new Site(siteRows, siteCols, AIBusiness.UNOWNED);
		sites.Add(site);
		sites [sites.Count - 1].placeSite(new Vector3(xy.x, 1.0f, xy.y));
		return site;
	}

	void Update() {
		if (sites == null || GameDirector.PAUSED) {
			return;
		}

		foreach (Site s in sites) {
			if (s.neighbors == null) {
				Debug.Log("No Neighbors");
				continue;
			}
			foreach (Site s1 in s.neighbors) {
				Debug.DrawLine(s1.SitePlane.transform.position, s.SitePlane.transform.position);
			}

		}
	}

	private void connectSites() {
		for (int i = 0; i < sites.Count -1; i++) {
			for (int j = i + 1; j < sites.Count; j++) {
				Vector3 s1 = sites [i].SitePlane.transform.position;
				Vector3 s2 = sites [j].SitePlane.transform.position;

				float dist = Vector3.Distance(s1, s2);


				bool doNotConnect = false;
				for (int k = 0; k < sites.Count; k++) {
					if (i == k || i == j) {
						continue;
					}
					Vector3 s3 = sites [k].SitePlane.transform.position;
					if (Vector3.Distance(s1, s3) < dist && Vector3.Distance(s2, s3) < dist) {
						doNotConnect = true;
						break;
					}
				}
				if (!doNotConnect) {
					sites [i].neighbors.Add(sites [j]);
					sites [j].neighbors.Add(sites [i]);
				}
				
			}
		}
	}
}
