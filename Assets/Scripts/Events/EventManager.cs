using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

/**
 * The event manager for handling random and other events
 */
public class EventManager : MonoBehaviour {

	/** The timer for how long until the event is displayed */
	private float timer;

	/** Whether the timer has been set */
	private bool timerSet;

	/** The maximum value of the timer to wait for the next event */
	public const float MAX_TIMER = 300.0f; // 5 minutes

	/** The minimum value of the timer to wait for the next event */
	public const float MIN_TIMER = 60.0f; // 1 minute

	/** The possible businesses affected by the event */
	private List<Business> businesses;

	/** The possible sites affected by the event */
	private List<Site> sites;

	/** The random event choices */
	private RandomEventPool choices;

	/** Random events that have been used */
	private List<RandomEvent> used;

	/** The current random event */
	private RandomEvent currentEvent;

	/** Time values for displaying the timer */
	private float currentTime, randTime, timerTime;

	/** Whether the event is currently being displayed */
	public bool displayed { get; private set; }

	/** The dialog for the event */
	public GameObject dialog;

	/** The option buttons */
	private List<Button> buttons;

	/** The timer */
	private Slider eventTimerSlider;

	/** Text elements used in the dialog */
	private Text title, description, afterText;

	/** Reference to the game director */
	private GameDirector gd;

	/**
	 * What to do when the event manager is initialized
	 */
	void Start() {
		gd = GameDirector.THIS;

		// Get the businesses
		businesses = new List<Business>();
		businesses.Add(gd.playerBusiness);

		// Get the sites
		World world = gd.world;
		sites = world.sites;

		// Inform us that the random timer hasn't been set, but don't set it yet
		timerSet = false;

		// Find the Blackout game object
		foreach (GameObject go in Object.FindObjectsOfType<GameObject>()) {
			if (go.name == "Blackout") {
				dialog = go;
				break;
			}
		}

		//Initialize random event info
		buttons = new List<Button>(4);
		for (int i = 0; i < 4; i++) {
			buttons.Add(null);
		}
		choices = RandomEventPool.get(businesses, sites);
		used = new List<RandomEvent>();

		// Put all the buttons in the list
		foreach (Button but in Object.FindObjectsOfType<Button>()) {
			switch (but.name) {
				case "REButton1":
					buttons [0] = but;
					break;
				case "REButton2":
					buttons [1] = but;
					break;
				case "REButton3":
					buttons [2] = but;
					break;
				case "REButton4":
					buttons [3] = but;
					break;
			}


		}	

		// Find all the text elements
		Text [] temp = dialog.GetComponentsInChildren<Text>();
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

		// Find the slider
		Slider [] s = Object.FindObjectsOfType<Slider>();
		foreach (Slider s1 in s) {
			if (s1.name.Equals("Timer")) {
				eventTimerSlider = s1;
				break;
			}
		}

		// Move the dialog back to the center of the screen
		if (dialog.activeSelf) {
			RectTransform rect = dialog.GetComponent<RectTransform>();
			rect.anchoredPosition = new Vector2(0, 0);
			dialog.SetActive(false);
		}
	}

	/**
	 * Resets the RandomEventUI back to the choices and disables the dialog
	 */
	private void resetRandomEventUI() {
		afterText.gameObject.SetActive(false);
		foreach (Button b in buttons) {
			b.gameObject.SetActive(true);
		}
		title.gameObject.SetActive(true);
		description.gameObject.SetActive(true);

		dialog.SetActive(false);
		displayed = false;
	}

	/**
	 * Switches over to the results dialog
	 */
	private void switchRandomEventUI() {
		title.gameObject.SetActive(false);
		description.gameObject.SetActive(false);
		foreach (Button b in buttons) {
			b.gameObject.SetActive(false);
		}
		eventTimerSlider.gameObject.SetActive(false);

		afterText.text = currentEvent.result;
		if (afterText.text == null || afterText.text.Trim() == "") {
			resetRandomEventUI();
			return;
		}
		afterText.gameObject.SetActive(true);
		buttons [3].gameObject.SetActive(true);
		Text[] t = buttons [3].GetComponentsInChildren<Text>();
		t [0].text = "OK.";
		buttons [3].onClick.AddListener(() => {
			resetRandomEventUI(); });
	}

	/**
	 * Populates the RandomEventUI with the current random event
	 */
	private void populateRandomEventUI() {
		dialog.SetActive(true);

		title.text = currentEvent.title;
		description.text = currentEvent.description;
		afterText.text = currentEvent.result;
		afterText.gameObject.SetActive(false);

		List<RandomEvent.Option> o = currentEvent.options;

		System.Random rand = new System.Random();
		for (int i = 0; i < buttons.Count; i++) {
			//still have options left
			if (o.Count > 0) {
				RandomEvent.Option pick = o [rand.Next(o.Count)];
				Text[] t = buttons [i].GetComponentsInChildren<Text>();
				t [0].text = pick.optionText;
				buttons [i].onClick.AddListener(() => {
					currentEvent.execute(pick);
					switchRandomEventUI(); });
				o.Remove(pick);
			} else {
				buttons [i].gameObject.SetActive(false);
			}
		}

		if (currentEvent.timer != RandomEvent.DISABLE_TIMER) {
			eventTimerSlider.maxValue = currentEvent.timer;
			eventTimerSlider.minValue = 0.0f;
			eventTimerSlider.value = currentEvent.timer;
			eventTimerSlider.gameObject.SetActive(true);
			timerTime = currentEvent.timer;
		} else {
			eventTimerSlider.gameObject.SetActive(false);
		}
	}

	/**
	 * What to do each frame
	 */
	void Update() {

		// // PAUSED BEHAVIOR // //

		// Also don't do anything if the current stage is Archaic
		if (GameDirector.PAUSED || gd.stager == null || gd.stager.currentStage == Stage.Machine) {
			return;
		}

		// Set the timer if it isn't currently set and we aren't displaying a message
		if (!timerSet && !displayed) {
			// Set the random timer for the first time
			timer = UnityEngine.Random.Range(MIN_TIMER, MAX_TIMER);
			randTime = updateCurrentTime() + timer;
			timerSet = true;
		}

		// // BEHAVIOR TO DISPLAY // //

		// Current time is the time less the time correction
		if (updateCurrentTime() >= randTime && !displayed) {
			pick();
			populateRandomEventUI();
			timerSet = false;
		}

		// Make sure we don't show the dialog when we don't need to
		if (!displayed && dialog.activeSelf) {
			dialog.SetActive(false);
		}

		// // BEHAVIOR WHILE DISPLAYED // //

		//decrement timer until 0
		if (eventTimerSlider.gameObject.activeSelf && eventTimerSlider.value > 0.0f && displayed) {
			timerTime = timerTime - Time.deltaTime;
			eventTimerSlider.value = timerTime;
		} else if (eventTimerSlider.gameObject.activeSelf && eventTimerSlider.value <= 0.0f && displayed) {
			//default choice
			currentEvent.execute(currentEvent.defaultOption);
			eventTimerSlider.gameObject.SetActive(false);
			switchRandomEventUI();
		}
	}

	private void pick() {
		displayed = true;
		dialog.SetActive(true);
		currentEvent = choices [UnityEngine.Random.Range(0, choices.Count)];
		choices.Remove(currentEvent);
		used.Add(currentEvent);

		if (choices.Count == 0) {
			choices = RandomEventPool.get(businesses, sites);
			used = new List<RandomEvent>();
		}
	}

	/**
	 * Updates the current time, adjusting for time correction
	 * 
	 * @return the current time
	 */
	private float updateCurrentTime() {
		currentTime = Time.time - GameDirector.timeCorrection;
		return currentTime;
	}
}