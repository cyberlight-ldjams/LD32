using UnityEngine;
using System.Collections;

public class GameDirector : MonoBehaviour {

	public int lotsPerSite;
	
	public Site site;

	// Use this for initialization
	void Start () {
		Instantiate (site.SitePlane);
		for (int i = 0; i < lotsPerSite; i++) {
			site.NewLot();
		}
	}

	public void MakeBuilding(Building b) {
		site.Lots[0].NewBuilding (b);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
