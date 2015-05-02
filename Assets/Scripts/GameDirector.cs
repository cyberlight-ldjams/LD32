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
	
	// Update is called once per frame
	void Update () {
	
	}
}
