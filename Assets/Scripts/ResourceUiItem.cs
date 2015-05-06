using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

public class ResourceUiItem : MonoBehaviour {

	public Resource myResource;
	public int yPos;
	public bool yPosSet = false;

	public InputField input;

	public Text resourceName, stock;

	// Use this for initialization
	void Start() {


		resourceName.text = Enum.GetName(typeof(Resource), myResource);
		int temp = (int)(GameDirector.THIS.playerBusiness.myInventory.getAmountOfAt(myResource, GameDirector.THIS.currentSite));
		stock.text = temp.ToString();
	
	}
	
	// Update is called once per frame
	void Update() {

		if (GameDirector.PAUSED) {
			return;
		}

		//sell item
		if (input.isFocused && input.text != "" && Input.GetKey(KeyCode.Return)) {
			int amount = 0;
			bool success = Int32.TryParse(input.text, out amount);

			//was it an integer?
			if (success) {
				input.text = "";
				//sell the amount of resource at site
				GameDirector.THIS.playerBusiness.SellResource(GameDirector.THIS.currentSite, myResource, amount);
			}
		}

		int temp = (int)(GameDirector.THIS.playerBusiness.myInventory.getAmountOfAt(myResource, GameDirector.THIS.currentSite));
		stock.text = temp.ToString();

		RectTransform rect = this.gameObject.GetComponent<RectTransform>();
		if (!yPosSet) {
			Debug.Log(this.GetHashCode());
			rect.position = new Vector3(rect.position.x, yPos, rect.position.z);
			yPosSet = !yPosSet;
		}
		
	}
}
