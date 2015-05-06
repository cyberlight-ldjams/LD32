using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RandomEvent {

	public const float DISABLE_TIMER = -0.1f;

	public int minTimer = 10;

	public string title{ get; private set; }

	public string description{ get; private set; }

	public string result { get; private set; }

	public List<Option> options{ get; private set; }

	public float timer{ get; private set; }

	public Option defaultOption{ get; private set; }


	public RandomEvent(string title, string desc, List<Option> options, float timeSec) {
		int optNum = Random.Range(0, options.Count - 1);
		defaultOption = options [optNum];
		this.title = title;
		description = desc;
		this.options = options;

		if (timer < minTimer) {
			timer = minTimer;
		}

	}

	/**
	 * Execute the specified option, along with all of its Affects */
	public void execute(Option choice) {

		foreach (Affect a in choice.getAffects()) {
			a.execute();
		}

		result = choice.afterText;

	}

	public class Option {

		public string optionText{ get; private set; }

		private List<Affect> affects;

		public string afterText{ get; private set; }

		public Option(string opText, List<Affect> affects, string result) {
			optionText = opText;
			this.affects = affects;
			afterText = result;
		}

		public List<Affect> getAffects() {
			return affects;
		}

	}

	public class Affect {

		private Business affectedBusiness;

		private Resource? affectedResource;

		private double currencyChange;

		private double resourceChange;

		private Site affectedSite;

		public Affect(Business b, Resource? r = null, Site site = null, int minCurrencyChange =0, int maxCurrencyChange =0, int minResourceChange =0, int maxResourceChange = 0) {

			affectedBusiness = b;
			affectedResource = r;
			affectedSite = site;

			int temp;
			
			//4AM safeguard
			if (minCurrencyChange > maxCurrencyChange || maxCurrencyChange < minCurrencyChange) {
				temp = minCurrencyChange;
				minCurrencyChange = maxCurrencyChange;
				maxCurrencyChange = temp;
			}
			
			if (r != null && (minResourceChange > maxResourceChange || maxResourceChange < minResourceChange)) {
				temp = minResourceChange;
				minResourceChange = maxResourceChange;
				maxResourceChange = temp;
			}
			
			currencyChange = Random.Range(minCurrencyChange, maxCurrencyChange);
			resourceChange = Random.Range(minResourceChange, maxResourceChange);

		}

		/**
		 * Exceutes an individual affect object
		 */
		public void execute() {

			//resource adjustments
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
