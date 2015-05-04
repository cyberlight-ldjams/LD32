using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

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

	private Dictionary<Resource, float> ageDemand { get; set; }

	public Demand() {
		ageDemand = new Dictionary<Resource, float>();
	}

	public void generateAgeDemand(Stage age) {
		switch (age) {
		case Stage.Archaic: generateArchaicAgeDemand();
			break;
		case Stage.Antiquity: generateAntiquityAgeDemand();
			break;
		case Stage.Medieval: generateMedievalAgeDemand();
			break;
		case Stage.Renaissance: generateRenaissanceAgeDemand();
			break;
		case Stage.Machine: generateModernAgeDemand();
			break;
		}
	}

	public void generateArchaicAgeDemand() {
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

	public void generateAntiquityAgeDemand() {
		generateArchaicAgeDemand(); // currently these are the same
	}

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

	public int getItemCategory(Resource r) {
		if (ageDemand[r] < 0.1f) {
			return GARBAGE;
		}
		if (ageDemand[r] < 0.3f) {
			return USELESS;
		}
		if (ageDemand[r] < 0.5f) {
			return CURIOSITY;
		}
		if (ageDemand[r] < 0.7f) {
			return DESIRABLE;
		}// else
		return MUSTHAVE;
	}
}
