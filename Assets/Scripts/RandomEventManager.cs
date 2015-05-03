using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class RandomEventManager : MonoBehaviour {

	private int timer;

	public int maxTimer = 300;

	public int minTimer = 60;

	private List<Business> businesses;
	private List<Site> sites;

	private RandomEventPool choices;
	private List<RandomEvent> used;
	private RandomEvent current;
	private float currentTime, endTime;

	public GameObject dialog;
	
	private Button eventBtn1, eventBtn2, eventBtn3, eventBtn4;
	
	private Slider eventTimerSlider;
	private Text title, description, afterText;


	public RandomEventManager(List<Business> businesses, List<Site> sites) {
		this.businesses = businesses;
		this.sites = sites;
	}

	// Use this for initialization
	void Start () {
		choices = RandomEventPool.get (businesses, sites);
		used = new List<RandomEvent>();
		Button [] b;
		foreach(Button but in Object.FindObjectsOfType<Button>()) {
			switch(but.name) {
				case "REButton1":
				eventBtn1 = but;
				break;
				case "REButton2":
				eventBtn2 = but;
				break;
				case "REButton3":
				eventBtn3 = but;
				break;
				case "REButton4":
				eventBtn4 = but;
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
	
	// Update is called once per frame
	void Update () {

		if (currentTime >= endTime) {
			//TODO: Fire the random event... somehow.

			pick ();
		}
	}



	private void pick() {
		timer = UnityEngine.Random.Range (minTimer, maxTimer);
		current = choices [UnityEngine.Random.Range (0, choices.Count - 1)];
		choices.Remove (current);
		used.Add (current);

		if (choices.Count == 0) {
			choices = RandomEventPool.get (businesses, sites);
			used = new List<RandomEvent>();
		}

		currentTime = Time.time;
		endTime = currentTime + (float)timer;
	}



}
