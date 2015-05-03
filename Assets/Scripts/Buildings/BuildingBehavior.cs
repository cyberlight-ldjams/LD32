using UnityEngine;
using System.Collections;

public class BuildingBehavior : MonoBehaviour {

	public Building Building;

	public void Update() {
		if (Building != null)
			Building.Produce();
	}

}