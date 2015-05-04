using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class GameTime<T> {

	/** Time of a quarter in seconds */
	public const float QUARTER = 4.0f;

	/** The list of game timers */
	private List<GameTimer> list;

	public GameTime() {
		list = new List<GameTimer>();
	}

	/**
	 * Takes the number of quarters to wait and the function to perform
	 * Returns the integer id of the function to get the result later
	 */
	public int performActionIn(int quarters, Func<T> function) {
		int currentQuarter = (int) (Time.time / QUARTER) + 1;
		float startTime = (currentQuarter * QUARTER) + 1;
		float endTime = startTime + (quarters * QUARTER);
		list.Add(new GameTimer(startTime, endTime, quarters, function));
		return (list.Count - 1);
	}

	public int performActionRepeatedly (int quarters, Func<T> function) {
		int i = performActionIn (quarters, function);
		list[i].repeat = true;
		return i;
	}

	public T getResult(int i) {
		return list[i].result;
	}

	public void disable(int i) {
		list[i].endTime = 0.0f;
	}

	public void enable(int i) {
		int currentQuarter = (int) (Time.time / QUARTER) + 1;
		float startTime = (currentQuarter * QUARTER) + 1;
		list[i].endTime = startTime + (list[i].quarters * QUARTER);
	}

	void Update() {
		if (GameDirector.PAUSED) {
			return;
		}

		float time = Time.time - GameDirector.timeCorrection;
		if (list.Count > 0) {
			foreach(GameTimer timer in list) {
				if (time >= timer.endTime) {
					timer.result = timer.function.Invoke();
					if (timer.repeat) {
						timer.endTime = timer.endTime + (QUARTER * timer.quarters);
					}
				}
			}
		}
	}

	private class GameTimer {
		public float startTime;

		public float endTime;

		public Func<T> function;

		public T result;

		public int quarters;

		public bool repeat;

		public GameTimer(float startTime, float endTime, int quarters, Func<T> function, bool repeat = false) {
			this.startTime = startTime;
			this.endTime = endTime;
			this.function = function;
			this.quarters = quarters;
			this.repeat = repeat;
		}
	}
}
