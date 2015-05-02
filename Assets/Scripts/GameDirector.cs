using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameDirector : MonoBehaviour {

	public int lotsPerSite;

	public GameObject selectedObject;

	public GameObject player;

	public List<GameObject> enemyAI;
	
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
		print (selectedObject);
		if (selectedObject.name.Contains("LotPlane")) {
			foreach (Lot l in site.Lots) {
				if (selectedObject == l.LotPlane) {
					l.NewBuilding(b);
					break;
				}
			}
			//Lot l = selectedObject.GetComponent<Lot>();
			//l.NewBuilding(b);
			//site.Lots[0].NewBuilding(b);
		}
	}

	private void randomAssign() {


		foreach(Lot l in site.Lots) {
			int winner = Random.Range (0, 1);
			if(winner == 0) {
				l.Owner = player;
			} else {
				l.Owner = enemyAI[0];
			}
		}
	}

	private GameObject calculateSelectedObject() {
		Debug.Log ("Click");
		Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
		RaycastHit hit;
		Physics.Raycast (ray, out hit);
		Debug.Log ("Hit at: " + hit.point);
		if (hit.collider == null) {
			return selectedObject;
		} else {
			return hit.collider.gameObject;
		}
	}
	// Update is called once per frame
	void Update () {

		if (Input.GetMouseButtonDown (0))
			selectedObject = calculateSelectedObject ();
	
	}
}
