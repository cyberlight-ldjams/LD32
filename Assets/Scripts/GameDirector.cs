﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameDirector : MonoBehaviour {

	public int lotsPerSite;

	public GameObject selectedObject;

	public GameObject player;

	public List<GameObject> enemyAI;
	
	public Site site;

	private GameObject selection;

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

	public Lot findLot() {

	}

	private void randomAssign() {


		foreach(Lot l in site.Lots) {
			int winner = Random.Range (0, 1);
			if(winner == 0) {
				//l.Owner = player;
			} else {
				//sl.Owner = enemyAI[0];
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
			if (selection != null) {
				Object.Destroy(selection);
			}
			GameObject go = (GameObject) GameObject.CreatePrimitive(PrimitiveType.Plane);
			selection = Instantiate(go);
			selection.layer = 2;
			Renderer r = selection.GetComponent<Renderer>();
			r.material.color = Color.red;
			selection.transform.position = 
				new Vector3(hit.collider.gameObject.transform.position.x, hit.collider.gameObject.transform.position.y - 0.01f, hit.collider.gameObject.transform.position.z);
			selection.transform.localScale = new Vector3(hit.collider.gameObject.transform.localScale.x + 0.02f, 1, hit.collider.gameObject.transform.localScale.z + 0.02f);
			return hit.collider.gameObject;
		}
	}
	// Update is called once per frame
	void Update () {

		if (Input.GetMouseButtonDown (0))
			selectedObject = calculateSelectedObject ();
	
	}
}
