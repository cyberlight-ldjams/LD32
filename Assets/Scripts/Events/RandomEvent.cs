using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/**
 * Class that describes a random event
 */
public class RandomEvent {

	/** Giving the timer this value will disable it */
	public const float DISABLE_TIMER = -0.1f;

	/** The minimum amount of time that a timer can have */
	public const float MIN_TIMER = 15.0f;

	/** Value that denotes to use a random default option */
	private const int RAND_DEFAULT = -1;

	/** The title of the random event */
	public string title{ get; private set; }

	/** The description of the random event */
	public string description{ get; private set; }

	/** The description of the result of the option choice */
	public string result { get; private set; }

	/** The options for the random event, up to 4 */
	public List<Option> options{ get; private set; }

	/** The timer for the random event, in seconds */
	public float timer{ get; private set; }

	/** A default option if the timer runs out before an option is chosen - if none is chosen, this is random */
	public Option defaultOption{ get; private set; }

	/**
	 * Creates a random event given the title, description, list of options, and time for the timer
	 * 
	 * @param title the title of the random event
	 * @param desc the description of the random event
	 * @param options the options the player may choose between
	 * @param timeSec the time, in seconds, for the timer, minimum by default
	 * @param defaultOption the list position of the default option, random by default
	 */
	public RandomEvent(string title, string desc, List<Option> options, float timeSec = MIN_TIMER, int defaultOption = RAND_DEFAULT) {
		int optNum = defaultOption;
		if (optNum == RAND_DEFAULT || optNum >= options.Count) {
			optNum = Random.Range(0, options.Count);
		}
		this.defaultOption = options [optNum];
		this.title = title;
		description = desc;
		this.options = options;

		// Disable the timer if that's what we're supposed to do
		if (timeSec <= DISABLE_TIMER) {
			timer = DISABLE_TIMER;
		}

		// If the timer is going to be less than the minimum time, set it to the minimum time
		else if (timer < MIN_TIMER) {
			timer = MIN_TIMER;
		} else {
			timer = timeSec;
		}

	}

	/**
	 * Execute the specified option, with all of its Affects
	 * Then set the result string equal to the choices after text
	 * 
	 * @param choice the option to execute
	 */
	public void execute(Option choice) {

		foreach (Affect a in choice.getAffects()) {
			a.execute();
		}

		result = choice.afterText;
	}

	/**
	 * Defines an option for a random event
	 */
	public class Option {

		/** The text description of the option */
		public string optionText{ get; private set; }

		/** The affects the option has */
		private List<Affect> affects;

		/** The text description of the result of choosing the option */
		public string afterText{ get; private set; }

		/**
		 * Creates an option given its text, a list of affects, and the result text
		 * 
		 * @param opText the text description of the option
		 * @param affects the list of affects the option has
		 * @param result the text description of the result
		 */
		public Option(string opText, List<Affect> affects, string result) {
			optionText = opText;
			this.affects = affects;
			afterText = result;
		}

		/**
		 * Get the list of affects the option has
		 * 
		 * @return the list of affects of choosing this option
		 */
		public List<Affect> getAffects() {
			return affects;
		}

	}

	/**
	 * Defines an affect of choosing an option
	 */
	public class Affect {

		/** Which business is affected */
		private Business affectedBusiness;

		/** Which resource is affected */
		private Resource? affectedResource;

		/** Any change to the business' money */
		private double currencyChange;

		/** Any change to the specified resource */
		private double resourceChange;

		/** The site where the affected resources are held */
		private Site affectedSite;

		/**
		 * Creates an affect, given a business
		 * 
		 * Can also take an affected resource, site where the resource is held, minimum and maximum currency change, and minimum and maximum resource change
		 * 
		 * @param b the business affected
		 * @param r the resource affected, null by default
		 * @param site the site where the affected resource is held, null by default
		 * @param minCurrencyChange the minimum affect to the business' money, 0 by default
		 * @param maxCurrencyChange the maximum affect to the business' money, 0 by default
		 * @param minResourceChange the minimum affect to the specified resource at the given site, 0 by default
		 * @param maxResourceChange the maximum affect to the specified resource at the given site, 0 by default
		 */
		public Affect(Business b, Resource? r = null, Site site = null, int minCurrencyChange =0, int maxCurrencyChange =0, int minResourceChange =0, int maxResourceChange = 0) {

			affectedBusiness = b;
			affectedResource = r;
			affectedSite = site;

			int temp;
			
			//4AM safeguard - make sure the min and max are actually min and max
			if (minCurrencyChange > maxCurrencyChange) {
				temp = minCurrencyChange;
				minCurrencyChange = maxCurrencyChange;
				maxCurrencyChange = temp;
			}
			
			if (r != null && (minResourceChange > maxResourceChange)) {
				temp = minResourceChange;
				minResourceChange = maxResourceChange;
				maxResourceChange = temp;
			}

			// Set the affect as a random value between the two choices
			currencyChange = Random.Range(minCurrencyChange, maxCurrencyChange);
			resourceChange = Random.Range(minResourceChange, maxResourceChange);

		}

		/**
		 * Exceutes an individual affect object
		 */
		public void execute() {

			//resource adjustments, if needed
			if (affectedResource != null) {

				double currentResource = affectedBusiness.myInventory.getAmountOfAt((Resource)affectedResource, affectedSite);
				double adjustedResource = currentResource + this.resourceChange;

				//no negative
				if (adjustedResource < 0.0) {
					adjustedResource = 0.0;
				}

				affectedBusiness.myInventory.setAmountOfAt((Resource)affectedResource, affectedSite, adjustedResource);
			}

			//currency adjustments
			affectedBusiness.myInventory.addBaseCurrency(currencyChange);

		}
	}
}
