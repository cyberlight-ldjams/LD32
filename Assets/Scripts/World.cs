using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class World : MonoBehaviour {

	private List<Site> sites;
	public int minSites = 5;
	public int maxSites = 8;

	public World() {
		sites = new List<Site>();
		int total = (int) Random.Range(minSites, maxSites);
		for (int i = 0; i < total; i++) {
			Site s = new Site(6, AIBusiness.UNOWNED);
			sites.Add(s);
		}
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
