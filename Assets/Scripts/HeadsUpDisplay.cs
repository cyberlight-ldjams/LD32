using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class HeadsUpDisplay : MonoBehaviour {

	public GameObject clayText;
	public GameObject potteryText;
	public PlayerBusiness business;
	public Site currentSite;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		Debug.Log(business);
		Debug.Log(business.myInventory);
		Debug.Log(currentSite);
		double clayAmount = business.myInventory.getAmountOfAt(Resource.Clay, currentSite);
		clayText.GetComponent<Text>().text = string.Format("Clay: {0}", clayAmount);

		double potteryAmount = business.myInventory.getAmountOfAt(Resource.Pottery, currentSite);
		potteryText.GetComponent<Text>().text = string.Format("Pottery: {0}", potteryAmount);
	}
}
