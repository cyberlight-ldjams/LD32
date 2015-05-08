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

	/** The titles of the resourceUI and buildingUI elements */
	private Text resourceTitle, buildingTitle;

	/** A list of the quantities for the resrouce UI */
	private List<GameObject> qtyList;

	/** The quarry making button */
	private Button quarryBtn;

	/** The currently appropriate quarry, or null if there is none */
	public Quarry appropriateQuarry { get; set; }

	/** The workshop making button */
	private Button workshopBtn;

	/** The button for puchasing a lot */
	private Button leaseBtn;

	/**
	 * Creates the HUD
	 */
	void Start() {
		gameDirector = GameDirector.THIS;

		// Finds the quarry, workshop, and purchase lot buttons
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
				case "LeaseLotButton":
					leaseBtn = but;
					break;
			}
		}

		GameObject [] go = UnityEngine.Object.FindObjectsOfType<GameObject>();
		foreach (GameObject g in go) {
			if (g.name == "sellingPanelContent") {
				resourceUIContents = g;
			} else if (g.name == "BuildingPanel") {
				buildingsUIContents = g;
				buildingTitle = buildingsUIContents.GetComponentInChildren<Text>();
			} else if (g.name == "SellingPanel") {
				Text[] text = g.GetComponentsInChildren<Text>();
				foreach (Text t in text) {
					if (t.gameObject.name == "Stock") {
						resourceTitle = t;
					}
				}
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
		resourceTitle.text = currentSite.name + " Stock";
		int count = -6; // This shouldn't be needed, but is for some reason
		// TODO : Find out why we need this...
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
	 * Enable/disable the quarry button
	 * 
	 * @param enable whether to enable (true) or disable (false) the quarry button
	 */
	private void enableQuarryButton(bool enable) {
		Text quarryText = quarryBtn.GetComponentInChildren<Text>();
		if (!enable && quarryBtn.IsActive()) {
			quarryText = quarryBtn.GetComponentInChildren<Text>();
			quarryText.text = "Quarry";
			quarryBtn.gameObject.SetActive(false);
			quarryBtn.enabled = false;
		} else if (enable && !quarryBtn.IsActive()) {
			quarryBtn.enabled = true;
			quarryBtn.gameObject.SetActive(true);
			quarryText = quarryBtn.GetComponentInChildren<Text>();
			quarryText.text = gameDirector.appropriateQuarry.GetType().Name;
		}
	}

	/**
	 * Disables both of the building buttons if they are active
	 */
	private void disableBuildingButtons() {
		if (quarryBtn.isActiveAndEnabled) {
			enableQuarryButton(false);
		}
		if (workshopBtn.isActiveAndEnabled) {
			workshopBtn.gameObject.SetActive(false);
			workshopBtn.enabled = false;
		}
	}

	/**
	 * Enables the building buttons if something is selected
	 */
	private void buildingButtons() {
		if (quarryBtn != null && workshopBtn != null) {
			//disallow placement before selection
			if ((gameDirector == null || gameDirector.selectedObject == null)) {
				disableBuildingButtons();
				if (leaseBtn.isActiveAndEnabled) {
					leaseBtn.gameObject.SetActive(false);
					leaseBtn.enabled = false;
				}
				buildingsUIContents.SetActive(false);
			} else if (Lot.FindLot(gameDirector.selectedObject, currentSite).Owner != 
				gameDirector.playerBusiness) {
				buildingsUIContents.SetActive(true);
				disableBuildingButtons();
				if (!leaseBtn.isActiveAndEnabled) {
					leaseBtn.gameObject.SetActive(true);
					leaseBtn.enabled = true;
				}
				buildingTitle.text = "Lease Lot";
				leaseBtn.enabled = true;

				// Set the button's two text elements
				Text[] text = leaseBtn.GetComponentsInChildren<Text>();
				foreach (Text t in text) {
					if (t.gameObject.name == "DownPayText") {
						t.text = Sales.LOT_DOWN_PAYMENT + " " + gameDirector.stager.currencyName;
					} else {
						t.text = "(" + Sales.LOT_LEASE_COST + " per " + 
							Sales.LOT_LEASE_DUE + " quarters)";
					}
				}
			} else {
				buildingsUIContents.SetActive(true);
				if (leaseBtn.isActiveAndEnabled) {
					leaseBtn.gameObject.SetActive(false);
					leaseBtn.enabled = false;
				}

				Lot selected = Lot.FindLot(gameDirector.selectedObject, currentSite);
				buildingsUIContents.SetActive(true);
				if (selected.Building != null) {
					disableBuildingButtons();
					if (selected.Owner == gameDirector.playerBusiness) {
						buildingTitle.text = "Building Options";
					} else {
						buildingTitle.text = "Building Statistics";
					}
				} else {
					buildingTitle.text = "Create Buildings";

					// If there is an appropriate quarry, display the button
					if (gameDirector.appropriateQuarry != null && !quarryBtn.IsActive()) {
						enableQuarryButton(true);
					} 

					// If the player has selected someplace without an appropriate quarry, disable the button
					else if (gameDirector.appropriateQuarry == null) {
						enableQuarryButton(false);
					}

					// Check to see if the text on the quarry button needs to change
					else if (quarryBtn.IsActive() && quarryBtn.GetComponentInChildren<Text>().text != 
						gameDirector.appropriateQuarry.GetType().Name) {
						Text quarryText = quarryBtn.GetComponentInChildren<Text>();
						quarryText.text = gameDirector.appropriateQuarry.GetType().Name;
					}
					workshopBtn.enabled = true;
					workshopBtn.gameObject.SetActive(true);
				}
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
