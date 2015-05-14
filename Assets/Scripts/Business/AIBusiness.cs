using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class AIBusiness : Business {

	// // STANCES OF AI BUSINESSES // //

	public const int AGGRESSIVE = 0;

	public const int PASSIVE = 1;

	public const int NEUTRAL = 2;

	// // AFFINITIES FOR AI BUSINESSES (OPTIONAL) // //

	public const int NO_AFFINITY = 0;

	public const int WATER_AFFINITY = 1;

	public const int MINING_AFFINITY = 2;

	public const int FOREST_AFFINITY = 3;


	/** Creates an AI business to denote that a lot is unowned */
	public static AIBusiness UNOWNED = new AIBusiness(true);

	/** The business' stance */
	public int stance { get; private set; }

	/** The business' affinity */
	public int affinity { get; private set; }
	
	/** Counter for getting AI Business colors */
	private static List<Color> usedColors;

	/**
	 * Creates a Business AI given a name, stance, and color
	 */
	public AIBusiness(string name, int stance, Color color) {
		this.name = name;
		this.stance = stance;
		this.businessColor = color;
		setBusinessColor(color);
		Init();
	}

	/**
	 * Creates a Business AI with a random name and a random stance
	 */
	public AIBusiness() {
		MakeRandomBusiness();
	}

	/**
	 * Makes a business with a random name and stance and color
	 */
	private void MakeRandomBusiness() {
		name = RandomNameGenerator.generateBusinessName();

		int rand1 = UnityEngine.Random.Range(0, 3);
		stance = rand1;
		
		businessColor = getBusinessColor();
		Init();
	}

	/**
	 * Creates the "UNOWNED" business
	 */
	private AIBusiness(bool unowned) {
		if (unowned) {
			name = "Unowned";
			stance = NEUTRAL;

			// Color is a light gray
			businessColor = new Color(0.8f, 0.8f, 0.8f);

			// Let the director know we used the color
			setBusinessColor(businessColor);
		} else {
			MakeRandomBusiness();
		}
	}

	/**
	 * Randomizes the contents of a list
	 * 
	 * @param list the list to randomize
	 */
	private void randomizeList<T>(List<T> list) {
		for (int i = 0; i < list.Count; i++) {
			int rand = UnityEngine.Random.Range(0, list.Count);
			T t = list [rand];
			list [rand] = list [i];
			list [i] = t;
		}
	}

	/**
	 * Causes the business to perform an action
	 * 
	 * As a general rule, only one action should be performed per call - so return after doing something
	 */
	public void performAction() {
		// // MAKE DECISIONS BASED ON MONEY, RESOURCES, EMPLOYEES, AVAILABLE LOTS, ETC. // //


		// // BUILDING BUYING AND LOT LEASING AI // //

		// This determines the minimum amount of money the AI feels it needs in order to decide it should
		// build a building or lease a lot
		double minMoney = 5000.0;
		double stanceChange = 1500.0;

		if (stance == AGGRESSIVE) {
			stanceChange *= 0;
		} else if (stance == PASSIVE) {
			stanceChange *= 2;
		} // Else, NEUTRAL stance, no change, use the default value

		minMoney = minMoney + stanceChange;

		// If money is plentiful, place a building on an existing lot, or lease another lot
		if (myInventory.getBaseCurrency() >= minMoney) {
			// See if there are any owned lots without buildings, if there are, buy one
			foreach (Lot buildOn in myLots) {
				// If the lot already has a building, move on
				if (buildOn.Building != null) {
					continue;
				}

				// Get a list of all the other lots owned by this AI at this site that have a building
				List<Lot> atSite = new List<Lot>();
				foreach (Lot onSite in buildOn.Site.Lots) {
					if (onSite.Owner == this && onSite.Building != null) {
						atSite.Add(onSite);
					}
				}

				// Check these buildings, see if any of them are quarries
				// If we find a quarry, see if it already has a workshop for the resource
				foreach (Lot other in atSite) {
					if (other.Building.GetType() == typeof(Quarry)) {
						Quarry q = (Quarry)other.Building;
						Workshop w = null;
						foreach (Lot workshop in atSite) {
							if (workshop.Building.GetType() == typeof(Workshop)) {
								w = (Workshop)workshop.Building;
								if (q.resourceProduced == w.resourceUsed) {
									// We found a match! Move on...
									break;
								} else {
									w = null;
									continue;
								}
							}
						}
						// If there is no compatable workshop for this quarry, build one!
						if (w == null) {
							try {
								GameDirector.THIS.lm.InstallBuilding(Workshop.NewAppropriateWorkshop(q.resourceProduced), this, buildOn);
								return; // We built the building. Action complete.
							} catch (ArgumentException ae) {
								// There is no compatible workshop
								Debug.LogWarning(ae.Message);
							}
						}
					}
				}

				// If the lot has a resource that isn't being used, build a quarry there
				if (buildOn.Resource.HasValue) {
					GameDirector.THIS.lm.InstallBuilding(Quarry.NewAppropriateQuarry(buildOn.Resource.Value), this, buildOn);
					return; // We built the building, action complete.
				} else {
					// This lot has no resources on it, but we also don't have an appropriate workshop to build
					// So we'll use this lot for storage and wait on building something here
				}
			}

			// Lease a lot if we don't have a building to build
			// So that all AIs don't rush to one lot, we randomize the order lots are searched in
			List<Site> lotSearch = new List<Site>(GameDirector.THIS.world.sites);
			randomizeList<Site>(lotSearch);

			List<Lot> potential = new List<Lot>(); // Lots that have potential to be purchased
			List<Lot> resourceless = new List<Lot>(); // Lots without resources, only used if there are no potential lots

			// Prioritize first lots with resources, and second lots on sites where we have other lots already
			foreach (Site s in lotSearch) {
				foreach (Lot l in s.Lots) {
					// If the lot isn't unowned, move on
					if (l.Owner != UNOWNED) {
						continue;
					}
					// If the lot is unowned and has a resource, it has potential
					if (l.Resource.HasValue) {
						potential.Add(l);
					}
				}
			}

			// If no lots have potential, now get the lots that have no resources
			if (potential.Count == 0) {
				potential = resourceless;
			}

			// Now randomize the lots also so that AIs don't have a lot purchase preference
			randomizeList<Lot>(potential);

			// See if any of these lots are on a site where another lot is owned by this business
			foreach (Lot toBuy in potential) {
				foreach (Lot alsoOwned in toBuy.Site.Lots) {
					// If we own another lot at this site, that's the green-light - buy it!
					if (alsoOwned.Owner == this) {
						GameDirector.THIS.sales.leaseLot(this, toBuy);
						return; // Lot purchased. Action complete.
					}
				}
			}

			// If we never bought a lot from all of that, buy the first lot that had potential
			if (potential.Count > 0) {
				GameDirector.THIS.sales.leaseLot(this, potential [0]);
				return; // Lot purchased. Action complete.
			}

			// If we get to this point, that means that there are no lots to sell and no buildings to build
			// Guess we need to do something else for this action...
		}


		// // EMPLOYMENT LABOR CAP AND WAGE AI // //

		// If the business is aggressive, they should max out the labor cap and offer the lowest possible wages
		// If the business is passive, they should offer competetive wages but have a low labor cap



		// // RESOURCE SELLING AND TRANSPORTING AI // //

		// If the business is aggressive, they should sell as soon as possible, probably transporting nothing
		// If the business is passive, they should sell when they need money and be willing to transport resources to improve them



		// // RUNNING LOW ON MONEY - PANIC MODE // //

		// If the buisness is aggressive, they should be quick to cut wages/employees, sell buildings/leases
		// If the business is passive, they should hold off on cutting wages/employees, selling buildings/leases
	}


	/**
	 * Method called on the failure of an AI business
	 */
	public void Failure() {
		//TODO : make this do something

		// Probably sell all buildings, sell all lots, and sell off all resources to "pay creditors"
	}

	/**
	 * Gets a color for an AI businesses
	 * 
	 * Generates a random color from all possible colors
	 * 
	 * @return the color
	 */
	public static Color getBusinessColor() {
		if (usedColors == null) {
			usedColors = new List<Color>();
		}

		int tries = 0;
		Color color = new Color(UnityEngine.Random.Range(0.0f, 1.0f), UnityEngine.Random.Range(0.0f, 1.0f), 
		                        UnityEngine.Random.Range(0.0f, 1.0f));
		// If this color was already used, and we haven't tried 1000 times, keep getting a new one
		while (colorUsed(color) && tries < 1000) {
			color = new Color(UnityEngine.Random.Range(0.0f, 1.0f), UnityEngine.Random.Range(0.0f, 1.0f), 
			                  UnityEngine.Random.Range(0.0f, 1.0f));
		}
		return color;
	}
	
	/**
	 * Lets the director know that a business has used a color
	 */
	private static void setBusinessColor(Color color) {
		if (usedColors == null) {
			usedColors = new List<Color>();
		}

		if (!usedColors.Contains(color)) {
			usedColors.Add(color);
		}
	}
	
	/**
	 * Checks if this, or a similar color, has been used as a business color
	 * 
	 * @param color the color to check against
	 * @return whether the color, or a similar one, was used already
	 */
	private static bool colorUsed(Color color) {
		if (usedColors == null) {
			usedColors = new List<Color>();
		}

		foreach (Color used in usedColors) {
			if (Mathf.Abs(color.r - used.r) < .1f && Mathf.Abs(color.g - used.g) < .1f && Mathf.Abs(color.b - used.b) < .1f) {
				return true;
			}
		}
		return false;
	}
}
