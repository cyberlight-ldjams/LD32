using System;
using System.Collections.Generic;
using System.Linq;

public enum Resource
{
	Empty=-1, Clay, Pottery, Stone, Iron, Fish, Meat, Timber, Oil, Gold, Brick, Weapon, Dinner, Furniture, Plastic, Lamp, ComputerChip, RailroadTie
};

public static class ResourceExtensions {
	public static Resource[] NaturalResources = new Resource[] { Resource.Clay, Resource.Stone, Resource.Iron, Resource.Fish, Resource.Meat, Resource.Timber, Resource.Oil, Resource.Gold };

	private static Random randGen = new Random();

	public static List<T> ChooseRandom<T>(T[] list, int count) {
		List<T> result = new List<T>();

		int remainingToChoose = count;
		int remainingTotal = list.Length;

		if (remainingToChoose > remainingTotal)
			throw new ArgumentException(string.Format("Cannot choose {0} elements from array of length {1}", remainingToChoose, remainingTotal));

		foreach (T item in list) {
			if (randGen.Next(remainingTotal) < remainingToChoose)
			{
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