using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameDirector : MonoBehaviour {

	public const int DEFAULT = 0;

	public const int IGNORE_RAYCAST = 2;

	private const float SITE_CAMERA_HEIGHT = 25.0f;

	public int lotsPerSite;

	public GameObject selectedObject;

	public GameObject player;

	public List<GameObject> enemyAI;
	
	private Site homesite;

	private Site currentSite;

	public World world;

	public PlayerBusiness playerBusiness;

	public HeadsUpDisplay headsUpDisplay;

	public GameObject sidebar;

	private GameObject selection;

	private Vector3 WORLDVIEW = new Vector3(0, 850, 0);

	private Vector3 HOMESITE = new Vector3(5, 25, 0);

	private Vector3 desiredCameraPosition;

	private int timer;

	// Use this for initialization
	void Start () {
		homesite = new Site(lotsPerSite, playerBusiness);

		headsUpDisplay.currentSite = homesite;
		headsUpDisplay.business = playerBusiness;

		desiredCameraPosition = HOMESITE;

		selection = (GameObject) GameObject.CreatePrimitive(PrimitiveType.Plane);
		selection.layer = IGNORE_RAYCAST;
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
		Lot.InstallBuilding (selectedObject, homesite, b);
	}

	public void InstallClayPit () {
		InstallBuilding(new ClayPit());
	}

	public void InstallPotteryWorkshop () {
		InstallBuilding(new PotteryWorkshop());
	}

	private void randomAssign() {
		foreach(Lot l in homesite.Lots) {
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

		float x = s.SitePlane.transform.position.x;
		float z = s.SitePlane.transform.position.z;

		desiredCameraPosition = new Vector3(x + 5.0f, SITE_CAMERA_HEIGHT, z);

		setSiteLayers(IGNORE_RAYCAST);
	}

	// Update is called once per frame
	void Update () {
		platformSpecific ();

		if (Input.GetMouseButtonDown (0)) {
			selectedObject = calculateSelectedObject ();
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
		if (headsUpDisplay.currentSite != currentSite) {
			headsUpDisplay.currentSite = currentSite;
		}
	}
}
