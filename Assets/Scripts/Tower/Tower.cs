﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour {

	// attack rate
	[SerializeField] private float timeBetweenAttacks;

	// how far the tower can attack
	[SerializeField] private float attackDistanceRadius;

	// define the projectile to use
	[SerializeField] private Projectile projectile;

	// current enemy target
	private Enemy targetEnemy = null;

	// collecting time between attacks
	private float attackTimeCounter;

	private bool isAttacking = false;


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update ()
	{

		// decerment the attack counter;  Countdown to attack!
		attackTimeCounter -= Time.deltaTime;

		// if we don't already have a target, go get one
		if (targetEnemy == null) {

			// get nearest enemy
			Enemy nearestEnemy = GetNearestEnemyInRange ();

			// is the nearest enemy within our attack radius?
			if (nearestEnemy != null && Vector2.Distance (transform.position, nearestEnemy.transform.position) <= attackDistanceRadius) {

				// if so, set the target.
				targetEnemy = nearestEnemy;

			}

		} else {

			// if its attack time...
			if (attackTimeCounter <= 0) {
				isAttacking = true;

				// reset the attacck timer
				attackTimeCounter = timeBetweenAttacks;

			} else {

				isAttacking = false;

			}

			// check the objects current distance to see if we can still attack it
			if (Vector2.Distance (transform.position, targetEnemy.transform.position) > attackDistanceRadius) {

				// clear target enemy
				targetEnemy = null;
			}

		}



	}


	void FixedUpdate ()
	{
		if (isAttacking) {
			Attack();
		}

	}


	public void Attack ()
	{

		isAttacking = false;

		// make the projectile
		Projectile newProjectile = Instantiate (projectile) as Projectile;
		newProjectile.transform.localPosition = transform.localPosition;

		// destroy it if target is N/A
		if (targetEnemy == null) {
			Destroy (newProjectile);
		} else {
			
			// TODO - move projectile to enemy
			StartCoroutine ( MoveProjectile(newProjectile) );

		}

	}



	IEnumerator MoveProjectile (Projectile projectile)
	{

		// if the enemy is far enough away, the projectile exists, and the enemy exists.
		while (getTargetDistance (targetEnemy) > 0.20f && projectile != null && targetEnemy != null) {

			// find the direction, subtract target from source
			var dir = targetEnemy.transform.localPosition - transform.localPosition;

			// get Tangent and convert from radians to degrees
			var angleDirection = Mathf.Atan2 (dir.y, dir.x) * Mathf.Rad2Deg;

			// angle the projectile toward the target
			projectile.transform.rotation = Quaternion.AngleAxis (angleDirection, Vector3.forward);

			// move the projectile toward the target
			projectile.transform.localPosition = Vector2.MoveTowards (projectile.transform.localPosition, targetEnemy.transform.localPosition, 5f * Time.deltaTime);

			yield return null;

		}

		// Sanity check in case things are bad
		if (projectile != null || targetEnemy == null) {

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

		// loop through to find enemies within the towers attack radius
		foreach (Enemy enemy in GameManager.Instance.EnemyList) {

			// measure the distance
			if (Vector2.Distance (transform.localPosition, enemy.transform.localPosition) <= attackDistanceRadius) {

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
			if (Vector2.Distance (transform.localPosition, enemy.transform.localPosition) < smallestDistance) {

				// set new smallest distance
				smallestDistance = Vector2.Distance (transform.localPosition, enemy.transform.localPosition);

				// set which enemy it belongs to
				nearestEnemy = enemy;
			}
			

		}


		return nearestEnemy;

	}


}
