using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {


	public static GameManager instance = null;

	// hold our spawn point
	public GameObject spawnPoint;

	// hold our skeletons
	public GameObject[] enemies;

	// enemy counter for increasing waves
	public int maxEnemiesOnScreen;

	// enemy count in the wave
	public int totalEnemies;

	// how many get spawned at a single time
	public int enemiesPerSpawn;


	// keep track of enemies on the screen
	private int currentEnemiesOnScreen = 0;
	

	void Awake ()
	{
		if (instance == null) {
			instance = this;
		} else if (instance != this){
			Destroy(gameObject);
		}

		DontDestroyOnLoad(gameObject);

	}


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}


	void spawnEnemy ()
	{

	}
}
