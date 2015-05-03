using UnityEngine;
using System.Collections;

public class OilDerrik : Quarry {
	public override Resource resourceProduced {	get { return Resource.Oil; } }
}