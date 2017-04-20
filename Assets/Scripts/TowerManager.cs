using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerManager : Singleton<TowerManager> {

	// get a button type
	private TowerButton towerButtonPressed;


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	// find out which tower was selected
	public void selectedTower (TowerButton towerSelected)
	{
		towerButtonPressed = towerSelected;
		Debug.Log("Tower Selected: " + towerButtonPressed.gameObject.name);

	}

}
