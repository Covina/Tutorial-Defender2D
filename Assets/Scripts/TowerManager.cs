using System.Collections;
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
			Vector2 worldPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);

			// fire the ray at the mouse click (world point), going in no direction
			RaycastHit2D hit = Physics2D.Raycast(worldPoint, Vector2.zero);

			//Debug.Log("Update() :: Mouse Clicked at " + worldPoint);

			PlaceTower(hit);

		}

	}

	// find out which tower was selected
	public void SelectTower (TowerButton towerSelected)
	{

		//Debug.Log("SelectTower() called;  Parameter passed: " + towerSelected);
		towerButtonPressed = towerSelected;

		//Debug.Log("SelectTower() ::  towerButtonPressed.gameObject.name: " + towerButtonPressed.gameObject.name);

		//Debug.Log("SelectTower() ::  towerButtonPressed.TowerObject : " + towerButtonPressed.TowerObject);

	}

	public void PlaceTower (RaycastHit2D hit)
	{

		//Debug.Log ("PlaceTower() called");

//		if (EventSystem.current.IsPointerOverGameObject ()) {
//			Debug.Log ("PlaceTower() - EventSystem.current.IsPointerOverGameObject is TRUE :: " + EventSystem.current.IsPointerOverGameObject() );
//		}
//
//		if (towerButtonPressed == null) {
//			Debug.Log ("PlaceTower() - towerButtonPressed IS NULL :: " + towerButtonPressed); 
//		}

		// check for UI and make sure a tower is selected
		if (!EventSystem.current.IsPointerOverGameObject() && towerButtonPressed != null) {


//			Debug.Log("PlaceTower() - towerButtonPressed :: " + towerButtonPressed);
//			Debug.Log("PlaceTower() - towerButtonPressed.gameObject.name :: " + towerButtonPressed.gameObject.name);
//			Debug.Log("PlaceTower() - towerButtonPressed.TowerObject :: " + towerButtonPressed.TowerObject);
//			Debug.Log("PlaceTower() - towerButtonPressed.TowerObject.gameObject.name :: " + towerButtonPressed.TowerObject.gameObject.name);


			// instantiate the tower
			GameObject newTower = Instantiate (towerButtonPressed.TowerObject);

			// set new tower position to the hit position
			newTower.transform.position = hit.transform.position;

		}


	}

}
