using UnityEngine;

/**
 * Manages the leasing of lots and creation of businesses
 */
public class LotManager : MonoBehaviour {

	/** Points to the Game Director */
	private GameDirector gd;

	/** Points to the Game Director's sales object */
	private Sales sales;

	/** The player */
	private PlayerBusiness player;

	/** Currently appropriate quarry to make, or null if there is none */
	public Quarry appropriateQuarry { get; set; }

	/**
	 * Gets objects from the game director
	 */
	void Start() {
		gd = GameDirector.THIS;
		sales = gd.sales;
		player = gd.playerBusiness;
	}
	
	/**
	 * Installs a building on the given lot
	 * 
	 * @param building the building to install
	 * @param business the business purchasing the building
	 * @param lot the lot to install the building at
	 */
	public void InstallBuilding(Building building, Business business, Lot lot) {
		// Only install the building if the business can afford it
		if (sales.buyBuilding(business, lot, building)) {
			// If the sale was successful
		}
		gd.requestWorkshop = false;
	}
	
	/**
	 * Install a building for the player at the currently selected site
	 * 
	 * @param building the building to install
	 */
	private void InstallBuilding(Building building) {
		Lot lot = Lot.FindLot(gd.selectedObject, gd.currentSite);
		
		InstallBuilding(building, player, lot);
	}
	
	/** 
	 * Installs a quarry based on the type of resource the lot has
	 * 
	 * If the lot has no resource, no quarry is installed
	 */
	public void InstallQuarry() {
		Lot lot = Lot.FindLot(gd.selectedObject, gd.currentSite);
		if (appropriateQuarry != null) {
			InstallBuilding(Quarry.NewAppropriateQuarry(lot.Resource.Value), player, lot);
			appropriateQuarry = null;
		}
	}
	
	/**
	 * Sells the building at the selected lot
	 */
	public void SellBuildingAtSelectedLot() {
		Lot lot = Lot.FindLot(gd.selectedObject, gd.currentSite);
		
		sales.sellBuilding(player, lot.Building);
		
		if (lot.Resource.HasValue) {
			appropriateQuarry = Quarry.NewAppropriateQuarry(lot.Resource.Value);
		} else {
			appropriateQuarry = null;
		}
	}
	
	/**
	 * Leases the selected lot
	 */
	public void LeaseSelectedLot() {
		Lot lot = Lot.FindLot(gd.selectedObject, gd.currentSite);
		if (!sales.leaseLot(player, lot)) {
			// Do something if the lot wasn't leased
		} else {
			// Set the selection color to the player's color
			Renderer r = gd.selection.GetComponent<Renderer>();
			r.material.color = lot.Owner.businessColor;
		}
	}
	
	/**
	 * Sells the lease at a lot
	 */
	public void SellLeaseAtSelectedLot() {
		Lot lot = Lot.FindLot(gd.selectedObject, gd.currentSite);
		
		sales.sellLease(player, lot);
		
		// Set the selection color to the unowned color
		Renderer r = gd.selection.GetComponent<Renderer>();
		r.material.color = lot.Owner.businessColor;
	}

	
	
	
	// // WORKSHOP TYPES TO INSTALL - METHODS USED BY WORKSHOP BUTTONS // //
	
	/**
	 * Installs a pottery workshop on the selected lot
	 */
	public void InstallPotteryWorkshop() {
		InstallBuilding(new PotteryWorkshop());
	}
	
	/**
	 * Installs a weapon smith on the selected lot
	 */
	public void InstallWeaponSmith() {
		InstallBuilding(new WeaponSmith());
	}
	
	/**
	 * Installs a brick works on the selected lot
	 */
	public void InstallBrickworks() {
		InstallBuilding(new Brickworks());
	}
	
	/**
	 * Installs a furniture workshop on the selected lot
	 */
	public void InstallFurnitureWorkshop() {
		InstallBuilding(new FurnitureWorkshop());
	}
	
	/**
	 * Installs a computer chip factory on the selected lot
	 */
	public void InstallChipFactory() {
		InstallBuilding(new ChipFactory());
	}
	
	/**
	 * Installs a lamp maker on the selected lot
	 */
	public void InstallLampMaker() {
		InstallBuilding(new LampMaker());
	}
	
	/**
	 * Installs a steakhouse on the selected lot
	 */
	public void InstallSteakhouse() {
		InstallBuilding(new Steakhouse());
	}
	
	/**
	 * Installs a fish fry on the selected lot
	 */
	public void InstallFishFry() {
		InstallBuilding(new FishFry());
	}
	
	/**
	 * Installs a train track maker on the selected lot
	 */
	public void InstallTrackMaker() {
		InstallBuilding(new TrackMaker());
	}
}

