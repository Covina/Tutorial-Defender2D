using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TowerManager : Singleton<TowerManager> {

	// get a button type
	public TowerButton towerButtonPressed { get; set; }

	private SpriteRenderer spriteRenderer;

	// store all the towers that we built
	private List<Tower> TowerList = new List<Tower>();

	// Store the building sites that were used
	private List<Collider2D> BuildList = new List<Collider2D>();

	private Collider2D buildTile;


	// Use this for initialization
	void Start () {

		spriteRenderer = GetComponent<SpriteRenderer>();

		buildTile = GetComponent<Collider2D>();

	}
	
	// Update is called once per frame
	void Update ()
	{
		// Where did the user click the mouse?
		if (Input.GetMouseButtonDown (0)) {


			// store the x,y of the mouse click
			Vector2 worldPoint = Camera.main.ScreenToWorldPoint (Input.mousePosition);

			// fire the ray at the mouse click (world point), going in no direction
			RaycastHit2D hit = Physics2D.Raycast (worldPoint, Vector2.zero);

			// only place towers on build site locations
			if (hit.collider.tag == "BuildSite") {

				// store the collider
				buildTile = hit.collider;

				// change tag to avoid repeat placement
				buildTile.tag = "BuildSiteFull";

				// register the site
				RegisterBuildSite(buildTile);

				// place the tower down on the map
				PlaceTower (hit);
			}

		}

		if (spriteRenderer.enabled) {

			FollowMouse();

		}

	}

	// Register the build site that we used for a tower
	public void RegisterBuildSite (Collider2D buildTag)
	{
		// add build
		BuildList.Add(buildTag);
	}

	// Register the tower object we built
	public void RegisterTower (Tower tower)
	{
		// add build
		TowerList.Add(tower);
	}

	// Reset the build site tags
	public void RenameTagsBuildSites ()
	{
		// loop through and rename
		foreach (Collider2D collider in BuildList) {

			collider.tag = "BuildSite";

		}

		// clear the build lsit
		BuildList.Clear();
	}


	// Destroy all towers on game reset
	public void DestroyAllTowers ()
	{
		foreach (Tower tower in TowerList) {

			Destroy(tower.gameObject);

		}

	}


	// find out which tower was selected
	public void SelectTower (TowerButton towerSelected)
	{

		// check to see if player can afford it
		if (GameManager.Instance.CurrencyBalance >= towerSelected.TowerObject.TowerCost) {

			towerButtonPressed = towerSelected;

			// enable the sprite for following the mouse
			enableDragSprite (towerButtonPressed.DragSprite);
		}

	}


	// Placing towers
	public void PlaceTower (RaycastHit2D hit)
	{
		// TODO - Add check if player can afford the tower

		// check for UI and make sure a tower is selected
		if (!EventSystem.current.IsPointerOverGameObject () && towerButtonPressed != null) {

			// instantiate the tower
			Tower newTower = Instantiate (towerButtonPressed.TowerObject);

			// set new tower position to the hit position
			newTower.transform.position = hit.transform.position;

			// buy the tower
			PurchaseTower (newTower.TowerCost);


			// move the tower from the mouse cursor
			disableDragSprite ();
		}


	}

	// charge the currency for the tower
	public void PurchaseTower (int cost)
	{
		// decrement the cost
		GameManager.Instance.SubtractCurrency(cost);
	}


	public void FollowMouse ()
	{
		// move the TowerManager Transform object to where the mouse is (with attached sprite)
		transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		transform.position = new Vector2(transform.position.x, transform.position.y);

	}


	public void enableDragSprite (Sprite sprite)
	{
		spriteRenderer.enabled = true;
		spriteRenderer.sprite = sprite;
	}


	public void disableDragSprite ()
	{
		spriteRenderer.enabled = false;
	}



}
