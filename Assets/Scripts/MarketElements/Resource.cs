using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/**
 * The resources available in the game
 */
public enum Resource {
	Empty=-1,
	Clay,
	Pottery,
	Stone,
	Iron,
	Fish,
	Meat,
	Timber,
	Oil,
	Gold,
	Brick,
	Weapon,
	Dinner,
	Furniture,
	Plastic,
	Lamp,
	ComputerChip,
	RailroadTie
}
;

/**
 * A class that works closely with the Resource Enum
 */
public static class ResourceExtensions {
	public static Resource[] NaturalResources = new Resource[] {
		Resource.Clay,
		Resource.Stone,
		Resource.Iron,
		Resource.Fish,
		Resource.Meat,
		Resource.Timber,
		Resource.Oil,
		Resource.Gold
	};

	public static Material Material(this Resource resource) {
		switch (resource) {
			case Resource.Clay:
				return (Material)Resources.Load("ClayResource");
			case Resource.Stone:
				return (Material)Resources.Load("StoneResource");
			case Resource.Iron:
				return (Material)Resources.Load("IronResource");
			case Resource.Fish:
				return (Material)Resources.Load("FishResource");
			case Resource.Meat:
				return (Material)Resources.Load("MeatResource");
			case Resource.Timber:
				return (Material)Resources.Load("TimberResource");
			case Resource.Oil:
				return (Material)Resources.Load("OilResource");
			case Resource.Gold:
				return (Material)Resources.Load("GoldResource");
			default:
				return new Material("");
		}
	}

	private static System.Random randGen = new System.Random();

	public static List<T> ChooseRandom<T>(T[] list, int count) {
		List<T> result = new List<T>();

		int remainingToChoose = count;
		int remainingTotal = list.Length;

		if (remainingToChoose > remainingTotal) {
			throw new ArgumentException(string.Format("Cannot choose {0} elements from array of length {1}", remainingToChoose, remainingTotal));
		}

		foreach (T item in list) {
			if (randGen.Next(remainingTotal) < remainingToChoose) {
				result.Add(item);
				remainingToChoose--;
			}

			remainingTotal--;
		}

		return result;
	}

	public static List<Resource> RandomResources(int count) {
		return ChooseRandom(NaturalResources, count);
	}
}