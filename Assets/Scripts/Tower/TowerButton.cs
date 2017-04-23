using UnityEngine;
using UnityEngine.UI;


public class TowerButton : MonoBehaviour {

	[SerializeField] private Tower towerObject;

	[SerializeField] private Sprite dragSprite;

	public Tower TowerObject {

		get	
			{ return towerObject; }

	}

	// Getter for DragSprite
	public Sprite DragSprite {

		get	
			{ return dragSprite; }

	}


	// find child cost text and set it to tower object cost
	void Start ()
	{

		Text towerCostTextValue = GetComponentInChildren<Text>();
		towerCostTextValue.text = TowerObject.TowerCost.ToString();


	}


}
