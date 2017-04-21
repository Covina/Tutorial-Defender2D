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


	// unit speed
	public float walkSpeed;

	private Transform enemy;
	private Collider2D enemyCollider;
	private Animator animator;

	private float navigationTime = 0;




	private bool isDead = false;


	public bool IsDead {

		get {
			return isDead;
		}

	}


	// Use this for initialization
	void Start () {

		
		enemy = GetComponent<Transform>();
		enemyCollider = GetComponent<Collider2D>();
		animator = GetComponent<Animator>();

		// register the enemy
		GameManager.Instance.RegisterEnemy(this);

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


	// to detect navigation changes as waypoints are hit
	void OnTriggerEnter2D (Collider2D collider)
	{

		if (collider.tag == "Checkpoint") {

			// change the target path node to move towards
			targetPathNode += 1;

		} else if (collider.tag == "Finish") {

			GameManager.Instance.UnregisterEnemy (this);

		}  else if (collider.tag == "Projectile") {


			// apply the damage
			EnemyHit(collider.gameObject.GetComponent<Projectile>().AttackStrength);

			// destroy the projectile
			Destroy(collider.gameObject);
			

		}


	}


	public void EnemyHit (int hitpoints)
	{

		if (healthPoints - hitpoints > 0) {

			healthPoints -= hitpoints;

			animator.Play("hurt");


		} else {

			// TODO - die animation
			// TODO - Die method
			Die();
		}

	}


	public void Die ()
	{
		isDead = true;
		enemyCollider.enabled = false;
		animator.SetTrigger("DieTrigger");
	}

}
