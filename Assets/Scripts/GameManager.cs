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
	[SerializeField] private Enemy[] enemies;

	// enemy count in the wave
	[SerializeField] private int totalEnemies = 3;

	// how many get spawned at a single time
//	[SerializeField] private int enemiesPerSpawn;

	// how long to wait between spawns
	[SerializeField] private float spawnDelayTime;

	// Which wave are we on?
	[SerializeField] private int currentWave = 0;

	// Total waves available in the game
	[SerializeField] private int totalWaves;

	// Total Enemies Killed in this round
	private int roundEnemiesKilled = 0;

	// Total Enemies Killed throughout the game
	private int totalEnemiesKilled = 0;

	// How many enemies escaped in that one Wave?
	private int roundEscapedEnemies = 0;

	// How many enemies have escaped in total?
	private int totalEscapedEnemiesCount = 0;

	// How many total escaped enemies are we allowing before player loses?
	private int totalEscapedEnemiesLimit = 10;


	// which of our enemies to spawn
	private int spawnEnemyID = 0;


	// Starting cash for the player
	[SerializeField] private int startingBalance = 10;

	// Store their current balance
	private int currencyBalance;

	// ===============  UI =================

	// their displayed currency balance value text component
	[SerializeField] private Text currencyBalanceTextValue;

	// the displayed current Wave value text component
	[SerializeField] private Text currentWaveTextValue;

	// The displayed current escaped enemies this round text component
	[SerializeField] private Text escapedEnemiesTextValue;


	[SerializeField] private Text actionButtonText;
	[SerializeField] private GameObject actionButtonObject;



	// to store the game state; init at play
	private GameStatus currentState = GameStatus.PLAY;

	// get the Audio source
	private AudioSource audioSource;



	// Getter/Setter for Current State enum
	public GameStatus CurrentState {
		get {
			return currentState;
		}
		set {
			currentState = value;
		}
	}

	// Getter/Setter for currency balance
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
	public int RoundEnemiesKilled {
		get {
			return roundEnemiesKilled;
		}
		set {
			roundEnemiesKilled = value;
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

	// Getter for totalEnemies
	public int TotalEnemies {
		get {
			return totalEnemies;
		}
		set {
			totalEnemies = value;
		}

	}


	public AudioSource MyAudioSource {

		get {
			return audioSource;
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

		// get access to the game manager audio source component
		audioSource = GetComponent<AudioSource>();


		// default button to "Start Game" at the beginning
		ShowMenu();

	}

	// Update function
	public void Update ()
	{
		// Drop the dragged tower
		HandleEscapeKey();
	}


	// make button start the game and spawn enemies
	public void ActionButtonPressed() 
	{

		switch (CurrentState) {

		case GameStatus.LOSE:
			RestartGame();
			break;

		case GameStatus.NEXT:
			// increment wave number
			currentWave++;

			// reset the round escaped counter
			RoundEscapedEnemies = 0;

			// Reset the kill counter
			RoundEnemiesKilled = 0;

			// disable the Action button
			actionButtonObject.SetActive(false);

			// Set game state to playing
			CurrentState = GameStatus.PLAY;

			// start spawning enemies
			StartCoroutine( ISpawnEnemy() );

			// Update the UI
			UpdateUI();
			break;

		case GameStatus.PLAY:
			RestartGame();
			break;

		case GameStatus.WIN:
			RestartGame();
			break;

		default:
			Debug.Log ("No currentState found for switch()");
			break;
		}




	}

	// Spawn the enemies
	IEnumerator ISpawnEnemy ()
	{
		// check to make sure we should spawn enemies
		if (TotalEnemies > 0 && EnemyList.Count < TotalEnemies) {

			// spawn enemies
			for (int i = 0; i < TotalEnemies; i++) {

				// Set the scaling enemies
				int enemyIndex = 0;
				if(currentWave == 1) enemyIndex = 0;					// all skulls
				if(currentWave == 2) enemyIndex = Random.Range(0,2);	// mix of skull and horns
				if(currentWave >= 3) enemyIndex = Random.Range(0,3);	// mix of skull, horns, arrows

				// create the new enemy
				Enemy newEnemy = Instantiate(enemies[ enemyIndex ]) as Enemy;

				// place it at the starting spot
				newEnemy.transform.position = spawnPoint.transform.position;

				// delay between spawns to space them out.
				yield return new WaitForSeconds(spawnDelayTime);
			}

			// recursive loop until we're full
			StartCoroutine(ISpawnEnemy());

		}

	}


	// Update all the important UI components
	public void UpdateUI ()
	{
		// Update Currency balanec
		currencyBalanceTextValue.text = currencyBalance.ToString();

		// Update Wave Number
		currentWaveTextValue.text = currentWave.ToString();

		// Update Ecaped Enemies within this Round
		escapedEnemiesTextValue.text = RoundEscapedEnemies + "/" + TotalEnemies;

	}


	// Add the enemy to the List
	public void RegisterEnemy (Enemy enemy, string sourceFunction)
	{
		Debug.Log("RegisterEnemy() Called from " + sourceFunction);

		// add the enemy to the list
		EnemyList.Add(enemy);

	}

	// Remove the enemy to the List
	public void UnregisterEnemy (Enemy enemy)
	{
		Debug.Log("UnregisterEnemy() Removing [" + enemy + "] :: Current EnemyList Count: [" + EnemyList.Count + "]");

		// Remove the enemy to the list
		EnemyList.Remove(enemy);
		Destroy(enemy.gameObject);

		Debug.Log("Destroy GameObject of [" + enemy + "] :: Current EnemyList Count: [" + EnemyList.Count + "]");
	}


	// delete all enemies (including killed ones)
	public void DestroyAllEnemies ()
	{

		foreach (Enemy enemy in GameObject.FindObjectsOfType<Enemy>()) {

			Destroy(enemy.gameObject);

		}

		// make it an empty list
		EnemyList.Clear();
	}


	// Handle an escaped enemy
	public void EnemyEscaped (Enemy enemy)
	{
		// Add to this rounds count of escaped enemies
		roundEscapedEnemies++;

		// Add to this escaped enemy to grand total
		totalEscapedEnemiesCount++;

		UnregisterEnemy (enemy);

		// Update the UI to reflect newly escaped enemy.
		UpdateUI();

		// check it Wave is over
		IsWaveOver ();

	}


	// Control what Action button to display
	public void ShowMenu ()
	{

		switch (CurrentState) {

		case GameStatus.LOSE:
			actionButtonText.text = "Try Again";
			GameManager.Instance.MyAudioSource.PlayOneShot (AudioManager.Instance.SFXGameOver);
			break;

		case GameStatus.NEXT:
			actionButtonText.text = "Next Wave";
			break;

		case GameStatus.PLAY:
			actionButtonText.text = "Start Game";
			break;

		case GameStatus.WIN:
			actionButtonText.text = "Play Again";
			GameManager.Instance.MyAudioSource.PlayOneShot (AudioManager.Instance.SFXGameOver);
			break;

		default:
			Debug.Log ("No currentState found for switch()");
			break;
		}


		// turn it on.
		actionButtonObject.SetActive(true);

	}


	// check to see if current wave is over
	public void IsWaveOver ()
	{

		Debug.Log("IsWaveOver() :: RoundEnemiesKilled [" + RoundEnemiesKilled + "] + RoundEscapedEnemies [" + RoundEscapedEnemies + "] == TotalEnemies [" + TotalEnemies + "]");


		// total up enemy states
		if ((RoundEnemiesKilled + RoundEscapedEnemies) == TotalEnemies) {

			//Debug.Log("IsWaveOver() = Yes");

			// increase total enemies to spawn
			TotalEnemies += 1;

			// clean up dead bodies
			DestroyAllEnemies();

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
			//Debug.Log ("SetCurrentGamestate () :: escaped exceeded limit [" + totalEscapedEnemiesCount + " > [" + TotalEscapedEnemiesLimit + "]");

			// set to lose
			CurrentState = GameStatus.LOSE;

		} else if (currentWave > totalWaves) {

			// set to win
			CurrentState = GameStatus.WIN;

		} else if (currentWave == 0 && (TotalEnemiesKilled + RoundEscapedEnemies) == 0) {

			// set to win
			CurrentState = GameStatus.PLAY;

		} else {

			CurrentState = GameStatus.NEXT;
		}

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


	// Allow Escape key to drop the selected tower.
	private void HandleEscapeKey ()
	{

		if (Input.GetKeyDown (KeyCode.Escape)) {

			// drop the selected tower on cursor
			TowerManager.Instance.disableDragSprite();
			TowerManager.Instance.towerButtonPressed = null;

		}

	}



	private void RestartGame() {

		Debug.Log("RestartGame() called");

		// reset the starting enemy count to 3
		TotalEnemies = 3;

		// set back to first wave
		currentWave = 1;

		// zero the round enemies killed
		RoundEnemiesKilled = 0;

		// zero the total enemies killed
		TotalEnemiesKilled = 0;

		// zero the round enemies escaped
		RoundEscapedEnemies = 0;

		// zero escaped enemies
		totalEscapedEnemiesCount = 0;

		// Set the money back to defult
		CurrencyBalance = startingBalance;

		// destroy all tower objects
		TowerManager.Instance.DestroyAllTowers();

		// rename all build sites so they are buildable again
		TowerManager.Instance.RenameTagsBuildSites();

		// Get rid of lingering tower selections
		TowerManager.Instance.towerButtonPressed = null;

		// disable the Action button
		actionButtonObject.SetActive(false);

		// Set game state to playing
		CurrentState = GameStatus.PLAY;

		// Update the UI
		UpdateUI();

		// start spawning enemies
		StartCoroutine( ISpawnEnemy() );

		// play new game starting sound
		audioSource.PlayOneShot(AudioManager.Instance.SFXNewGame);

	}


}
