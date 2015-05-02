using UnityEngine;
using System.Collections;

public class StoneQuarry : Quarry {
	public override Resource resourceProduced {	get { return Resource.Stone; } }
}