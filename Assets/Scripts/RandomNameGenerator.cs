using UnityEngine;
using System.Collections;

public class RandomNameGenerator {

	private static string[] businessTypes = {"Corp.", "Factories", "Firm", "Institute", "Venture", "Cartel", "Syndicate", "Trust", "Megacorp", 
		"Mom and Pop", "Business", "Stores"};

	private static string[] placeTypes = {"bluff", "burg", "view", "ville", "shire", " Abbey", "ston", "mount", "field", "'s Mill", "'s Hill", 
		"keep", "town", " City", " Lake", "hedge", "ford", "ton", " Rapids", "'s Landing", "bury"};

	private static string[] firstName = {"Elmer", "Rene", "Victor", "Pamela", "Rosemarie", "Claude", "Sherri", "Al", "Cornelius", "Clyde", "Malcolm", 
		"Oscar", "Jerome", "Elsa", "Donald", "Devin" };

	private static string[] lastName = {"Cobb", "Saunders", "Huff", "Wilkerson", "Barker", "Beck", "Reed", "Valdez", "Jenkins", "Delgado", "Webb", 
		"Horton", "Larson", "Cooper", "Day", "Christensen" };

	private static string[] commonWords = {"People", "Way", "Day", "Living", "State", "Family", "Car", "Law", "Number", "Night", "Level", "Snow", 
		"Apple", "Orange", "Red", "Blue", "Green", "Black", "Yellow", "Indigo", "Violet", "Rain", "North", "South", "East", "West", "Rock", "Water", 
		"Fire", "Air", "Peace", "War"};

	public static string generateBusinessName() {
		string name = "";
		
		int rand1 = Random.Range(0,businessTypes.Length);
		
		name = generatePersonName() + " " + businessTypes[rand1];
		return name;
	}

	public static string generatePersonName() {
		string name = "";

		int rand1 = Random.Range(0, firstName.Length);
		int rand2 = Random.Range(0, lastName.Length);

		name = firstName[rand1] + " " + lastName[rand2];

		return name;
	}

	public static string generatePersonPlaceName() {
		string name = "";

		int rand1 = Random.Range(0, firstName.Length);
		int rand2 = Random.Range(0, lastName.Length);
		int rand3 = Random.Range(0, placeTypes.Length);
		float rand4 = Random.Range(0.0f, 1.0f);

		if (rand4 > 0.5f) {
			name = firstName[rand1] + placeTypes[rand3];
		} else {
			name = lastName[rand2] + placeTypes[rand3];
		}

		return name;
	}

	public static string generateGenericPlaceName() {
		string name = "";

		int rand1 = Random.Range(0, commonWords.Length);
		int rand2 = Random.Range(0, placeTypes.Length);

		name = commonWords[rand1] + placeTypes[rand2];
		
		return name;
	}

	public static string generatePlaceName() {
		float rand1 = Random.Range(0.0f, 1.0f);
		
		if (rand1 > 0.5f) {
			return generatePersonPlaceName();
		} else {
			return generateGenericPlaceName();
		}
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
