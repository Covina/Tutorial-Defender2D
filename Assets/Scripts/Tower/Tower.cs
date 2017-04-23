using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour {

	// attack rate
	[SerializeField] private float timeBetweenAttacks;


	// how far the tower can attack
	[SerializeField] private float minAttackDistanceRadius;

	// how far the tower can attack
	[SerializeField] private float maxAttackDistanceRadius;

	// define the projectile to use
	[SerializeField] private Projectile projectile;

	// define the Tower Cost
	[SerializeField] private int towerCost;

	// current enemy target
	private Enemy targetEnemy = null;

	// collecting time between attacks
	private float attackTimeCounter;

	// store state of whether the tower is attacking
	private bool isAttacking = false;


	// Getter for the Tower Cost
	public int TowerCost {
		get {
			return towerCost;
		}

	}


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update ()
	{

		// decerment the attack counter;  Countdown to attack!
		attackTimeCounter -= Time.deltaTime;

		// if we don't already have a target, go get one
		if (targetEnemy == null || targetEnemy.IsDead) {

			// get nearest enemy
			Enemy nearestEnemy = GetNearestEnemyInRange ();

			// is the nearest enemy within our attack radius?
			if (nearestEnemy != null && Vector2.Distance (transform.position, nearestEnemy.transform.position) <= maxAttackDistanceRadius) {

				// if so, set the target.
				targetEnemy = nearestEnemy;

				//Debug.Log("Target acquired: " + targetEnemy.gameObject.name);

			}

		} else {

			// === We already have a target ===

			// if its attack time...
			if (attackTimeCounter <= 0) {

				// set attacking state to true
				isAttacking = true;

				// reset the attack timer
//				attackTimeCounter = timeBetweenAttacks;

			} else {

				// its not attack time, set it to false;
				isAttacking = false;

			}

			// check the objects current distance to see if we can still attack it
			if (Vector2.Distance (transform.position, targetEnemy.transform.position) > maxAttackDistanceRadius) {

				Debug.Log("Target enemy just went out of range.");

				// clear target enemy
				targetEnemy = null;
			}

		}



	}


	void FixedUpdate ()
	{
		if (isAttacking) {
			Attack(targetEnemy);
		}

	}


	public void Attack (Enemy currEnemy)
	{

		// set to false since we're attacking
		isAttacking = false;

		// Reset attack timer now that we've actually attacked
		attackTimeCounter = timeBetweenAttacks;


		// make the projectile
		Projectile newProjectile = Instantiate (projectile) as Projectile;
		newProjectile.transform.localPosition = transform.localPosition;

		// Play the right sound effect
		if (newProjectile.ProjectileType == ProjectileTypeEnum.arrow) {

			GameManager.Instance.MyAudioSource.PlayOneShot (AudioManager.Instance.SFXArrow);

		} else if (newProjectile.ProjectileType == ProjectileTypeEnum.fireball) {

			GameManager.Instance.MyAudioSource.PlayOneShot (AudioManager.Instance.SFXFireball);

		} else if (newProjectile.ProjectileType == ProjectileTypeEnum.rock) {

			GameManager.Instance.MyAudioSource.PlayOneShot (AudioManager.Instance.SFXRock);
		}



		// destroy projectile it if target is N/A
		if (currEnemy == null) {
			Destroy (newProjectile);
		} else {
			
			// TODO - move projectile to enemy
			StartCoroutine ( MoveProjectile(newProjectile, currEnemy) );

		}



	}



	IEnumerator MoveProjectile (Projectile projectile, Enemy currEnemy)
	{

		// if the enemy is far enough away, the projectile exists, and the enemy exists.
		while (getTargetDistance (currEnemy) > minAttackDistanceRadius && projectile != null && currEnemy != null) {

			// find the direction, subtract target from source
			var dir = currEnemy.transform.localPosition - transform.localPosition;

			// get Tangent and convert from radians to degrees
			var angleDirection = Mathf.Atan2 (dir.y, dir.x) * Mathf.Rad2Deg;

			// angle the projectile toward the target
			projectile.transform.rotation = Quaternion.AngleAxis (angleDirection, Vector3.forward);

			// move the projectile toward the target
			projectile.transform.localPosition = Vector2.MoveTowards (projectile.transform.localPosition, currEnemy.transform.localPosition, 5f * Time.deltaTime);

			yield return null;

		}

		// Sanity check in case things are bad
		if (projectile != null || currEnemy == null || currEnemy.IsDead) {

			Destroy(projectile);


		}


	}



	private float getTargetDistance (Enemy thisEnemy)
	{
		// if we passed in a null enemy, find a new one
		if (thisEnemy == null) {
			// find nearest enemy
			thisEnemy = GetNearestEnemyInRange ();

			// check again if that is null
			if (thisEnemy == null) {

				// zero enemies remaining
				return 0f;

			}
		}

		// Otherwise, return the distance to target.
		return Mathf.Abs(Vector2.Distance(transform.localPosition, thisEnemy.transform.localPosition));
		

	}


	// get all the enemies in the range
	private List<Enemy> GetEnemiesInRange ()
	{

		List<Enemy> enemiesInRange = new List<Enemy> ();

		if (GameManager.Instance.EnemyList.Count > 0) {
			// loop through to find enemies within the towers attack radius
			foreach (Enemy enemy in GameManager.Instance.EnemyList) {

				float enemyTargetDistance = Vector2.Distance (transform.localPosition, enemy.transform.localPosition);

				// measure the distance
				if (enemy != null && enemyTargetDistance <= maxAttackDistanceRadius && enemyTargetDistance >= minAttackDistanceRadius) {

					enemiesInRange.Add (enemy);
				}
			}

		} 

		return enemiesInRange;

	}


	private Enemy GetNearestEnemyInRange ()
	{

		Enemy nearestEnemy = null;
		float smallestDistance = float.PositiveInfinity;

		// loop through
		foreach (Enemy enemy in GetEnemiesInRange ()) {

			// measure the distance
			if (Vector2.Distance (transform.localPosition, enemy.transform.localPosition) < smallestDistance) {

				// set new smallest distance
				smallestDistance = Vector2.Distance (transform.localPosition, enemy.transform.localPosition);

				// set which enemy it belongs to
				nearestEnemy = enemy;
			}
			

		}


		return nearestEnemy;

	}


	public void OnDrawGizmos ()
	{
		// Draw min radius
		Gizmos.color = Color.blue;
		Gizmos.DrawWireSphere(transform.position, minAttackDistanceRadius);

		// Draw Max Radius
		Gizmos.color = Color.red;
		Gizmos.DrawWireSphere(transform.position, maxAttackDistanceRadius);


	}


}
