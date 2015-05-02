using UnityEngine;
using System.Collections;

/**
 * Superclass for all workshops
 */
public abstract class Workshop : Building {

	/** The resource this workshop uses */
	public abstract Resource resourceUsed { get; }

	/** The good this workshop produces */
	public abstract Resource goodProduced { get; }
}