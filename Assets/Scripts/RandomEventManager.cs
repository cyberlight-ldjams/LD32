using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

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


	public RandomEventManager(List<Business> businesses, List<Site> sites) {
		this.businesses = businesses;
		this.sites = sites;
	}

	// Use this for initialization
	void Start () {
		choices = RandomEventPool.get (businesses, sites);
		used = new List<RandomEvent>();

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
