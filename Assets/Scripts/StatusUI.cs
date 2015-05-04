using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

public class StatusUI : MonoBehaviour {


	public GameDirector director;

	private PlayerBusiness player;

	private GameTime<bool> time;

	private Text[] fields;

	// Use this for initialization
	void Start () {
		player = director.playerBusiness;
		time = new GameTime<bool> ();
		fields = this.gameObject.GetComponentsInChildren<Text> ();
		Debug.Log (this.gameObject.name);
	}
	
	// Update is called once per frame
	void Update () {

		foreach (Text t in fields) {
			if(t.name == "CompanyName") {
				t.text = player.name;
			} else if(t.name == "Bank") {
				t.text = "Bank: " + player.myInventory.getBaseCurrency() + " " + director.stager.currencyName;
			} else if(t.name == "Date") {
				t.text = "Year: " + (time.currentQuarter / 4) + " (" + time.currentQuarter % 4 + ")";
			} else {
				t.text = Enum.GetName(typeof(Stage), director.stager.currentStage);
			}


		}

		Debug.Log(fields.Length);
	
	}
}
