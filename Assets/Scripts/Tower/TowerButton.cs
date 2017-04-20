using UnityEngine;

public class TowerButton : MonoBehaviour {

	[SerializeField] private GameObject towerObject;

	[SerializeField] private Sprite dragSprite;

	public GameObject TowerObject {

		get	
			{ return towerObject; }

	}

	// Getter for DragSprite
	public Sprite DragSprite {

		get	
			{ return dragSprite; }

	}


}
