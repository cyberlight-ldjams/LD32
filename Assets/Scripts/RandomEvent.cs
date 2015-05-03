using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RandomEvent {

	public int minTimer = 10;

	private string title;

	private string description;

	private List<Option> options;

	private float timer;


	public RandomEvent(string title, string desc, List<Option> options, float timeSec) {
		this.title = title;
		description = desc;
		this.options = options;

		if (timer < minTimer) {
				timer = minTimer;
			}

	}

	public float getCountdownTimer() {
		return timer;
	}


	public class Option {

		private string optionText;

		private List<Affect> affects;

		private string afterText;

		public Option (string opText, List<Affect> affects, string result) {
			optionText = opText;
			this.affects = affects;
			afterText = result;
		}

	}

	public class Affect {

		private Business affectedBusiness;

		private Resource? affectedResource;

		private double currencyChange;

		private double resourceChange;

		private Site affectedSite;

		public Affect (Business b, Resource? r = null, Site site = null, int minCurrencyChange =0, int maxCurrencyChange =0, int minResourceChange =0, int maxResourceChange = 0) {

			affectedBusiness = b;
			affectedResource = r;
			this.currencyChange = currencyChange;
			this.resourceChange = resourceChange;
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
			
			currencyChange = Random.Range (minCurrencyChange, maxCurrencyChange);
			resourceChange = Random.Range (minResourceChange, maxResourceChange);

		}

		public void execute() {

			//resource adjustments
			if (affectedResource != null) {

				double currentResource = affectedBusiness.myInventory.getAmountOfAt ((Resource)affectedResource, affectedSite);
				double adjustedResource = currentResource + this.resourceChange;

				//no negative
				if (adjustedResource < 0.0) {
					adjustedResource = 0.0;
				}

				affectedBusiness.myInventory.setAmountOfAt ((Resource)affectedResource, affectedSite, adjustedResource);
			}

			//currency adjustments
			affectedBusiness.myInventory.addBaseCurrency (currencyChange);

		}
	}
}
