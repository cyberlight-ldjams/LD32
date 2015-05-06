using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

public class StatusUI : MonoBehaviour {


	public GameDirector director;

	private PlayerBusiness player;

	private GameTime time;

	private Text[] fields;

	public GameObject statusUI;

	// Use this for initialization
	void Start() {
		player = director.playerBusiness;
		time = GameDirector.gameTime;
		fields = statusUI.gameObject.GetComponentsInChildren<Text>();
	}
	
	// Update is called once per frame
	void Update() {

		foreach (Text t in fields) {
			if (t.name == "CompanyName") {
				t.text = player.name;
			} else if (t.name == "Bank") {
				t.text = "Bank: " + player.myInventory.getBaseCurrency() + " " + director.stager.currencyName;
			} else if (t.name == "Date") {
				int currentQuarter = (time.currentQuarter % 4);

				if (currentQuarter == 0) {
					currentQuarter = 4;
				}

				t.text = "Year: " + ((time.currentQuarter - 1) / 4) + " (Q" + currentQuarter + ")";
			} else {
				t.text = Enum.GetName(typeof(Stage), director.stager.currentStage);
			}


		}
	
	}
}
