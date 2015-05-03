using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class HeadsUpDisplay : MonoBehaviour {

	public GameObject clayText;
	public GameObject potteryText;
	public GameObject employeeText;
	public PlayerBusiness business { get; private set; }
	public Site currentSite;
	public GameDirector gameDirector;


	private Button quarryBtn;
	private Button workshopBtn;



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

		int employees = business.myInventory.GetEmployeesAt(currentSite);
		employeeText.GetComponent<Text>().text = string.Format ("Employees: {0}", employees);

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
