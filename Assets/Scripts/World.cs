using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class World : MonoBehaviour {

	public GameObject worldPlane;
	private List<Site> sites;
	public int minSites = 5;
	public int maxSites = 8;
	
	void Start () {
		sites = new List<Site>();
		int total = (int) Random.Range(minSites, maxSites);
		float worldSize = worldPlane.transform.localScale.x * 4.9f;
		for (int i = 0; i < total; i++) {
			Site s = new Site(6, AIBusiness.UNOWNED);
			sites.Add(s);
			s.placeSite(new Vector3 (0, 1.0f, 0));
			float x = Random.Range(-worldSize, worldSize);
			float z = Random.Range(-worldSize, worldSize);
			for (int j = -1; j < sites.Count; j++) {
				if (j == -1 && ((Mathf.Abs(0 - x) < worldSize / 8.0f) && 
				                  (Mathf.Abs(0 - z) < worldSize / 8.0f))) {
					j = -2; // Two because 1 is added before the next iteration
				} else if ((Mathf.Abs(sites[i].getPlaneLocation().x - x) < worldSize / 8.0f) && 
				    (Mathf.Abs(sites[i].getPlaneLocation().z - z) < worldSize / 8.0f)) {
					x = Random.Range(-worldSize, worldSize);
					z = Random.Range(-worldSize, worldSize);
					j = -2;
				}
			}
			s.placeSite(new Vector3 (x, 0.1f, z));
		}
	}
}
