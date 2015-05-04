using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameDirector : MonoBehaviour {

	public const int DEFAULT = 0;

	public const int IGNORE_RAYCAST = 2;

	private const float SITE_CAMERA_HEIGHT = 25.0f;

	public int lotsPerSite;

	public GameObject selectedObject;

	public List<GameObject> enemyAI;
	
	private Site homesite;

	private Site currentSite;

	public World world;

	public PlayerBusiness playerBusiness { get; private set; }

	public HeadsUpDisplay headsUpDisplay;

	public GameObject sidebar;

	private GameObject selection;

	private Vector3 WORLDVIEW = new Vector3(0, 850.0f, -200.0f);

	private Vector3 HOMESITE = new Vector3(5, 25, -5);

	private Vector3 desiredCameraPosition;

	private int timer;

	public EventManager em;
	// Use this for initialization
	void Start () {
		playerBusiness = new PlayerBusiness ();
		//playerBusiness.myInventory.SetEmployeesAt(homesite, 8);

		desiredCameraPosition = HOMESITE;

		selection = (GameObject) GameObject.CreatePrimitive(PrimitiveType.Plane);
		selection.layer = IGNORE_RAYCAST;
		selection.SetActive(false);
		List<Business> temp = new List<Business> ();
		temp.Add (playerBusiness);
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
		Lot.InstallBuilding (selectedObject, homesite, b);
	}

	public void InstallClayPit () {
		InstallBuilding(new ClayPit());
	}

	public void InstallPotteryWorkshop () {
		InstallBuilding(new PotteryWorkshop());
	}

	public void DeployEmployee() {
		Building building = Lot.FindLot(selectedObject, currentSite).Building;
		int employeesAvailable = playerBusiness.myInventory.GetEmployeesAt(currentSite);

		if (building != null && employeesAvailable > 0) {
			playerBusiness.myInventory.SetEmployeesAt(currentSite, employeesAvailable - 1);
			building.employees++;
		}
	}

	public void RecallEmployee() {
		Building building = Lot.FindLot(selectedObject, currentSite).Building;

		if (building != null && building.employees > 0) {
			building.employees--;
			int employeesAvailable = playerBusiness.myInventory.GetEmployeesAt(currentSite);
			playerBusiness.myInventory.SetEmployeesAt(currentSite, employeesAvailable + 1);
		}
	}

	private GameObject calculateSelectedObject() {
		Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
		RaycastHit hit;
		Physics.Raycast (ray, out hit);
		if (hit.collider == null) {
			return selectedObject;
		} else {
			selection.SetActive(true);
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

	private void setSiteLayers(int layer) {
		foreach (Site s in world.sites) {
			s.SitePlane.layer = layer;
		}
	}

	private void worldview() {
		timer = 0;

		desiredCameraPosition = WORLDVIEW;
		sidebar.SetActive(false);

		setSiteLayers(DEFAULT);
	}

	private void toHomesite() {
		toSite (homesite);
	}

	private void toSite(Site s) {
		timer = 0;
		setCurrentSite(s);

		float x = s.SitePlane.transform.position.x;
		float z = s.SitePlane.transform.position.z;

		sidebar.SetActive (true);
		desiredCameraPosition = new Vector3(x + 5.0f, SITE_CAMERA_HEIGHT, z + -5.0f);

		setSiteLayers(IGNORE_RAYCAST);
	}

	public void setCurrentSite(Site s) {
		currentSite = s;
		headsUpDisplay.currentSite = s;
	}

	private void deselectObject() {
		selectedObject = null;
		selection.SetActive(false);
	}

	// Update is called once per frame
	void Update () {
		platformSpecific ();

		if (!this.GetComponent<World>().enabled) {
			world = this.GetComponent<World>();
			world.player = playerBusiness;
			this.GetComponent<World>().enabled = true;
		} else if (world.isReady) {
			Debug.Log("World is ready!");
			world.isReady = false;
			this.GetComponent<EventManager>().enabled = true;
			em = this.GetComponent<EventManager>();
			headsUpDisplay = this.GetComponent<HeadsUpDisplay>();
			setCurrentSite(currentSite);
			this.GetComponent<HeadsUpDisplay>().enabled = true;
		}

		if (Input.GetMouseButtonDown (0)) {
			selectedObject = calculateSelectedObject ();
			if (selectedObject == null) {
				// Do nothing!
			} else {
				foreach (Site s in world.sites) {
					if (s.SitePlane == selectedObject) {
						toSite(s);
						deselectObject();
						break;
					}
				}
			}
		}
		if (Input.GetKeyDown(KeyCode.F2)) {
			worldview();
		}
		if (Input.GetKeyDown(KeyCode.F1)) {
			toHomesite();

		}
		if (Camera.main.transform.position != desiredCameraPosition) {
			moveCamera();
			timer++;
		}
	}
}
