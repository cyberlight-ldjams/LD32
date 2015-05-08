using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

/**
 * Determines the demand, and therefore price, of all resources
 */
public class Demand {

	/** Denotes the demand of an item is increadibly low */
	public const int GARBAGE = 0;

	/** Denotes the demand of an item is low */
	public const int USELESS = 1;

	/** Denotes the demand of an item is moderate */
	public const int CURIOSITY = 2;

	/** Denotes the demand of an item is high */
	public const int DESIRABLE = 3;

	/** Denotes the demand of an item is increadibly high */
	public const int MUSTHAVE = 4;

	/** For how many quarters the amount of supply should be relevant */
	private const int RELEVANT_QUARTERS = 12;

	/** A base price for all items, this is used along with supply and demand, to generate the current price */
	private const float basePrice = 1000.0f;

	/** The demand for items in the current age */
	private Dictionary<Resource, float> ageDemand { get; set; }

	/** The sales that have occured this quarter - this should eventually be converted into a Dictionary */
	private List<int[]> quartersSales;

	/**
	 * Creates a demand object for determining the demand of resources in stages
	 */
	public Demand() {
		ageDemand = new Dictionary<Resource, float>();
		quartersSales = new List<int[]>();
		generateAgeDemand(Stage.Archaic);
	}

	/**
	 * Adds the sales of items this quarter to the quarterSales list
	 * 
	 * @param the total sales from all sites and businesses this past quarter
	 */
	public void addQuarterSales(int[] quarterSales) {
		quartersSales.Add(quarterSales);
	}

	/**
	 * Gets the current price of a resource based on supply and demand
	 * 
	 * Supply only is relevant for the last RELEVANT_QUARTERS quarters
	 * 
	 * @param r the resource to get the price of
	 * @return the current price of the given resource
	 */
	public float getPrice(Resource r) {
		int rLoc = (int)r;
		int totalSales = 0;
		int relevantQuarters = RELEVANT_QUARTERS;
		for (int i = quartersSales.Count - 1; i > 0 && relevantQuarters > 0; i++) {
			totalSales = totalSales + quartersSales [i] [rLoc];
			relevantQuarters--;
		}

		float priceRatio = ageDemand [r] / totalSales;

		return priceRatio * basePrice;
	}

	/**
	 * Gets a ballpark estimate of the value of a resource based on its demand
	 * 
	 * Values returned are stored as constants in this class
	 * 
	 * This could be used to allow the player to perform "market research" on a given resource
	 * or to tell the player useful information in an event result
	 * 
	 * @param r the resource to get the category of
	 * @return the category of the resource
	 */
	public int getItemCategory(Resource r) {
		if (ageDemand [r] < 0.1f) {
			return GARBAGE;
		}
		if (ageDemand [r] < 0.3f) {
			return USELESS;
		}
		if (ageDemand [r] < 0.5f) {
			return CURIOSITY;
		}
		if (ageDemand [r] < 0.7f) {
			return DESIRABLE;
		}// else
		return MUSTHAVE;
	}

	/**
	 * A method for calling "generate(StageName)AgeDemand"
	 */
	public void generateAgeDemand(Stage age) {
		switch (age) {
			case Stage.Archaic:
				generateArchaicAgeDemand();
				break;
			case Stage.Antiquity:
				generateAntiquityAgeDemand();
				break;
			case Stage.Medieval:
				generateMedievalAgeDemand();
				break;
			case Stage.Renaissance:
				generateRenaissanceAgeDemand();
				break;
			case Stage.Machine:
				generateModernAgeDemand();
				break;
		}
	}

	/**
	 * Generates a demand list for the "Archaic" stage
	 */
	private void generateArchaicAgeDemand() {
		ageDemand = new Dictionary<Resource, float>();
		ageDemand.Add(Resource.Brick, UnityEngine.Random.Range(0.6f, 0.8f));
		ageDemand.Add(Resource.Clay, UnityEngine.Random.Range(0.3f, 0.6f));
		ageDemand.Add(Resource.ComputerChip, UnityEngine.Random.Range(0.03f, 0.1f));
		ageDemand.Add(Resource.Dinner, UnityEngine.Random.Range(0.6f, 0.8f));
		ageDemand.Add(Resource.Empty, 0);
		ageDemand.Add(Resource.Fish, UnityEngine.Random.Range(0.4f, 0.6f));
		ageDemand.Add(Resource.Meat, UnityEngine.Random.Range(0.4f, 0.6f));
		ageDemand.Add(Resource.Furniture, UnityEngine.Random.Range(0.6f, 0.7f));
		ageDemand.Add(Resource.Gold, UnityEngine.Random.Range(0.7f, 0.8f));
		ageDemand.Add(Resource.Iron, UnityEngine.Random.Range(0.1f, 0.2f));
		ageDemand.Add(Resource.Lamp, UnityEngine.Random.Range(0.5f, 0.7f));
		ageDemand.Add(Resource.Oil, UnityEngine.Random.Range(0.2f, 0.6f));
		ageDemand.Add(Resource.Plastic, UnityEngine.Random.Range(0.1f, 0.2f));
		ageDemand.Add(Resource.Pottery, UnityEngine.Random.Range(0.6f, 0.9f));
		ageDemand.Add(Resource.Stone, UnityEngine.Random.Range(0.4f, 0.6f));
		ageDemand.Add(Resource.Timber, UnityEngine.Random.Range(0.5f, 0.7f));
		ageDemand.Add(Resource.Weapon, UnityEngine.Random.Range(0.8f, 0.9f));
		ageDemand.Add(Resource.RailroadTie, UnityEngine.Random.Range(0.2f, 0.3f));
	}

	/**
	 * Generates a demand list for the "Antiquity" stage
	 */
	private void generateAntiquityAgeDemand() {
		generateArchaicAgeDemand(); // currently these are the same
	}

	/**
	 * Generates a demand list for the "Medieval" stage
	 */
	public void generateMedievalAgeDemand() {
		ageDemand = new Dictionary<Resource, float>();
		ageDemand.Add(Resource.Brick, UnityEngine.Random.Range(0.6f, 0.8f));
		ageDemand.Add(Resource.Clay, UnityEngine.Random.Range(0.3f, 0.6f));
		ageDemand.Add(Resource.ComputerChip, UnityEngine.Random.Range(0.1f, 0.15f));
		ageDemand.Add(Resource.Dinner, UnityEngine.Random.Range(0.5f, 0.7f));
		ageDemand.Add(Resource.Empty, 0);
		ageDemand.Add(Resource.Fish, UnityEngine.Random.Range(0.3f, 0.5f));
		ageDemand.Add(Resource.Meat, UnityEngine.Random.Range(0.3f, 0.5f));
		ageDemand.Add(Resource.Furniture, UnityEngine.Random.Range(0.7f, 0.9f));
		ageDemand.Add(Resource.Gold, UnityEngine.Random.Range(0.75f, 0.9f));
		ageDemand.Add(Resource.Iron, UnityEngine.Random.Range(0.4f, 0.6f));
		ageDemand.Add(Resource.Lamp, UnityEngine.Random.Range(0.6f, 0.8f));
		ageDemand.Add(Resource.Oil, UnityEngine.Random.Range(0.5f, 0.8f));
		ageDemand.Add(Resource.Plastic, UnityEngine.Random.Range(0.3f, 0.4f));
		ageDemand.Add(Resource.Pottery, UnityEngine.Random.Range(0.6f, 0.8f));
		ageDemand.Add(Resource.Stone, UnityEngine.Random.Range(0.4f, 0.6f));
		ageDemand.Add(Resource.Timber, UnityEngine.Random.Range(0.5f, 0.7f));
		ageDemand.Add(Resource.Weapon, UnityEngine.Random.Range(0.7f, 0.8f));
		ageDemand.Add(Resource.RailroadTie, UnityEngine.Random.Range(0.3f, 0.5f));
	}

	/**
	 * Generates a demand list for the "Renaissance" stage
	 */
	public void generateRenaissanceAgeDemand() {
		ageDemand = new Dictionary<Resource, float>();
		ageDemand.Add(Resource.Brick, UnityEngine.Random.Range(0.6f, 0.8f));
		ageDemand.Add(Resource.Clay, UnityEngine.Random.Range(0.2f, 0.45f));
		ageDemand.Add(Resource.ComputerChip, UnityEngine.Random.Range(0.2f, 0.3f));
		ageDemand.Add(Resource.Dinner, UnityEngine.Random.Range(0.45f, 0.65f));
		ageDemand.Add(Resource.Empty, 0);
		ageDemand.Add(Resource.Fish, UnityEngine.Random.Range(0.2f, 0.45f));
		ageDemand.Add(Resource.Meat, UnityEngine.Random.Range(0.2f, 0.45f));
		ageDemand.Add(Resource.Furniture, UnityEngine.Random.Range(0.6f, 0.7f));
		ageDemand.Add(Resource.Gold, UnityEngine.Random.Range(0.7f, 0.9f));
		ageDemand.Add(Resource.Iron, UnityEngine.Random.Range(0.7f, 0.8f));
		ageDemand.Add(Resource.Lamp, UnityEngine.Random.Range(0.7f, 0.8f));
		ageDemand.Add(Resource.Oil, UnityEngine.Random.Range(0.6f, 0.75f));
		ageDemand.Add(Resource.Plastic, UnityEngine.Random.Range(0.3f, 0.6f));
		ageDemand.Add(Resource.Pottery, UnityEngine.Random.Range(0.4f, 0.5f));
		ageDemand.Add(Resource.Stone, UnityEngine.Random.Range(0.2f, 0.4f));
		ageDemand.Add(Resource.Timber, UnityEngine.Random.Range(0.5f, 0.7f));
		ageDemand.Add(Resource.Weapon, UnityEngine.Random.Range(0.6f, 0.75f));
		ageDemand.Add(Resource.RailroadTie, UnityEngine.Random.Range(0.7f, 0.8f));
	}

	/**
	 * Generates a demand list for the "Modern" stage
	 */
	public void generateModernAgeDemand() {
		ageDemand = new Dictionary<Resource, float>();
		ageDemand.Add(Resource.Brick, UnityEngine.Random.Range(0.4f, 0.6f));
		ageDemand.Add(Resource.Clay, UnityEngine.Random.Range(0.15f, 0.4f));
		ageDemand.Add(Resource.ComputerChip, UnityEngine.Random.Range(0.8f, 0.9f));
		ageDemand.Add(Resource.Dinner, UnityEngine.Random.Range(0.4f, 0.6f));
		ageDemand.Add(Resource.Empty, 0);
		ageDemand.Add(Resource.Fish, UnityEngine.Random.Range(0.2f, 0.3f));
		ageDemand.Add(Resource.Meat, UnityEngine.Random.Range(0.2f, 0.3f));
		ageDemand.Add(Resource.Furniture, UnityEngine.Random.Range(0.6f, 0.7f));
		ageDemand.Add(Resource.Gold, UnityEngine.Random.Range(0.7f, 0.9f));
		ageDemand.Add(Resource.Iron, UnityEngine.Random.Range(0.5f, 0.6f));
		ageDemand.Add(Resource.Lamp, UnityEngine.Random.Range(0.25f, 0.4f));
		ageDemand.Add(Resource.Oil, UnityEngine.Random.Range(0.75f, 0.9f));
		ageDemand.Add(Resource.Plastic, UnityEngine.Random.Range(0.5f, 0.8f));
		ageDemand.Add(Resource.Pottery, UnityEngine.Random.Range(0.6f, 0.9f));
		ageDemand.Add(Resource.Stone, UnityEngine.Random.Range(0.5f, 0.7f));
		ageDemand.Add(Resource.Timber, UnityEngine.Random.Range(0.5f, 0.7f));
		ageDemand.Add(Resource.Weapon, UnityEngine.Random.Range(0.4f, 0.55f));
		ageDemand.Add(Resource.RailroadTie, UnityEngine.Random.Range(0.6f, 0.75f));
	}
}
