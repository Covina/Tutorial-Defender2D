using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public enum GameStatus {
	NEXT, PLAY, LOSE, WIN
}

public class GameManager : Singleton<GameManager> {

	// create list of Enemies
	public List<Enemy> EnemyList = new List<Enemy>();

	// hold our spawn point
	[SerializeField] private GameObject spawnPoint;

	// hold our skeletons
	[SerializeField] private GameObject[] enemies;

	// enemy count in the wave
	[SerializeField] private int totalEnemies = 3;

	// how many get spawned at a single time
	[SerializeField] private int enemiesPerSpawn;

	// how long to wait between spawns
	[SerializeField] private float spawnDelayTime;

	// Which wave are we on?
	[SerializeField] private int currentWave = 0;

	// Total waves available in the game
	[SerializeField] private int totalWaves;

	private int totalEnemiesKilled = 0;
	private int roundEscapedEnemies = 0;
	private int totalEscapedEnemiesCount = 0;
	private int totalEscapedEnemiesLimit = 10;


	// which of our enemies to spawn
	[SerializeField] private int spawnEnemyID = 0;

	[SerializeField] private int startingBalance = 10;
	[SerializeField] private int currencyBalance;

		// ===============  UI =================
	[SerializeField] private Text currencyBalanceTextValue;
	[SerializeField] private Text currentWaveTextValue;

	[SerializeField] private Text escapedEnemiesTextValue;
	[SerializeField] private Text nextWaveText;
	[SerializeField] private GameObject nextWaveButton;

	// to store the game state; init at play
	private GameStatus currentState = GameStatus.PLAY;



	public GameStatus CurrentState {
		get {
			return currentState;
		}
		set {
			currentState = value;
		}
	}

	// Getter for currency balance
	public int CurrencyBalance {
		get {
			return currencyBalance;
		}

		set {
			currencyBalance = value;
			UpdateUI ();
		}
	}

	// getter for totalEscapedEnemies
	public int TotalEscapedEnemiesLimit {
		get {
			return totalEscapedEnemiesLimit;
		}
		
	}

	// Getter for escaped enemies that round
	public int RoundEscapedEnemies {
		get {
			return roundEscapedEnemies;
		}
		set {
			roundEscapedEnemies = value;
		}
		
	}

	// Getter for totalEnemiesKilled
	public int TotalEnemiesKilled {
		get {
			return totalEnemiesKilled;
		}
		set {
			totalEnemiesKilled = value;
		}
		
	}


	// Use this for initialization
	void Start () {

		// default button to "Start Game" at the beginning
		ShowMenu();

		// set currency balance value
		currencyBalance = startingBalance;

		// set currency balance in UI
		currencyBalanceTextValue.text = currencyBalance.ToString();

		// set current wave
		currentWaveTextValue.text = currentWave.ToString();

	}

	public void Update ()
	{
		HandleEscapeKey();
	}




	// make button start the game and spawn enemies
	public void StartWave() 
	{
		// increment wave number
		currentWave++;

		// reset the round escaped counter
		RoundEscapedEnemies = 0;

		// disable the Next Wave button
		nextWaveButton.SetActive(false);
		CurrentState = GameStatus.PLAY;

		// start spawning enemies
		StartCoroutine( ISpawnEnemy() );

		// Update the UI
		UpdateUI();

	}


	IEnumerator ISpawnEnemy ()
	{
		// check to make sure we should spawn enemies
		if (enemiesPerSpawn > 0 && EnemyList.Count < totalEnemies) {

			// spawn enemies
			for (int i = 0; i < enemiesPerSpawn; i++) {

				// honor the max number of enemies 
				if (EnemyList.Count < totalEnemies) {

					GameObject newEnemy = Instantiate(enemies[ Random.Range(0,3) ]) as GameObject;

					newEnemy.transform.position = spawnPoint.transform.position;

				}

			}

			yield return new WaitForSeconds(spawnDelayTime);
			StartCoroutine(ISpawnEnemy());

		}

	}

	public void UpdateUI ()
	{
		// Update Currency
		currencyBalanceTextValue.text = currencyBalance.ToString();

		// Update Wave
		currentWaveTextValue.text = currentWave.ToString();

		// Update Ecaped Enemies
		escapedEnemiesTextValue.text = RoundEscapedEnemies + "/" + totalEnemies;

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


	// keep track of ewscaped enemies
	public void EnemyEscaped ()
	{
		// Log the escaped enemy
		roundEscapedEnemies++;
		totalEscapedEnemiesCount++;
		UpdateUI();
		IsWaveOver ();

	}


	// support earning money through killing enemies
	public void AddCurrency (int amount)
	{
		// update the amount
		CurrencyBalance += amount;

	}

	// support spending money through buying towers
	public void SubtractCurrency (int amount)
	{
		// update the amount
		CurrencyBalance -= amount;

	}


	public void ShowMenu ()
	{

		switch (CurrentState) {

		case GameStatus.LOSE:
			nextWaveText.text = "Play Again";
			break;

		case GameStatus.NEXT:
			nextWaveText.text = "Next Wave";
			break;

		case GameStatus.PLAY:
			nextWaveText.text = "Start Game";
			break;

		case GameStatus.WIN:
			nextWaveText.text = "Play";
			break;

		default:
			Debug.Log ("No currentState found for switch()");
			break;
		}


		// turn it on.
		nextWaveButton.SetActive(true);

	}



	private void HandleEscapeKey ()
	{

		if (Input.GetKeyDown (KeyCode.Escape)) {

			// drop the selected tower on cursor
			TowerManager.Instance.disableDragSprite();
			TowerManager.Instance.towerButtonPressed = null;


		}

	}

	// check to see if current wave is over
	public void IsWaveOver ()
	{
		// total up enemy states
		if ((TotalEnemiesKilled + RoundEscapedEnemies) == totalEnemies) {

			Debug.Log("IsWaveOver() = yes");

			// increase total enemies
			totalEnemies++;

			// clean up dead bodies
			RemoveDeadEnemies ();

			// update the game state
			SetCurrentGamestate();

			// show the UI next buttons
			ShowMenu();

		}

	}


	// Set the enum game state
	public void SetCurrentGamestate ()
	{

		if (totalEscapedEnemiesCount >= TotalEscapedEnemiesLimit) {
			Debug.Log ("SetCurrentGamestate () :: escaped exceeded limit [" + totalEscapedEnemiesCount + " > [" + TotalEscapedEnemiesLimit + "]");

			// set to lose
			CurrentState = GameStatus.LOSE;

		} else if (currentWave >= totalWaves) {

			// set to win
			CurrentState = GameStatus.WIN;

		} else if (currentWave == 0 && (TotalEnemiesKilled + RoundEscapedEnemies) == 0) {

			// set to win
			CurrentState = GameStatus.PLAY;

		} else {

			CurrentState = GameStatus.NEXT;
		}

	}



	public void RemoveDeadEnemies ()
	{

		Debug.Log ("Count in EnemyList [" + EnemyList.Count + "]");

		if (EnemyList.Count > 0) {
		
			for (int i = 0; i< EnemyList.Count; i++) {

				UnregisterEnemy (EnemyList[i]);

			}

		}
	}

}
