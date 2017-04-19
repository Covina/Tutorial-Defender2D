using UnityEngine;

public class Loader : MonoBehaviour {

	public GameObject gameManager;


	void Awake ()
	{
		// check if the game manager is available
		if (GameManager.instance == null) {
			Instantiate(gameManager);
		}


	}


}
