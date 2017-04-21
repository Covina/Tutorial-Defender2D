using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : Singleton<GameManager> {

	// hold our spawn point
	[SerializeField] private GameObject spawnPoint;

	// hold our skeletons
	[SerializeField] private GameObject[] enemies;

	// enemy counter for increasing waves
	[SerializeField] private int maxEnemiesOnScreen;

	// enemy count in the wave
	[SerializeField] private int totalEnemies;

	// how many get spawned at a single time
	[SerializeField] private int enemiesPerSpawn;

	// how long to wait between spawns
	[SerializeField] private float spawnDelayTime;

	// create list of Enemies
	public List<Enemy> EnemyList = new List<Enemy>();

	// ===============  UI =================

	// how long to wait between spawns

	[SerializeField] private int startingBalance = 10;
	[SerializeField] private int currencyBalance;
	[SerializeField] private Text currencyBalanceTextValue;

	[SerializeField] private int totalWaves;
	[SerializeField] private int currentWave = 0;
	[SerializeField] private Text currentWaveTextValue;
	[SerializeField] private int escapedEnemiesCount = 0;
	[SerializeField] private Text escapedEnemiesTextValue;
	[SerializeField] private int enemiesKilled = 0;

	// which of our enemies to spawn
	[SerializeField] private int spawnEnemyID = 0;


	[SerializeField] private GameObject nextWaveButton;



	// Getter for currency balance
	public int CurrencyBalance {
		get {
			return currencyBalance;
		}

		set {
			currencyBalance = value;
		}
	}


	// Use this for initialization
	void Start () {

		// set currency balance value
		currencyBalance = startingBalance;

		// set currency balance in UI
		currencyBalanceTextValue.text = currencyBalance.ToString();

		// set current wave
		currentWaveTextValue.text = currentWave.ToString();


	}

	// make button start the game and spawn enemies
	public void StartWave() 
	{
		// increment wave number
		currentWave++;

		// disable the Next Wave button
		nextWaveButton.SetActive(false);

		// start spawning enemies
		StartCoroutine( ISpawnEnemy() );

		// Update the UI
		UpdateUI();

	}

	public void UpdateUI ()
	{
		// Update Currency
		currencyBalanceTextValue.text = currencyBalance.ToString();

		// Update Wave
		currentWaveTextValue.text = currentWave.ToString();

		// Update Ecaped Enemies
		escapedEnemiesTextValue.text = escapedEnemiesCount + "/" + totalEnemies;

	}



	IEnumerator ISpawnEnemy ()
	{
		// check to make sure we should spawn enemies
		if (enemiesPerSpawn > 0 && EnemyList.Count < totalEnemies) {

			// spawn enemies
			for (int i = 0; i < enemiesPerSpawn; i++) {

				// honor the max number of enemies 
				if (EnemyList.Count < maxEnemiesOnScreen) {

					GameObject newEnemy = Instantiate(enemies[1]) as GameObject;

					newEnemy.transform.position = spawnPoint.transform.position;



				}
				

			}

			yield return new WaitForSeconds(spawnDelayTime);
			StartCoroutine(ISpawnEnemy());

		}




	}



	public void RegisterEnemy (Enemy enemy)
	{
		// add the enemy to the list
		EnemyList.Add(enemy);

	}


	public void UnregisterEnemy (Enemy enemy)
	{
		// Remove the enemy to the list
		EnemyList.Remove(enemy);
		Destroy(enemy.gameObject);
	}

	// delete all enemies (including killed ones)
	public void DestroyAllEnemies ()
	{

		// loop through and destroy gameobjects
		foreach (Enemy enemy in EnemyList) {

			Destroy(enemy.gameObject);

		}

		// make it an empty list
		EnemyList.Clear();
	}



}
