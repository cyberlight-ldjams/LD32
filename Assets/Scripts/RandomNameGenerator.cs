using UnityEngine;
using System.Collections;

/**
 * Generates random names for sites, businesses, and employees
 * 
 * Feel free to add more names
 */
public class RandomNameGenerator {
	
	/** Possible first names */
	private static string[] firstName = {"Elmer", "Rene", "Victor", "Pamela", "Rosemarie", "Claude", "Sherri", "Al", 
		"Cornelius", "Clyde", "Malcolm", "Robert", "Allison", "Oscar", "Jerome", "Elsa", "Donald", "Devin", 
		"Murphy", "Milton", "Ivan", "Leslie", "Edgar", "Winifred", "Leo", "Gabriel", "Kelvin", "Ted", 
		"Theodore", "Mona", "Derek", "Rodney", "Vivian", "Vera", "Samantha", "Patricia", "Stanley", "Tanner",
		"Madeline", "Rose", "Karen", "Sandra", "Michael", "Cecilia", "Jason", "Roland"
	};
	
	/** Possible last names */
	private static string[] lastName = {"Cobb", "Saunders", "Huff", "Wilkerson", "Barker", 
		"Beck", "Reed", "Valdez", "Jenkins", "Delgado", "Webb", "Horton", "Larson", "Cooper", 
		"Day", "Christensen", "Palmer", "Hunt", "Marsh", "Santos", "Bowman", "Abbott", "Blair",
		"Young", "Simpson", "Holmes", "Gibb", "Jordan", "Fisher", "Adkinson", "Bishop", "Warner",
		"Hart", "Goodwin", "Alexander", "Bryant", "Park", "Mey", "Swett", "Knooihuisen",
		"Hammond", "Brooks", "Price", "Fowler", "Pitman", "Nguyen", "Carter", "Hudson",
		"Madison", "Washington", "Jefferson", "Carson"
	};

	/** Suffixes for business names - if the suffix needs a space beforehand, add one */
	private static string[] businessTypes = {" Corp.", " Factories", " Firm", " Institute", " Venture", " Cartel", 
		" Syndicate", " Trust", " Megacorp", " Mom and Pop", " Business", " Stores"
	};

	/** Suffixes for place names - if the suffix needs a space beforehand, add one */
	private static string[] placeTypes = {
		"bluff", "burg", "view", "ville", "shire", " Abbey", "mount", "field", "keep", "town", " City", 
		" Lake", "hedge", "ford", "ton", " Rapids", "bury", " Place", " Creek", "chester", "land", "ston", " Run",
		"'s Mill", "'s Hill", "'s Landing", "stead",
	};

	/** Common words to use at the start of place names */
	private static string[] commonWords = {"People", "Way", "Day", "Living", "State", "Family", "Car", "Law",
		"Number", "Night", "Level", "Snow", "Apple", "Orange", "Red", "Blue", "Green", "Black", "Yellow", 
		"Indigo", "Violet", "Rain", "North", "South", "East", "West", "Rock", "Water", "Fire", "Air", "Peace", 
		"War", "Spring", "Fall", "Winter", "Summer", "Chester", "Clay", "Center", "Kings", "Queens", "Knight",
		"Win", "Wind", "Oak", "Cedar", "Maple", "Fair", "New", "Old", "Bright", "Dark", "White", "Light",
		"Night", "Hud", "Bristol", "Ash", "Free", "Ox", "Bull"
	};

	/**
	 * Generates a name for a business
	 * 
	 * This is a person's full name followed by a business type
	 */
	public static string generateBusinessName() {
		string name = "";
		
		int rand1 = Random.Range(0, businessTypes.Length);
		
		name = generatePersonName() + businessTypes [rand1];
		return name;
	}

	/**
	 * Generates a person's name - first and last
	 */
	public static string generatePersonName() {
		string name = "";

		int rand1 = Random.Range(0, firstName.Length);
		int rand2 = Random.Range(0, lastName.Length);

		name = firstName [rand1] + " " + lastName [rand2];

		return name;
	}

	/**
	 * Generates a place name
	 * 
	 * This is either a person's first name, last name, or a common word followed by a place type
	 */
	public static string generatePlaceName() {
		float rand1 = Random.Range(0.0f, 1.0f);
		
		if (rand1 > 0.5f) {
			return generatePersonPlaceName();
		} else {
			return generateGenericPlaceName();
		}
	}

	/**
	 * Generates a place name using a person's name
	 * 
	 * This is either a person's first name or last name followed by a place type
	 */
	public static string generatePersonPlaceName() {
		string name = "";

		int rand1 = Random.Range(0, firstName.Length);
		int rand2 = Random.Range(0, lastName.Length);
		int rand3 = Random.Range(0, placeTypes.Length);
		float rand4 = Random.Range(0.0f, 1.0f);

		if (rand4 > 0.5f) {
			name = checkName(firstName [rand1], placeTypes [rand3]);
		} else {
			name = checkName(lastName [rand2], placeTypes [rand3]);
		}

		return name;
	}

	/**
	 * Generates a place name
	 * 
	 * This is a common word followed by a place type
	 */
	public static string generateGenericPlaceName() {
		string name = "";

		int rand1 = Random.Range(0, commonWords.Length);
		int rand2 = Random.Range(0, placeTypes.Length);

		name = checkName(commonWords [rand1], placeTypes [rand2]);
		
		return name;
	}

	/**
	 * Adds some intelligence to the formation of place names
	 */
	private static string checkName(string part1, string part2) {
		// No "Washtingtonton" or "Chesterchester"
		if (part1.Contains(part2)) {
			return part1;
		}

		// No "Gibbsston" or "Bullland"
		if (part2.StartsWith(part1 [part1.Length - 1] + "")) {
			part2 = part2.Substring(1);
		}
		// Change "Gibbs's Mill" to "Gibbs' Mill"
		else if (part1.EndsWith("s") && part2.StartsWith("'s")) {
			part2 = part2.Remove(1, 1);
		}

		string name = part1 + part2;

		return name;
	}
}
