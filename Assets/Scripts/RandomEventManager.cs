using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class NewBehaviourScript : MonoBehaviour {

	private int timer;

	public int maxTimer = 300;

	public int minTimer = 60;

	private List<RandomEvent> choices;
	private List<RandomEvent> used;
	private RandomEvent current;
	private float currentTime, endTime;

	// Use this for initialization
	void Start () {
		makeEvents ();
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
		
		timer = Random.Range (minTimer, maxTimer);
		current = choices [Random.Range (0, choices.Count - 1)];
		choices.Remove (current);
		used.Add (current);

		if (choices.Count == 0) {
			choices = used;
		}

		currentTime = Time.time;
		endTime = currentTime + (float)timer;
	}

	private void makeEvents() {
		//TODO: JASON

	}


}
