using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class GameTime : MonoBehaviour {

	/** Time of a quarter in seconds */
	public const float QUARTER = 4.0f;

	/** The list of game timers */
	private List<GameTimer> list;

	/** The current quarter */
	public int currentQuarter { get; private set; }

	/** Creates the list for storing timers */
	public GameTime() {
		list = new List<GameTimer>();
	}

	/**
	 * Takes the number of quarters to wait and the function to perform
	 * Returns the integer ID of the function to get the result later
	 * or to disable the function before it happens or to renable it
	 * 
	 * @param quarters how many quarters to wait before performing the function
	 * @param function the function to perform
	 * @return int the ID of this function
	 */
	public int performActionIn(int quarters, Func<object> function) {
		int currentQuarter = (int)(Time.time / QUARTER) + 1;
		float startTime = (currentQuarter * QUARTER) + 1;
		float endTime = startTime + (quarters * QUARTER);
		list.Add(new GameTimer(startTime, endTime, quarters, function));
		return (list.Count - 1);
	}

	public int performActionRepeatedly(int quarters, Func<object> function) {
		int i = performActionIn(quarters, function);
		list [i].repeat = true;
		return i;
	}

	public object getResult(int i) {
		return list [i].result;
	}

	/**
	 * Disables a timer given its ID
	 * 
	 * @param i the ID of the timer to disable
	 */
	public void disable(int i) {
		list [i].endTime = 0.0f;
	}

	/**
	 * Enables and resets a timer given its ID
	 * 
	 * @param i the ID of the timer to renable
	 */
	public void enable(int i) {
		int currentQuarter = (int)(Time.time / QUARTER) + 1;
		float startTime = (currentQuarter * QUARTER) + 1;
		list [i].endTime = startTime + (list [i].quarters * QUARTER);
	}

	/**
	 * Checks whether timers need to have their functions performed
	 */
	void Update() {

		// // PAUSED BEHAVIOR // //

		if (GameDirector.PAUSED) {
			return;
		}

		// Get the current quarter, correcting for how long the game has been paused
		float time = Time.time - GameDirector.timeCorrection;
		currentQuarter = (int)(time / QUARTER) + 1;

		// If there are any timers, iterate through and check them
		if (list.Count > 0) {
			foreach (GameTimer timer in list) {
				// If a timer is ready to be enabled, invoke its function
				if (time >= timer.endTime) {
					timer.result = timer.function.Invoke();
					// If the timer is supposed to repeat, set it to repeat
					if (timer.repeat) {
						timer.endTime = timer.endTime + (QUARTER * timer.quarters);
					}
				}
			}
		}
	}

	/**
	 * Creates a timer for a specific function
	 */
	private class GameTimer {

		/** When this timer started */
		public float startTime;

		/** The time when this timer's function is triggered */
		public float endTime;

		/** The function performed by this timer */
		public Func<object> function;

		/** The result of this function, either true or false */
		public object result;

		/** How many quarters to wait until the function is triggered */
		public int quarters;

		/** Whether or not this timer repeats after it completes its function */
		public bool repeat;

		/** 
		 * Creates a game timer
		 * 
		 * @param startTime the start time of the timer - first quarter it was active or renabled
		 * @param endTime the time when the timer triggers
		 * @param quarters how many quarters to wait before performing the function
		 * @param function the function to perform
		 * @param repeat whether the timer repeats, false by default
		 */
		public GameTimer(float startTime, float endTime, int quarters, Func<object> function, bool repeat = false) {
			this.startTime = startTime;
			this.endTime = endTime;
			this.function = function;
			this.quarters = quarters;
			this.repeat = repeat;
		}
	}
}
