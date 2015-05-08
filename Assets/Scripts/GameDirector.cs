using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/**
 * Directs the behavior of other game elements
 * Initializes all elements when the game starts in an intelligent order
 */
public class GameDirector : MonoBehaviour {

	/** The amount of time the game has been paused */
	public static float timeCorrection { get; private set; }

	/** 
	 * Switch this to true to pause the game or false to unpause
	 * 
	 * All MonoBehaviors that use the Update method should handle
	 * what to do if GameDirector.PAUSED is true
	 */
	public static bool PAUSED { get; set; }

	/** Always points to the main GameDirector object */
	public static GameDirector THIS { get; private set; }

	/** Always points to the main GameTime object */
	public static GameTime gameTime;

	/** The value of the "Default" layer */
	public const int DEFAULT = 0;

	/** The value of the "Ignore Raycast" layer */
	public const int IGNORE_RAYCAST = 2;

	/** The event manager for handling random events */	
	public EventManager em { get; private set; }

	/** The currently selected object */
	public GameObject selectedObject { get; private set; }

	/** The pause screen */
	public GameObject pauseScreen; // Set in the Unity Editor

	/** The whole World on the World Plane */
	public World world; // Set in the Unity Editor

	/** The home site at 0,0 */
	public Site homesite { get; private set; }

	/** The current site */
	public Site currentSite { get; private set; }

	/** The Stager for moving through the stages */
	public Stager stager { get; private set; }

	/** The Sales item for handling buying and selling */
	public Sales sales { get; private set; }

	/** The player */
	public PlayerBusiness playerBusiness { get; private set; }

	/** The heads up display */
	public HeadsUpDisplay headsUpDisplay;

	/** The sidebar */
	public GameObject sidebar;

	/** The selection plane */
	private GameObject selection;

	// // CAMERA VALUES // //

	/** Camera position for the world view */
	private Vector3 WORLDVIEW = new Vector3(0, 850.0f, -200.0f);

	/** How far the camera should be from sites in site view in the Y coordinate */
	private const float SITE_CAMERA_HEIGHT = 25.0f;

	/** Camera position at the home site */
	private Vector3 HOMESITE = new Vector3(5, 25, -5);

	/** The desired camera position */
	private Vector3 desiredCameraPosition;

	/** Timer for moving the camera */
	private int timer;

	/** Currently appropriate quarry to make, or null if there is none */
	public Quarry appropriateQuarry { get; private set; }

	/**
	 * Initializes the game
	 */
	void Start() {
		THIS = this;
		gameTime = this.gameObject.AddComponent<GameTime>();
		playerBusiness = new PlayerBusiness();
		//playerBusiness.myInventory.SetEmployeesAt(homesite, 8);

		stager = new Stager();

		desiredCameraPosition = HOMESITE;

		sales = new Sales(this);
		timeCorrection = 0.0f;

		selection = (GameObject)GameObject.CreatePrimitive(PrimitiveType.Plane);
		selection.layer = IGNORE_RAYCAST;
		selection.SetActive(false);
		List<Business> temp = new List<Business>();
		temp.Add(playerBusiness);
	}

	/**
	 * Installs a building on the selected lot
	 */
	public void InstallBuilding(Building b) {
		Lot lot = Lot.FindLot(selectedObject, currentSite);

		// Only install the building if the player can afford it
		if (sales.buyBuilding(playerBusiness, lot, b)) {
			//Lot.InstallBuilding(selectedObject, currentSite, b);
		}
	}

	/** 
	 * Installs a quarry based on the type of resource the lot has
	 * 
	 * If the lot has no resource, no quarry is installed
	 */
	public void InstallQuarry() {
		if (appropriateQuarry != null) {
			InstallBuilding(appropriateQuarry);
			appropriateQuarry = null;
		}
	}

	/**
	 * Installs a pottery workshop on the selected lot
	 */
	public void InstallPotteryWorkshop() {
		InstallBuilding(new PotteryWorkshop());
	}

	/**
	 * Leases the selected lot
	 */
	public void LeaseSelectedLot() {
		Lot lot = Lot.FindLot(selectedObject, currentSite);
		if (!sales.leaseLot(playerBusiness, lot)) {
			// Do something if the lot wasn't leased
		}
	}

	/**
	 * Labor distribution is now being determined by the offered wage in the Site class
	 * This method may not be needed...
	 * 
	 * Deplays an employee
	 */
	public void DeployEmployee() {
		Building building = Lot.FindLot(selectedObject, currentSite).Building;
		int employeesAvailable = playerBusiness.myInventory.GetEmployeesAt(currentSite);

		if (building != null && employeesAvailable > 0) {
			//playerBusiness.myInventory.SetEmployeesAt(currentSite, employeesAvailable - 1);
			//building.employees++;
		}
	}

	/**
	 * Labor distribution is now being determined by the offered wage in the Site class
	 * This method may not be needed...
	 *
	 * Recalls an employee
	 */
	public void RecallEmployee() {
		Building building = Lot.FindLot(selectedObject, currentSite).Building;

		if (building != null && building.employees > 0) {
			//building.employees--;
			//int employeesAvailable = playerBusiness.myInventory.GetEmployeesAt(currentSite);
			//playerBusiness.myInventory.SetEmployeesAt(currentSite, employeesAvailable + 1);
		}
	}

	/** Counter for getting AI Business colors */
	private static List<Color> usedColors = new List<Color>();

	/**
	 * Gets a color for AI businesses
	 * 
	 * Cycles through the 8 possible color colors
	 * 
	 * @return the color
	 */
	public static Color getBusinessColor() {
		int tries = 0;
		Color color = new Color(Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f), 
			Random.Range(0.0f, 1.0f));
		// If this color was already used, and we haven't tried 1000 times, keep getting a new one
		while (colorUsed(color) && tries < 1000) {
			color = new Color(Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f), 
				Random.Range(0.0f, 1.0f));
		}
		return color;
	}

	/**
	 * Lets the director know that a business has used a color
	 */
	public static void setBusinessColor(Color color) {
		if (!usedColors.Contains(color)) {
			usedColors.Add(color);
		}
	}

	/**
	 * Checks if this, or a similar color, has been used as a business color
	 * 
	 * @param color the color to check against
	 * @return whether the color, or a similar one, was used already
	 */
	public static bool colorUsed(Color color) {
		foreach (Color used in usedColors) {
			if (Mathf.Abs(color.r - used.r) < .1f && Mathf.Abs(color.g - used.g) < .1f && Mathf.Abs(color.b - used.b) < .1f) {
				return true;
			}
		}
		return false;
	}

	/**
	 * Figures out if the player clicked on an object in the Default layer
	 * 
	 * If they did, selects it with the "selection" plane
	 */
	private GameObject calculateSelectedObject() {
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		RaycastHit hit;
		Physics.Raycast(ray, out hit);
		if (hit.collider == null) {
			return selectedObject;
		} else {
			return hit.collider.gameObject;
		}
	}

	/**
	 * Moves the camera toward the desiredCameraPosition slowly
	 */
	private void moveCamera() {
		Vector3 distance = (desiredCameraPosition - Camera.main.transform.position) / 3;
		Camera.main.transform.Translate(distance * Time.deltaTime * 20, Space.World);
		// If we've done this for more than 80 frames, or 45 frames when the camera has to go up high, 
		// or we're really close, just put the camera at the desired position
		if (timer > 80 || (timer > 45 && desiredCameraPosition.y > 400) || 
			(Mathf.Abs(distance.x) < 0.0001f && Mathf.Abs(distance.y) < 0.0001f && Mathf.Abs(distance.z) < 0.0001f)) {
			Camera.main.transform.position = desiredCameraPosition;
		}
	}

	/**
	 * Sets all of the SitePlanes and their children to the given layer
	 */
	private void setSiteLayers(int layer) {
		foreach (Site s in world.sites) {
			s.SitePlane.layer = layer;
			foreach (Transform t in s.SitePlane.transform) {
				t.gameObject.layer = layer;
			}
		}
	}

	/**
	 * Go to the worldview
	 */
	private void worldview() {
		timer = 0;

		deselectObject();

		desiredCameraPosition = WORLDVIEW;
		sidebar.SetActive(false);

		// Make the sites selectable
		setSiteLayers(DEFAULT);
	}

	/**
	 * Shortcut to calling "toSite(homesite)"
	 */
	private void toHomesite() {
		toSite(homesite);
	}

	/**
	 * To to a specific site
	 */
	private void toSite(Site s) {
		if (s == null) {
			Debug.LogError("Null Site");
			return;
		}

		deselectObject();

		timer = 0;
		setCurrentSite(s);

		float x = s.SitePlane.transform.position.x;
		float z = s.SitePlane.transform.position.z;

		sidebar.SetActive(true);
		desiredCameraPosition = new Vector3(x + 5.0f, SITE_CAMERA_HEIGHT, z + -5.0f);

		// Make the site objects non-selectable
		setSiteLayers(IGNORE_RAYCAST);
	}

	/**
	 * Sets the current site value here and also in HeadsUpDisplay
	 * 
	 * DO NOT USE THIS TO MOVE THE CAMERA TO A SITE, USE "toSite(Site s)" INSTEAD
	 */
	public void setCurrentSite(Site s) {
		currentSite = s;
		if (headsUpDisplay != null) {
			headsUpDisplay.currentSite = s;
		}
	}

	/**
	 * Deselects the currently selected object
	 */
	private void deselectObject() {
		selectedObject = null;
		selection.SetActive(false);
	}

	/**
	 * Handles platform specific behaviors
	 */
	private void platformSpecific() {
		switch (Application.platform) {
			case RuntimePlatform.OSXPlayer:
			case RuntimePlatform.WindowsPlayer:
			case RuntimePlatform.LinuxPlayer:
			// Esc quits the game
				if (Input.GetKeyDown(KeyCode.Escape)) {
					Application.Quit();
				}
				break;
		}
	}

	/**
	 * The GameDirector's update behaviors
	 * 
	 * All behaviors should involve primary game elements 
	 * i.e. initializing primary scripts, pausing, mouse/keyboard input, camera position
	 * 
	 * If a behavior here is specific to one game element, consider moving it to a more appropriate update
	 */
	void Update() {
		// Handle platform spectific behaviors
		// Press ESC to quit is run here, even before pausing is checked
		platformSpecific();


		// // PAUSE BEHAVIORS // //


		// Pause/unpause the game when 'P' is pressed
		if (Input.GetKeyDown(KeyCode.P)) {
			PAUSED = !PAUSED;
		}

		// When the game is paused
		if (PAUSED) {
			// If the pause screen isn't active, turn it on
			if (!pauseScreen.activeSelf) {
				pauseScreen.SetActive(true);
			}

			// Increment the time correction, but don't do other things
			timeCorrection += Time.deltaTime;
			return;

			// If the game is not paused, but the pause screen is active, turn it off
		} else if (pauseScreen.activeSelf) {
			pauseScreen.SetActive(false);
		}


		// // INITIALIZATION OF GAME ELEMENTS IF THEY ARE NOT ENABLED // //


		// If the world component hasn't been enabled, enable it
		if (!this.GetComponent<World>().enabled) {
			world = this.GetComponent<World>();
			world.player = playerBusiness;
			this.GetComponent<World>().enabled = true;

			// When the world has announced it is ready, do these things
		} else if (world.isReady) {
			world.isReady = false; // Make sure this if only runs once
			homesite = world.homesite;

			this.GetComponent<EventManager>().enabled = true;
			em = this.GetComponent<EventManager>();
			headsUpDisplay = this.GetComponent<HeadsUpDisplay>();
			setCurrentSite(currentSite);
			this.GetComponent<HeadsUpDisplay>().enabled = true;
			this.GetComponent<StatusUI>().enabled = true;
		}


		// // NON-PAUSE, NON-QUIT KEYBOARD AND MOUSE BEHAVIORS // //


		// When the left mouse button is clicked
		if (Input.GetMouseButtonDown(0)) {
			selectedObject = calculateSelectedObject();
			if (selectedObject == null) {
				// Do nothing!
			} else {
				foreach (Site s in world.sites) {
					if (s.SitePlane == selectedObject) {
						toSite(s);
						goto breakout;
					}
					foreach (Lot l in s.Lots) {
						if (l.LotPlaneIs(selectedObject)) {
							if (desiredCameraPosition == WORLDVIEW) {
								toSite(s);
							} else {
								selection.SetActive(true);
								Renderer r = selection.GetComponent<Renderer>();
								r.material.color = l.Owner.businessColor;
								selection.transform.position = 
									new Vector3(selectedObject.transform.position.x, selectedObject.transform.position.y - 0.01f, selectedObject.transform.position.z);
								selection.transform.localScale = new Vector3(selectedObject.transform.localScale.x + 0.02f, 1, selectedObject.transform.localScale.z + 0.02f);

								// If the lot is an empty lot with resources, let the user make a quarry there
								if (l.Resource.HasValue && l.Building == null) {
									appropriateQuarry = Quarry.NewAppropriateQuarry(l.Resource.Value);
								} else {
									appropriateQuarry = null;
								}
							}
							goto breakout;
						}
					}
				}
				// This will display if an object on the default layer is able to be clicked but not handled
				Debug.LogWarning(selectedObject + " is selectable but not used");

				breakout :
				{}
			}
		}

		// When F2 is pressed, go to the world view
		if (Input.GetKeyDown(KeyCode.F2)) {
			deselectObject();
			worldview();
		}

		// When F1 is pressed, go to the home site
		if (Input.GetKeyDown(KeyCode.F1)) {
			deselectObject();
			toHomesite();
		}


		// // CAMERA MOVMENT // //


		// When the camera isn't in the desired position, move it - always do this last
		//
		// Because this is always done last, it is roughly equivalent to "LateUpdate"
		if (Camera.main.transform.position != desiredCameraPosition) {
			moveCamera();
			timer++;
		}
	}
}
