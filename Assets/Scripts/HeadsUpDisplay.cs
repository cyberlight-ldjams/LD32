using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class HeadsUpDisplay : MonoBehaviour {

	public GameObject clayText;
	public GameObject potteryText;
	public PlayerBusiness business { get; private set; }
	public Site currentSite;
	public GameDirector gameDirector;

	public GameObject dialog;

	private Button eventBtn1, eventBtn2, eventBtn3, eventBtn4;

	private Slider eventTimerSlider;


	private Button quarryBtn;
	private Button workshopBtn;
	private Text title, description, afterText;


	// Use this for initialization
	void Start () {
		Button[] buttons = Object.FindObjectsOfType<Button> ();
		foreach (Button but in buttons) {
			string butName = but.name;

			switch (butName) {
			case  "QuarryButton":
				quarryBtn = but;
				break;
			case "WorkshopButton":
				workshopBtn = but;
				break;
			
			case "REButton1":
				eventBtn1 = but;
				break;
			case "REButton2":
				eventBtn2 = but;
				break;
			case "REButton3":
				eventBtn3 = but;
				break;
			case "REButton4":
				eventBtn4 = but;
				break;
			}

		}

		Text [] temp = dialog.GetComponentsInChildren<Text> ();
		foreach (Text t in temp) {
			string textName = t.name;

			switch (textName) {
			case "Title":
				title = t;
				break;
			case "Description":
				description = t;
				break;
			case "afterText":
				afterText = t;
				break;
			}
		}

		Slider [] s = Object.FindObjectsOfType<Slider> ();
		foreach (Slider s1 in s) {
			if (s1.name.Equals ("Timer")) {
				eventTimerSlider = s1;
				break;
			}
		}

		business = gameDirector.playerBusiness;
	}
	
	// Update is called once per frame
	void Update () {
 		int clayAmount = (int)business.myInventory.getAmountOfAt(Resource.Clay, currentSite);
		clayText.GetComponent<Text>().text = string.Format("Clay: {0}", clayAmount);

		int potteryAmount = (int)business.myInventory.getAmountOfAt(Resource.Pottery, currentSite);
		potteryText.GetComponent<Text> ().text = string.Format ("Pottery: {0}", potteryAmount);


		if (quarryBtn != null && workshopBtn != null) {
			//dissalow placement before selection
			if ((gameDirector == null || gameDirector.selectedObject == null)) {
				quarryBtn.enabled = false;
				workshopBtn.enabled = false;
			} else {
				quarryBtn.enabled = true;
				quarryBtn.enabled = true;
			}
		}
	}


}
