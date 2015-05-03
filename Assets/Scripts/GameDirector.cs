using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameDirector : MonoBehaviour {

	public int lotsPerSite;

	public GameObject selectedObject;

	public GameObject player;

	public List<GameObject> enemyAI;
	
	private Site site;

	public PlayerBusiness playerBusiness;

	public HeadsUpDisplay headsUpDisplay;

	private GameObject selection;

	private Vector3 WORLDVIEW = new Vector3(0, 800, 0);

	private Vector3 HOMESITE = new Vector3(5, 20, 0);

	private Vector3 desiredCameraPosition = new Vector3(5, 20, 0);

	private int timer;

	// Use this for initialization
	void Start () {
		site = new Site(lotsPerSite, playerBusiness);

		headsUpDisplay.currentSite = site;
		headsUpDisplay.business = playerBusiness;

		selection = (GameObject) GameObject.CreatePrimitive(PrimitiveType.Plane);
		selection.SetActive(false);
	}

	private void platformSpecific() {
		switch (Application.platform) {
		case RuntimePlatform.OSXPlayer:
		case RuntimePlatform.WindowsPlayer:
		case RuntimePlatform.LinuxPlayer:
			if (Input.GetKeyDown (KeyCode.Escape)) {
				Application.Quit ();
			}
			break;
		}

	}

	public void InstallBuilding (Building b) {
		Lot.InstallBuilding (selectedObject, site, b);
	}

	public void InstallClayPit () {
		InstallBuilding(new ClayPit());
	}

	public void InstallPotteryWorkshop () {
		InstallBuilding(new PotteryWorkshop());
	}

	private void randomAssign() {
		foreach(Lot l in site.Lots) {
			int winner = Random.Range (0, 1);
			if(winner == 0) {
				//l.Owner = player;
			} else {
				//sl.Owner = enemyAI[0];
			}
		}
	}

	private GameObject calculateSelectedObject() {
		Debug.Log ("Click");
		Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
		RaycastHit hit;
		Physics.Raycast (ray, out hit);
		Debug.Log ("Hit at: " + hit.point);
		if (hit.collider == null) {
			return selectedObject;
		} else {
			selection.SetActive(true);
			selection.layer = 2;
			Renderer r = selection.GetComponent<Renderer>();
			r.material.color = Color.red;
			selection.transform.position = 
				new Vector3(hit.collider.gameObject.transform.position.x, hit.collider.gameObject.transform.position.y - 0.01f, hit.collider.gameObject.transform.position.z);
			selection.transform.localScale = new Vector3(hit.collider.gameObject.transform.localScale.x + 0.02f, 1, hit.collider.gameObject.transform.localScale.z + 0.02f);
			return hit.collider.gameObject;
		}
	}

	private void moveCamera() {
		Debug.Log(Camera.main.transform.position);
		Debug.Log(desiredCameraPosition);
		Vector3 distance = (desiredCameraPosition - Camera.main.transform.position) / 3;
		Camera.main.transform.Translate(distance * Time.deltaTime * 20, Space.World);
		if (timer > 80 || (timer > 45 && desiredCameraPosition.y > 400) || (distance.x < 0.0001f && distance.y < 0.0001f && distance.z < 0.0001f)) {
			Camera.main.transform.position = desiredCameraPosition;
		} 
	}

	// Update is called once per frame
	void Update () {
		platformSpecific ();

		if (Input.GetMouseButtonDown (0)) {
			selectedObject = calculateSelectedObject ();
		}
		if (Input.GetKeyDown(KeyCode.F2)) {
			desiredCameraPosition = WORLDVIEW;
			timer = 0;
		}
		if (Input.GetKeyDown(KeyCode.F1)) {
			desiredCameraPosition = HOMESITE;
			timer = 0;
		}
		if (Camera.main.transform.position != desiredCameraPosition) {
			moveCamera();
			timer++;
		}
	}
}
