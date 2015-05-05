using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

public class HeadsUpDisplay : MonoBehaviour {

	public GameObject clayText;
	public GameObject potteryText;
	public GameObject employeeText;
	public PlayerBusiness business { get; private set; }
	public Site currentSite;
	public GameDirector gameDirector;

	private GameObject resourceUIContents, buildingsUIContents;


	private Button quarryBtn;
	private Button workshopBtn;



	// Use this for initialization
	void Start () {
		Button[] buttons = Object.FindObjectsOfType<Button> ();
		foreach (Button but in buttons) {
			string butName = but.name;

			switch (butName) {
			case  "QuarryButton":
				quarryBtn = but;
				break;
			case "WorkshopButton":
				workshopBtn = but;
				break;
			
			}
		}

		GameObject [] go = Object.FindObjectsOfType<GameObject> ();
		foreach (GameObject g in go) {
			if(g.name == "SellResource") {
				resourceUIContents = g;
			} else if(g.name == "BuildingPanel") {
				buildingsUIContents = g;
			}
		}

		business = gameDirector.playerBusiness;
	}

	private void sellingUI() {

		Resource [] has = new Resource[];

		foreach(

	}

	private void buildingButtons() {
		if (quarryBtn != null && workshopBtn != null) {
			//dissalow placement before selection
			if ((gameDirector == null || gameDirector.selectedObject == null)) {
				quarryBtn.enabled = false;
				workshopBtn.enabled = false;
			} else {
				quarryBtn.enabled = true;
				quarryBtn.enabled = true;
			}
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (GameDirector.PAUSED) {
			return;
		}

		sellingUI ();


		buildingButtons ();
	
		
	}


}
