using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour {

	// attack rate
	[SerializeField] private float timBetweenAttacks;

	// how far the tower can attack
	[SerializeField] private float attackDistanceRadius;

	// define the projectile to use
	private Projectile projectile;

	// current enemy target
	private Enemy targetEnemy = null;

	// collecting time between attacks
	private float attackTimeCounter;


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	// get all the enemies in the range
	private List<Enemy> GetEnemiesInRange ()
	{

		List<Enemy> enemiesInRange = new List<Enemy> ();

		// loop through to find enemies within the towers attack radius
		foreach (Enemy enemy in GameManager.Instance.EnemyList) {

			// measure the distance
			if (Vector2.Distance (transform.position, enemy.transform.position) <= attackDistanceRadius) {

				enemiesInRange.Add(enemy);
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
			if (Vector2.Distance (transform.position, enemy.transform.position) < smallestDistance) {

				// set new smallest distance
				smallestDistance = Vector2.Distance (transform.position, enemy.transform.position);

				// set which enemy it belongs to
				nearestEnemy = enemy;
			}
			

		}


		return nearestEnemy;

	}


}
