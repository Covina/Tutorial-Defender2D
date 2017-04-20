﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TowerManager : Singleton<TowerManager> {

	// get a button type
	private TowerButton towerButtonPressed;


	// Use this for initialization
	void Start () {



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

				PlaceTower (hit);
			}

		}

	}

	// find out which tower was selected
	public void SelectTower (TowerButton towerSelected)
	{

		towerButtonPressed = towerSelected;

	}

	public void PlaceTower (RaycastHit2D hit)
	{


		// check for UI and make sure a tower is selected
		if (!EventSystem.current.IsPointerOverGameObject() && towerButtonPressed != null) {


			// instantiate the tower
			GameObject newTower = Instantiate (towerButtonPressed.TowerObject);

			// set new tower position to the hit position
			newTower.transform.position = hit.transform.position;

		}


	}

}
