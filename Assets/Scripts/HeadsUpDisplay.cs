using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;
using System.Collections.Generic;

/**
 * The HUD, gives the player relevant information and allows them to make choices
 */
public class HeadsUpDisplay : MonoBehaviour {

	/** Pixel offset for new items in the Resource UI */
	public const int RESOURCE_UI_OFFSET = -32;

	/** The player */
	public PlayerBusiness business { get; private set; }

	/** The current site */
	public Site currentSite { get; set; }

	/** The previously current site - best variable name ever */
	private Site oldCurrent;

	/** The GameDirector */
	private GameDirector gameDirector;

	/** The resource UI and building UI */
	private GameObject resourceUIContents, buildingsUIContents;

	/** A list of the quantities for the resrouce UI */
	private List<GameObject> qtyList;

	/** The quarry making button */
	private Button quarryBtn;

	/** The workshop making button */
	private Button workshopBtn;

	/**
	 * Creates the HUD
	 */
	void Start() {
		gameDirector = GameDirector.THIS;

		// Creates the quarry and workshop buttons
		Button[] buttons = UnityEngine.Object.FindObjectsOfType<Button>();
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

		GameObject [] go = UnityEngine.Object.FindObjectsOfType<GameObject>();
		foreach (GameObject g in go) {
			if (g.name == "sellingPanelContent") {
				resourceUIContents = g;
			} else if (g.name == "BuildingPanel") {
				buildingsUIContents = g;
			}
		}

		if (resourceUIContents == null || buildingsUIContents == null) {
			Debug.LogError("We can't find the parent UI elements");
		}

		business = gameDirector.playerBusiness;
		qtyList = new List<GameObject>();
	}

	/**
	 * Creates the selling UI
	 */
	private void sellingUI() {
		int count = -6;
		foreach (Resource r in Enum.GetValues(typeof(Resource))) {

			bool has = false;
			foreach (GameObject ob in qtyList) {
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

			if (qty > 0) {
				GameObject obj = (GameObject)UnityEngine.Object.Instantiate(Resources.Load("ResourceLineUI"));
				ResourceUiItem script = obj.GetComponent<ResourceUiItem>();
				script.myResource = r;
				obj.transform.SetParent(resourceUIContents.transform, false);
				script.yPos = count * RESOURCE_UI_OFFSET;
				qtyList.Add(obj);
				count++;
			}
		}
		
	}

	/**
	 * Enables the building buttons if something is selected
	 */
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
	
	/**
	 * What to do each frame
	 */
	void Update() {

		// // PAUSED BEHAVIOR // //

		if (GameDirector.PAUSED) {
			return;
		}

		// // UPDATE RESOURCE UI // //

		if (oldCurrent != currentSite) {
			foreach (GameObject go in qtyList) {
				UnityEngine.Object.Destroy(go);
			}
			qtyList.Clear();
			sellingUI();
			oldCurrent = currentSite;
		} else {
			//sellingUI ();
		}

		// // ENABLE/DISABLE APPROPRIATE BUTTONS // //

		buildingButtons();		
	}
}
