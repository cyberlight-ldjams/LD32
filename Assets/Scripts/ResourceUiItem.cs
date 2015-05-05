using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

public class ResourceUiItem : MonoBehaviour {

	public Resource myResource;
	public int yPos;
	public GameDirector director;

	private InputField input;

	private Text resourceName, stock;

	// Use this for initialization
	void Start () {

		input = this.gameObject.GetComponent<InputField> ();
		Text [] texts = this.gameObject.GetComponents<Text> ();

		foreach (Text t in texts) {
			if (t.name == "inStock") {
				stock = t;
			} else if (t.name == "resourceName") {
				resourceName = t;
			}
		}

		resourceName.text = Enum.GetName (typeof(Resource), myResource);
		int temp = (int)(director.playerBusiness.myInventory.getAmountOfAt (myResource, director.currentSite));
		stock.text = temp.ToString ();
	
	}
	
	// Update is called once per frame
	void Update () {

		//sell item
		if(input.isFocused && input.text != "" && Input.GetKey(KeyCode.Return)) {
			int amount = 0;
			bool success = Int32.TryParse(input.text, out amount);

			//was it an integer?
			if(success) {
				input.text = "";
				//sell the amount of resource at site
				director.playerBusiness.SellResource(director.currentSite, myResource, amount);
			}
		}

		int temp = (int)(director.playerBusiness.myInventory.getAmountOfAt (myResource, director.currentSite));
		stock.text = temp.ToString ();
	
	}
}
