using UnityEngine;
using System.Collections;

/**
 * Superclass for all quarries, 
 */
public abstract class Quarry : Building {

	/** The resource this quarry produces */
	public abstract Resource resourceProduced { get; }

}