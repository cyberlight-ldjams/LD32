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
	private float currentTime, randTime;

	public GameObject dialog;
	
	private List<Button> buttons;
	
	private Slider eventTimerSlider;
	private Text title, description, afterText;


	public EventManager(List<Business> businesses, List<Site> sites) {
		this.businesses = businesses;
		this.sites = sites;
	}

	// Use this for initialization
	void Start () {

		//Initialize random event info
		choices = RandomEventPool.get (businesses, sites);
		used = new List<RandomEvent>();
		Button [] b;
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
	}

	private void randButton(List<RandomEvent.Option> o) {
		int count = 0;
		for (int i = 0; i < buttons.Count; i++) {
			if (count <= i) {
				
			}
		}
	}



	private void populateRandomEventUI() {
		title.text = currentEvent.title;
		description.text = currentEvent.description;
	}
	
	// Update is called once per frame
	void Update () {

		if (currentTime >= randTime) {
			//TODO: Fire the random event... somehow.

			pick ();
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
