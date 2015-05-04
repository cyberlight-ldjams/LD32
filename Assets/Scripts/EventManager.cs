using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class EventManager : MonoBehaviour {

	private int timer;

	public int maxTimer = 300;

	public int minTimer = 60;

	private List<Business> businesses;
	private List<Site> sites;

	private RandomEventPool choices;
	private List<RandomEvent> used;
	private RandomEvent currentEvent;
	private float currentTime, randTime, timerTime;

	public GameObject dialog;
	
	private List<Button> buttons;
	
	private Slider eventTimerSlider;
	private Text title, description, afterText;



	// Use this for initialization
	void Start () {

		World world = Object.FindObjectOfType<World> ();
		GameDirector gd = Object.FindObjectOfType < GameDirector> ();
		businesses = new List<Business> ();
		businesses.Add (gd.playerBusiness);

		sites = world.sites;

		Debug.Log ("I'm alive!");
		//Initialize random event info
		buttons = new List<Button> (4);
		choices = RandomEventPool.get (businesses, sites);
		used = new List<RandomEvent>();

		foreach(Button but in Object.FindObjectsOfType<Button>()) {
			switch(but.name) {
				case "REButton1":
				buttons[0] = but;
				break;
				case "REButton2":
				buttons[1] = but;
				break;
				case "REButton3":
				buttons[2] = but;
				break;
				case "REButton4":
				buttons[3] = but;
				break;
			}


		}	

	Text [] temp = dialog.GetComponentsInChildren<Text> ();
	foreach (Text t in temp) {
		string textName = t.name;
		
		switch (textName) {
		case "Title":
			title = t;
			break;
		case "Description":
			description = t;
			break;
		case "afterText":
			afterText = t;
			break;
		}
	}

	Slider [] s = Object.FindObjectsOfType<Slider> ();
	foreach (Slider s1 in s) {
		if (s1.name.Equals ("Timer")) {
			eventTimerSlider = s1;
			break;
		}
	}

		pick ();
		//TODO REMOVE
		populateRandomEventUI ();
	}

	private void resetRandomEventUI() {
		dialog.SetActive (false);
		afterText.gameObject.SetActive (false);
		foreach (Button b in buttons) {
			b.gameObject.SetActive (true);
		}
		title.gameObject.SetActive (true);
		description.gameObject.SetActive (true);
	}

	private void switchRandomEventUI() {
		title.gameObject.SetActive (false);
		description.gameObject.SetActive (false);
		foreach (Button b in buttons) {
			b.gameObject.SetActive (false);
		}
		eventTimerSlider.gameObject.SetActive (false);

		afterText.gameObject.SetActive (true);
		Text t = buttons [3].GetComponent<Text> ();
		t.text = "OK.";
		buttons [3].onClick.AddListener (() => { resetRandomEventUI(); });
		buttons [3].gameObject.SetActive (true);
	}

	private void populateRandomEventUI() {
		title.text = currentEvent.title;
		description.text = currentEvent.description;
		afterText.text = currentEvent.result;
		afterText.gameObject.SetActive (false);

		List<RandomEvent.Option> o = currentEvent.options;

		System.Random rand = new System.Random ();
		for (int i = 0; i < buttons.Count; i++) {
			//still have options left
			if(o.Count > 0) {
				RandomEvent.Option pick = o[rand.Next(o.Count)];
				Text t = buttons[i].GetComponent<Text>();
				t.text = pick.optionText;
				buttons[i].onClick.AddListener(() => { currentEvent.execute(pick); switchRandomEventUI(); });
				
			} else {
				buttons[i].gameObject.SetActive(false);
			}
		}

		eventTimerSlider.maxValue = currentEvent.timer;
		eventTimerSlider.minValue = 0.0f;
		eventTimerSlider.value = currentEvent.timer;
		eventTimerSlider.gameObject.SetActive (true);
		timerTime = Time.time + currentEvent.timer;
		dialog.SetActive (true);
	}
	
	// Update is called once per frame
	void Update () {

		if (currentTime >= randTime) {
			//TODO: Fire the random event... somehow.


			pick ();
		}

		//decrement timer until 0
		if (eventTimerSlider.gameObject.activeSelf && eventTimerSlider.value > 0.0f) {
			eventTimerSlider.value = Time.time - timerTime;
		} else if (eventTimerSlider.gameObject.activeSelf && eventTimerSlider.value == 0.0f) {
			//default choice
			currentEvent.execute (currentEvent.defaultOption);
			eventTimerSlider.gameObject.SetActive (false);
			switchRandomEventUI();
		}
	}



	private void pick() {
		timer = UnityEngine.Random.Range (minTimer, maxTimer);
		currentEvent = choices [UnityEngine.Random.Range (0, choices.Count - 1)];
		choices.Remove (currentEvent);
		used.Add (currentEvent);

		if (choices.Count == 0) {
			choices = RandomEventPool.get (businesses, sites);
			used = new List<RandomEvent>();
		}

		currentTime = Time.time;
		randTime = currentTime + (float)timer;
	}
}