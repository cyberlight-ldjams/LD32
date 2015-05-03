using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class World : MonoBehaviour {

	public GameObject worldPlane;
	public List<Site> sites { get; private set; }
	public int minSites = 5;
	public int maxSites = 8;
	
	void Start () {
		sites = new List<Site>();
		int total = (int) Random.Range(minSites, maxSites);
		float worldSizeX = worldPlane.transform.localScale.x * 4.5f;
		float worldSizeZ = worldPlane.transform.localScale.z * 4.5f;
		for (int i = 0; i < total; i++) {
			Site s = new Site(6, AIBusiness.UNOWNED);
			sites.Add(s);
			s.placeSite(new Vector3 (0, -1.0f, 0));
			float x = Random.Range(-worldSizeX, worldSizeX);
			float z = Random.Range(-worldSizeZ, worldSizeZ);
			int count = 0;
			for (int j = -1; j < sites.Count; j++) {
				//avoid potential infinite loop of death
				if(count > 1000) {
					count = 0;
					Debug.Log("I can't count that high");
					continue;
				}
				if (j == -1 && ((Mathf.Abs(0 - x) < worldSizeX / 6.0f) && 
				                  (Mathf.Abs(0 - z) < worldSizeZ / 6.0f))) {
					j = -2; // Two because 1 is added before the next iteration
				} else if ((Mathf.Abs(sites[i].getPlaneLocation().x - x) < worldSizeX / 6.0f) && 
				    (Mathf.Abs(sites[i].getPlaneLocation().z - z) < worldSizeZ / 6.0f)) {
					x = Random.Range(-worldSizeX, worldSizeX);
					z = Random.Range(-worldSizeZ, worldSizeZ);
					j = -2;
				}

				count++;

			}
			s.placeSite(new Vector3 (x, 1.0f, z));
		}
	}
}
