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

	// how long to wait between spawns
	public float spawnDelayTime;

	// keep track of enemies on the screen
	private int currentEnemiesOnScreen = 0;
	
	// Create singleton
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
		StartCoroutine( ISpawnEnemy() );
	}



	IEnumerator ISpawnEnemy ()
	{
		// check to make sure we should spawn enemies
		if (enemiesPerSpawn > 0 && currentEnemiesOnScreen < totalEnemies) {

			// spawn enemies
			for (int i = 0; i < enemiesPerSpawn; i++) {

				// honor the max number of enemies at any one time
				if (currentEnemiesOnScreen < maxEnemiesOnScreen) {

					GameObject newEnemy = Instantiate(enemies[1]) as GameObject;

					newEnemy.transform.position = spawnPoint.transform.position;

					currentEnemiesOnScreen++;

				}
				

			}

		}

		yield return new WaitForSeconds(spawnDelayTime);
		StartCoroutine(ISpawnEnemy());


	}





	// Update the number of enemies on the screen to allow more to spawn.
	public void RemoveEnemyFromScreen ()
	{
		if (currentEnemiesOnScreen > 0) {

			currentEnemiesOnScreen--;

		}

	}

}
