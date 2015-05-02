using UnityEngine;
using System.Collections;

/**
 * Superclass for all buildings
 */
public abstract class Workshop : Building {

	/** The resource this workshop uses */
	public Resource resourceUsed;

	/** The good this workshop produces */
	public Resource goodProduced;
}