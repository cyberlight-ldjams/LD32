using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

/**
 * The status UI in the top right corner of the screen
 */
public class StatusUI : MonoBehaviour {

	/** The player */
	private PlayerBusiness player;

	/** The GameDirector's game timer */
	private GameTime time;

	/** The stager */
	private Stager stage;

	/** The text elements contained in this UI */
	private Text[] fields;

	/** The game object of the Status UI */
	public GameObject statusUI; // Set in the Unity Interface

	/**
	 * Finds the elements for the fields
	 */
	void Start() {
		player = GameDirector.THIS.playerBusiness;
		time = GameDirector.gameTime;
		fields = statusUI.gameObject.GetComponentsInChildren<Text>();
		stage = GameDirector.THIS.stager;
	}
	
	/**
	 * Updates the status UI to reflect the current status
	 */
	void Update() {

		// // PAUSED BEHAVIOR // //

		if (GameDirector.PAUSED) {
			return;
		}

		// // UPDATE ALL STATUS ELEMENTS // //

		// Get the string of the name of the current stage
		string stageName = Enum.GetName(typeof(Stage), stage.currentStage);

		// Get the string represenation of the player's bank info
		string bankText = "Bank: " + player.myInventory.getBaseCurrency() + " " + stage.currencyName;

		// Construct the time information based on the current quarter
		// There are four quarters in a year... like always...
		int currentQuarter = (time.currentQuarter % 4);
		if (currentQuarter == 0) {
			currentQuarter = 4;
		}
		string timeInfo = "Year: " + ((time.currentQuarter - 1) / 4) + " (Q" + currentQuarter + ")";

		// Update each text field if it has changed
		foreach (Text t in fields) {
			if (t.name == "CompanyName" && t.text != player.name) {
				t.text = player.name;
			} else if (t.name == "Bank" && t.text != bankText) {
				t.text = bankText;
			} else if (t.name == "Date" && t.text != timeInfo) {
				t.text = timeInfo;
			} else if (t.name == "Stage" && t.text != stageName) {
				t.text = stageName;
			}


		}
	
	}
}
