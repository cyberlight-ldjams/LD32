using UnityEngine;
using System.Collections;

/**
 * Superclass for all buildings
 */
public abstract class Workshop : Building {

	/** The resource this workshop uses */
	public readonly Resources resourceUsed;

	/** The good this workshop produces */
	public readonly Resources goodProduced;
}