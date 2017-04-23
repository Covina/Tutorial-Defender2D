using UnityEngine;

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


}
