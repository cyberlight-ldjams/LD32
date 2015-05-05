using UnityEngine;
using System.Collections;

public class GoldMine : Quarry {
	public override Resource resourceProduced {	get { return Resource.Gold; } }
}