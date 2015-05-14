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
	private GameObject resourceUIContents, buildingsUIContents, workshopPanel;

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

	/** The buttons for selling leases and buildings */
	private Button sellLeaseBtn, sellBuildingBtn;

	/** The sliders for setting the labor and wage caps at a building */
	private Slider laborCap, wage;

	/** The currently selected lot, or null if none selected */
	private Lot selectedLot;

	/** The previously selected lot */
	private Lot prevSelected;

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
				case "SellLease":
					sellLeaseBtn = but;
					break;
				case "SellBuilding":
					sellBuildingBtn = but;
					break;
			}
		}

		Slider[] sliders = UnityEngine.Object.FindObjectsOfType<Slider>();
		foreach (Slider s in sliders) {
			if (s.name == "LaborCapSlider") {
				laborCap = s;
			} else if (s.name == "WageSlider") {
				wage = s;
			}
		}

		GameObject [] go = UnityEngine.Object.FindObjectsOfType<GameObject>();
		foreach (GameObject g in go) {
			if (g.name == "sellingPanelContent") {
				resourceUIContents = g;
			} else if (g.name == "BuildingPanel") {
				buildingsUIContents = g;
				Text[] text = g.GetComponentsInChildren<Text>();
				foreach (Text t in text) {
					if (t.gameObject.name == "TitleText") {
						buildingTitle = t;
					}
				}
			} else if (g.name == "SellingPanel") {
				Text[] text = g.GetComponentsInChildren<Text>();
				foreach (Text t in text) {
					if (t.gameObject.name == "Stock") {
						resourceTitle = t;
					}
				}
			} else if (g.name == "WorkshopPanel") {
				workshopPanel = g;
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
			quarryText.text = gameDirector.lm.appropriateQuarry.GetType().Name;
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
	 * Disables the leasing button if active
	 */
	private void disableLeaseButton() {
		if (leaseBtn.isActiveAndEnabled) {
			leaseBtn.gameObject.SetActive(false);
			leaseBtn.enabled = false;
		}
	}

	/**
	 * Disables both selling buttons if they are active
	 */
	private void disableSellingButtons() {
		if (sellBuildingBtn.isActiveAndEnabled) {
			sellBuildingBtn.gameObject.SetActive(false);
			sellBuildingBtn.enabled = false;
		}
		if (sellLeaseBtn.isActiveAndEnabled) {
			sellLeaseBtn.gameObject.SetActive(false);
			sellLeaseBtn.enabled = false;
		}
	}

	/**
	 * Disables all of the sliders for wage and labor
	 */
	private void disableSliders() {
		if (laborCap.isActiveAndEnabled) {
			laborCap.gameObject.SetActive(false);
			laborCap.enabled = false;
		}
		if (wage.isActiveAndEnabled) {
			wage.gameObject.SetActive(false);
			wage.enabled = false;
		}
	}

	/**
	 * Disables all the buildingUI buttons and sliders
	 */
	private void disableAllBuildingButtons() {
		disableBuildingButtons();
		disableLeaseButton();
		disableSellingButtons();
		disableSliders();
		if (workshopPanel.activeSelf) {
			workshopPanel.SetActive(false);
		}
	}

	/**
	 * Enables the building buttons if something is selected
	 */
	private void buildingButtons() {
		if (quarryBtn != null && workshopBtn != null) {
			// Don't display any of the buildings UI if there is nothing selected
			if ((gameDirector == null || gameDirector.selectedObject == null)) {
				disableAllBuildingButtons();
				buildingsUIContents.SetActive(false);
				prevSelected = selectedLot;
				selectedLot = null;
				return;
			}

			prevSelected = selectedLot;
			selectedLot = Lot.FindLot(gameDirector.selectedObject, currentSite);

			// If the selected lot does not belong to anyone, let the user lease it
			if (selectedLot.Owner == AIBusiness.UNOWNED) {
				buildingsUIContents.SetActive(true);
				disableSliders();
				disableBuildingButtons();
				disableSellingButtons();
				if (workshopPanel.activeSelf) {
					workshopPanel.SetActive(false);
				}
				buildingTitle.text = "Lease Lot";

				// Enable the lease button
				leaseBtn.enabled = true;
				leaseBtn.gameObject.SetActive(true);

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
			}

			// Otherwise, bring up the building options
			else {
				buildingsUIContents.SetActive(true);

				// If there is a building, either display options for the player or the AI building stats
				if (selectedLot.Building != null) {
					disableBuildingButtons();
					disableLeaseButton();
					if (workshopPanel.activeSelf) {
						workshopPanel.SetActive(false);
					}

					// Display Sliders, make sure to update them if we change lots
					if (!laborCap.isActiveAndEnabled || prevSelected != selectedLot) {
						laborCap.enabled = true;
						laborCap.gameObject.SetActive(true);
						laborCap.value = selectedLot.Building.laborCap;
						int employCount = 0;
						foreach (Lot l in selectedLot.Site.Lots) {
							if (l.Building != null) {
								employCount += l.Building.employees;
							}
						}
						laborCap.maxValue = selectedLot.Site.employees + employCount;
					}
					if (!wage.isActiveAndEnabled || prevSelected != selectedLot) {
						wage.enabled = true;
						wage.gameObject.SetActive(true);
						wage.value = selectedLot.Building.employeeWage;
					}

					// Get the labor and wage slider text elements
					Text laborText = null;
					Text laborCount = null;
					foreach (Text t in laborCap.GetComponentsInChildren<Text>()) {
						if (t.gameObject.name == "Text") {
							laborText = t;
						} else {
							laborCount = t;
						}
					}
					Text wageText = wage.GetComponentInChildren<Text>();

					// If this lot is owned by the player, let them interact with the sliders
					// Also display the sell building button
					if (selectedLot.Owner == gameDirector.playerBusiness) {
						wage.interactable = true;
						laborCap.interactable = true;

						// Check to see if the user has changed the sliders
						// If they changed the sliders, change the actual values
						if (wage.value != selectedLot.Building.employeeWage) {
							selectedLot.Building.employeeWage = (int)wage.value;
						}
						if (laborCap.value != selectedLot.Building.laborCap) {
							selectedLot.Building.laborCap = (int)laborCap.value;
						}

						if (!sellBuildingBtn.isActiveAndEnabled) {
							sellBuildingBtn.enabled = true;
							sellBuildingBtn.gameObject.SetActive(true);
							Text t = sellBuildingBtn.GetComponentInChildren<Text>();
							t.text = "Sell Building for\n" + Sales.BUILD_SELL + " " + gameDirector.stager.currencyName;
						}
						if (sellLeaseBtn.isActiveAndEnabled) {
							sellLeaseBtn.gameObject.SetActive(false);
							sellLeaseBtn.enabled = false;
						}
						buildingTitle.text = selectedLot.Building.GetType().Name + " Options";
					} 

					// If the player doesn't own this lot, display the building stats, but don't let them be edited
					else {
						buildingTitle.text = selectedLot.Owner.name + "'s " + selectedLot.Building.GetType().Name;
						disableAllBuildingButtons();

						wage.interactable = false;
						laborCap.interactable = false;

						// Update the values in case the AI has changed something
						wage.value = selectedLot.Building.employeeWage;
						laborCap.maxValue = selectedLot.Site.employees;
						laborCap.value = selectedLot.Building.laborCap;
					}

					wageText.text = "Workers' Wage: " + (int)wage.value;
					laborText.text = "Desired Employees: " + (int)laborCap.value;
					laborCount.text = "Current Employment: " + selectedLot.Building.employees;
				} 

				// If there is no building, let the player place buildings, if they own this lot
				else if (selectedLot.Owner == gameDirector.playerBusiness) {
					buildingTitle.text = "Create Buildings";
					disableLeaseButton();
					disableSliders();

					// If the player is requesting a workshop, display that panel
					if (gameDirector.requestWorkshop) {
						if (!workshopPanel.activeSelf) {
							workshopPanel.SetActive(true);
						}
					} else {
						if (workshopPanel.activeSelf) {
							workshopPanel.SetActive(false);
						}
					}

					// There's no building, so don't display the sell building button
					if (sellBuildingBtn.isActiveAndEnabled) {
						sellBuildingBtn.gameObject.SetActive(false);
						sellBuildingBtn.enabled = false;
					}
					// But we do need the sell lease button
					if (!sellLeaseBtn.isActiveAndEnabled) {
						sellLeaseBtn.enabled = true;
						sellLeaseBtn.gameObject.SetActive(true);
						Text t = sellLeaseBtn.GetComponentInChildren<Text>();
						t.text = "Sell Lease for\n" + Sales.LOT_LEASE_SELL + " " + gameDirector.stager.currencyName;
					}

					// If there is an appropriate quarry, display the button
					if (gameDirector.lm.appropriateQuarry != null && !quarryBtn.IsActive()) {
						enableQuarryButton(true);
					} 

					// If the player has selected someplace without an appropriate quarry, disable the button
					else if (gameDirector.lm.appropriateQuarry == null) {
						enableQuarryButton(false);
					}

					// Check to see if the text on the quarry button needs to change
					else if (quarryBtn.IsActive() && quarryBtn.GetComponentInChildren<Text>().text != 
						gameDirector.lm.appropriateQuarry.GetType().Name) {
						Text quarryText = quarryBtn.GetComponentInChildren<Text>();
						quarryText.text = gameDirector.lm.appropriateQuarry.GetType().Name;
					}
					workshopBtn.enabled = true;
					workshopBtn.gameObject.SetActive(true);
				} 

				// If someone else owns the lot, just state who owns it
				// We already checked if it was unowned earlier, so that should never be the case
				else if (selectedLot.Owner != gameDirector.playerBusiness) {
					buildingTitle.text = selectedLot.Owner.name + "'s Lot";
					disableAllBuildingButtons();
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
			sellingUI();
		}

		// // ENABLE/DISABLE APPROPRIATE BUTTONS // //

		buildingButtons();		
	}
}
