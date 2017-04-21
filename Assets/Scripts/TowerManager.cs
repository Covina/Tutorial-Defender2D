using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TowerManager : Singleton<TowerManager> {

	// get a button type
	private TowerButton towerButtonPressed;

	private SpriteRenderer spriteRenderer;


	// Use this for initialization
	void Start () {

		spriteRenderer = GetComponent<SpriteRenderer>();

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

				// change tag to avoid repeat placement
				hit.collider.tag = "BuildSiteFull";

				PlaceTower (hit);
			}

		}

		if (spriteRenderer.enabled) {

			FollowMouse();

		}

	}

	// find out which tower was selected
	public void SelectTower (TowerButton towerSelected)
	{

		towerButtonPressed = towerSelected;

		// check to see if player can afford it
		if (GameManager.Instance.CurrencyBalance >= towerButtonPressed.TowerObject.GetComponent<Tower> ().TowerCost) {

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
				GameObject newTower = Instantiate (towerButtonPressed.TowerObject);

				// set new tower position to the hit position
				newTower.transform.position = hit.transform.position;

				// buy the tower
				PurchaseTower(newTower.GetComponent<Tower>().TowerCost);


				// move the tower from the mouse cursor
				disableDragSprite ();

		}


	}

	// charge the currency for the tower
	public void PurchaseTower (int cost)
	{
		// decrement the cost
		GameManager.Instance.CurrencyBalance -= cost;

		// Update the UI
		GameManager.Instance.UpdateUI();
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
