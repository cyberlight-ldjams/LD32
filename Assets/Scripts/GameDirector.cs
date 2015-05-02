using UnityEngine;
using System.Collections;

public class GameDirector : MonoBehaviour {

	public int lotsPerSite;

	public GameObject selectedObject;
	
	public Site site;

	// Use this for initialization
	void Start () {
		GameObject go = (GameObject)Instantiate (site.SitePlane);
		site.SitePlane = go;
		for (int i = 0; i < lotsPerSite; i++) {
			site.NewLot();
		}
	}

	public void MakeBuilding(Building b) {
		site.Lots[0].NewBuilding (b);
	}

	private GameObject calculateSelectedObject() {
		Debug.Log ("Click");
		Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
		RaycastHit hit;
		Physics.Raycast (ray, out hit);
		Debug.Log ("Hit at: " + hit.point);
		return hit.collider.gameObject;
	}
	// Update is called once per frame
	void Update () {

		if (Input.GetMouseButtonDown (0))
			selectedObject = calculateSelectedObject ();
	
	}
}
