using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;
using System.Collections.Generic;

public class HeadsUpDisplay : MonoBehaviour {

	public GameObject clayText;
	public GameObject potteryText;
	public GameObject employeeText;
	public PlayerBusiness business { get; private set; }
	public Site currentSite;
	private Site oldCurrent;
	public GameDirector gameDirector;

	public const int RESOURCE_UI_OFFSET = -32;

	private GameObject resourceUIContents, buildingsUIContents;

	private List<GameObject> qtyList;


	private Button quarryBtn;
	private Button workshopBtn;



	// Use this for initialization
	void Start () {
		Button[] buttons = UnityEngine.Object.FindObjectsOfType<Button> ();
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

		GameObject [] go = UnityEngine.Object.FindObjectsOfType<GameObject> ();
		foreach (GameObject g in go) {
			if(g.name == "sellingPanelContent") {
				resourceUIContents = g;
			} else if(g.name == "BuildingPanel") {
				buildingsUIContents = g;
			}
		}

		if (resourceUIContents == null || buildingsUIContents == null) {
			Debug.LogError ("We can't find the parent UI elements");
		}

		business = gameDirector.playerBusiness;
		qtyList = new List<GameObject> ();
	}

	private void sellingUI() {
		int count = -6;
		foreach(Resource r in Enum.GetValues(typeof(Resource))) {

			bool has = false;
			foreach(GameObject ob in qtyList) {
				ResourceUiItem scriptRes = ob.GetComponent<ResourceUiItem>();
				if (scriptRes.myResource == r) {
					has = true;
					break;
				}
			}
			if (has) {
				continue;
			}
			
			int qty = (int)gameDirector.playerBusiness.myInventory.getAmountOfAt(r, currentSite);

			//if(qty > 0) {
				GameObject obj = (GameObject) UnityEngine.Object.Instantiate (Resources.Load ("ResourceLineUI"));
				ResourceUiItem script = obj.GetComponent<ResourceUiItem>();
				script.myResource = r;
				obj.transform.SetParent (resourceUIContents.transform, false);
				script.yPos = count * RESOURCE_UI_OFFSET;
				qtyList.Add(obj);
				count++;
			//}
		}
		
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

		if (oldCurrent != currentSite) {
			foreach(GameObject go in qtyList) {
				UnityEngine.Object.Destroy(go);
			}
			qtyList.Clear();
			sellingUI ();
			oldCurrent = currentSite;
		} else {
			//sellingUI ();
		}

		buildingButtons ();
	
		
	}


}
