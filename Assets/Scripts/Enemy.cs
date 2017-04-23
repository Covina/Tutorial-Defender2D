using UnityEngine;

public class Enemy : MonoBehaviour {

	// which checkpoint to pathfind toward
	[SerializeField] private int targetPathNode = 0;

	// the level exit location
	[SerializeField] private Transform exitPoint;

	// store all checkpoints
	[SerializeField] private Transform[] wayPoints;

	// control checks on navigation
	[SerializeField] private float navigationUpdate;

	// control checks on navigation
	[SerializeField] private int healthPoints;

	// control checks on navigation
	[SerializeField] private int currencyReward;


	// unit speed
	public float walkSpeed;

	private float navigationTime = 0;

	// Component variables
	private Transform enemy;
	private Collider2D enemyCollider;
	private Animator animator;


	// Keep track if this enemy is dead or alive
	private bool isDead = false;

	// Getter for isDead
	public bool IsDead {

		get {
			return isDead;
		}

	}


	// Use this for initialization
	void Start () {

		// Get Component variables
		enemy = GetComponent<Transform>();
		enemyCollider = GetComponent<Collider2D>();
		animator = GetComponent<Animator>();

		// register this enemy
		GameManager.Instance.RegisterEnemy(this, "Start() in Enemy.cs");

	}
	
	// Update is called once per frame
	void Update ()
	{

		if (wayPoints != null && isDead != true) {

			// add time increments
			navigationTime += Time.deltaTime;

			// if we are ready to update
			if (navigationTime > navigationUpdate) {

				// if the target Node number is within the quantity available
				if (targetPathNode < wayPoints.Length) {

					// move the target
					enemy.position = Vector2.MoveTowards (enemy.position, wayPoints [targetPathNode].position, navigationTime);

				} else {
					// only remaining node is the exit
					enemy.position = Vector2.MoveTowards (enemy.position, exitPoint.position, navigationTime);
					
				}

				// reset timer
				navigationTime = 0;

			}

		}


	}



	public void EnemyHit (int hitpoints)
	{
		// check if we're at zero
		if (healthPoints - hitpoints > 0) {

			healthPoints -= hitpoints;

			animator.Play("hurt");


		} else {

			// enemy is dead
			Die();
		}

	}

	// Kill the enemy
	public void Die ()
	{
		isDead = true;
		enemyCollider.enabled = false;
		animator.SetTrigger("DieTrigger");

		// reward player for killing the enemy
		GameManager.Instance.AddCurrency(currencyReward);

		// record the enemy being killed
		//GameManager.Instance.UnregisterEnemy (this);
		GameManager.Instance.RoundEnemiesKilled += 1;
		GameManager.Instance.TotalEnemiesKilled += 1;

		// now check if wae is over
		GameManager.Instance.IsWaveOver ();


	}


	// to detect navigation changes as waypoints are hit
	void OnTriggerEnter2D (Collider2D collider)
	{

		if (collider.tag == "Checkpoint") {

			// change the target path node to move towards
			targetPathNode += 1;

		} else if (collider.tag == "Finish") {

			// enemy made it to the end alive, so remove it from the field
			GameManager.Instance.EnemyEscaped (this);
//			GameManager.Instance.UnregisterEnemy (this);


		} else if (collider.tag == "Projectile") {

			if (!gameObject) {
				Debug.Log("NRE - OnTrigger() this enemy does not exist");
			}
			if (!collider) {
				Debug.Log("NRE - OnTrigger() this collider object does not exist");
			}

			//Debug.Log("Enemy.cs OnTrigger for [" + gameObject.name + "];  Applying damage of [" + collider.gameObject.GetComponent<Projectile>().AttackStrength + "]");

			// apply the damage
			EnemyHit(collider.gameObject.GetComponent<Projectile>().AttackStrength);

			// destroy the projectile
			Destroy(collider.gameObject);
			

		}


	}

}
