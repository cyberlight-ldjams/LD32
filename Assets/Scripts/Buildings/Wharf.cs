using UnityEngine;
using System.Collections;

public class Wharf : Quarry {
	public override Resource resourceProduced {	get { return Resource.Fish; } }
}
