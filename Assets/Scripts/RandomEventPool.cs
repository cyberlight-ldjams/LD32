using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

/**
 * A pool of random events
 * 
 * Use the EventMaker.java program for a UI for creating the code for these
 */
public class RandomEventPool : List<RandomEvent> {

	/**
	 * Gets the list of random events
	 */
	public static RandomEventPool get(List<Business> businesses, List<Site> sites) {

		/** Store in this list of random events */
		RandomEventPool rep = new RandomEventPool();


		/*
		 * This is a generic, random event : it can affect any business and any type of resource
		 */
		string title = "";
		string description = "";
		List<RandomEvent.Option> options = new List<RandomEvent.Option>();
		float time = 0.0f;
		
		string text = "";
		List<RandomEvent.Affect> affects = new List<RandomEvent.Affect>();
		string result = "";

		int randBiz = (int)UnityEngine.Random.Range(0, businesses.Count);
		Business business = businesses [randBiz];
		
		Resource resource = (Resource)((int)UnityEngine.Random.Range(0, Enum.GetValues(typeof(Resource)).Length));
		string resourceName = getResourceName(resource);
		
		title = resourceName + " Crisis!";
		description = "The " + resourceName + " bubble has burst! And by that we mean that employees were claiming we had more inventory than we really had... " 
			+ "Our customers won't be happy, but our top accountant is most worried about the tax collectors reaction to our books! ";
		time = UnityEngine.Random.Range(60.0f, 120.0f);
		
		text = "Tell customers it's \"in the public delivery service\" and push this under some floor covering. Risky, but could be worth a shot.";
		float buyIt = UnityEngine.Random.Range(0.0f, 4.0f);
		if (buyIt <= 1.0f) {
			result = "Whew! Dodged that projectile! They bought it. Well, the lie, not the stuff. We don't have that. Not much was lost.";
		} else {
			result = "Oh dear. They say honesty is the best policy. That might be true... Look at these business losses!";
		}
		
		Site site = sites [((int)UnityEngine.Random.Range(0, sites.Count))];
		int minMoneyDelta = (int)UnityEngine.Random.Range(-40.0f * buyIt, -50.0f * buyIt);
		int maxMoneyDelta = (int)UnityEngine.Random.Range(-100.0f * buyIt, -400.0f * buyIt);
		int minResourceDelta = (int)UnityEngine.Random.Range(-40.0f, -50.0f);
		int maxResourceDelta = (int)UnityEngine.Random.Range(-100.0f, -400.0f);
		RandomEvent.Affect affect = new RandomEvent.Affect(business, resource, site, minMoneyDelta, maxMoneyDelta, minResourceDelta, maxResourceDelta);
		affects.Add(affect);
		
		RandomEvent.Option option1 = new RandomEvent.Option(text, affects, result);
		
		text = "Be honest. An uncommon idea in business, but at least no one can accuse us of lying twice!";
		result = "Okay. A few of our customers sent angry letters, and we had to pay out some refunds, " 
			+ "but at least the tax folks didn't crush us for this mistake.";
		buyIt = 1.0f;
		minMoneyDelta = (int)UnityEngine.Random.Range(-40.0f * buyIt, -50.0f * buyIt);
		maxMoneyDelta = (int)UnityEngine.Random.Range(-100.0f * buyIt, -400.0f * buyIt);
		minResourceDelta = (int)UnityEngine.Random.Range(-40.0f, -50.0f);
		maxResourceDelta = (int)UnityEngine.Random.Range(-100.0f, -400.0f);
		
		affects.Remove(affect);
		affect = new RandomEvent.Affect(business, resource, site, minMoneyDelta, maxMoneyDelta, minResourceDelta, maxResourceDelta);
		
		affects.Add(affect);
		RandomEvent.Option option2 = new RandomEvent.Option(text, affects, result);
		
		options.Add(option1);
		options.Add(option2);
		
		RandomEvent re = new RandomEvent(title, description, options, time);
		rep.Add(re);

		// -------------------------------------------------------- //

		// Next event here

		// -------------------------------------------------------- //
		
		/** at the end */
		return rep;
	}


	/**
	 * Gets the name of a resource with spaces
	 * (i.e. "RailroadTie" becomes "Railroad Tie"
	 * 
	 * @param res the resource to get the name of
	 * @return the name of the resource
	 */
	private static string getResourceName(Resource res) {
		string resourceName = Enum.GetName(typeof(Resource), res);
		if (resourceName == null) {
			return "";
		}
		int length = resourceName.Length;

		// Search for uppercase letters, starting at index 1 because 
		// the first will be uppercase
		for (int i = 1; i < length; i++) {
			// If we find an uppercase letter, put a space before it
			if (Char.IsUpper(resourceName [i])) {
				string half = resourceName.Substring(0, i);
				half = half + " " + resourceName.Substring(i);
				resourceName = half;
				length = resourceName.Length;
				i++; // Add 1 because we don't want to count the uppercase letter twice
			}
		}

		return resourceName;
	}
}
